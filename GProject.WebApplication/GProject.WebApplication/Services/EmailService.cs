using GProject.WebApplication.Services;
using System.Net.Mail;
using System.Net;
using GProject.Api.MyServices.IServices;
using GProject.Api.MyServices.Services;
using GProject.Data.MyRepositories.IRepositories;

namespace GProject.WebApplication.Services
{
    public class EmailService
    {
        private static ISendMailRepository sendMailRepository;
        public EmailService()
        {
            sendMailRepository = new SendMailRepository();
        }

        public static bool EMailSender(string from, string password, string to, string subject, string body, string createby)
        {
            try
            {
                bool SendResult = SendMail(from, password, to, subject, body);
                GProject.Data.DomainClass.SendMail sendmail = new GProject.Data.DomainClass.SendMail()
                {
                    Id = Guid.NewGuid(),
                    Sender = from,
                    Recipient = to,
                    CCEmail = "",
                    Subject = subject,
                    Message = body,
                    FileName = "",
                    CreateBy = createby,
                    CreateDate = DateTime.Now,
                    SortOrder = 0,
                    Status = !SendResult ? Data.Enums.SendMailStatus.Unsuccessfully : Data.Enums.SendMailStatus.Successfully,
                    Description = ""
                };
                sendMailRepository.Add(sendmail);
                return SendResult;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool SendMail(string from, string pass, string to, string subject, string body)
        {

            try
            {
                MailMessage message = new MailMessage();
                message.From = new MailAddress(from);
                message.Subject = subject;
                message.To.Add(new MailAddress(to));
                message.Body = body;
                message.IsBodyHtml = true;

                var smtpClient = new SmtpClient()
                {
                    Host = "smtp.gmail.com",
                    Port = 587,// cổng kết nối smtp
                    Credentials = new NetworkCredential(from, pass),
                    EnableSsl = true, // mã hóa dữ liệu khi gửi mail
                    DeliveryMethod = SmtpDeliveryMethod.Network,//Đặt phthuc gửi mail là Network
                    UseDefaultCredentials = false,
                };
                smtpClient.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}
