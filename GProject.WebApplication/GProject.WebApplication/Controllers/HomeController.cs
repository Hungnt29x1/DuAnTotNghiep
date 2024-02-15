using GProject.WebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Net.Mail;
using System.Net;

namespace GProject.WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("contact.html", Name = "Contact")]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [Route("contact.html", Name = "Contact")]
        public IActionResult Contact(ContactModel contact)
        {
            if (contact != null)
            {
                // Gửi email
                bool emailSent = SendEmail(contact);

                if (emailSent)
                {
                    return View(contact);
                }
                else
                {
                    return Json(new { success = false });
                }
            }

            return View(contact);
        }

        private bool SendEmail(ContactModel contact)
        {
            try
            {
                string smtpServer = "smtp.gmail.com";
                int smtpPort = 587;
                string smtpUsername = "hungnt29.x1@gmail.com"; // Thay thế bằng địa chỉ email thực tế
                string smtpPassword = "ewfn bayy kqxs iolz"; // Thay thế bằng mật khẩu email thực tế

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(smtpUsername);
                mail.To.Add(contact.Email); // Thay thế bằng địa chỉ email người nhận
                mail.Subject = contact.Subject;
                mail.Body = $@"
                            <html>
                            <head>
                                <style>
                                    body {{
                                        font-family: Arial, sans-serif;
                                        margin: 0;
                                        padding: 0;
                                    }}
                                    .container {{
                                        max-width: 600px;
                                        margin: 0 auto;
                                        padding: 20px;
                                        background-color: #f7f7f7;
                                    }}
                                    .header {{
                                        background-color: #3498db;
                                        color: #fff;
                                        text-align: center;
                                        padding: 10px 0;
                                    }}
                                    .content {{
                                        background-color: #fff;
                                        padding: 20px;
                                        border-radius: 5px;
                                        box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                                    }}
                                    .footer {{
                                        text-align: center;
                                        margin-top: 20px;
                                        color: #888;
                                    }}
                                </style>
                            </head>
                            <body>
                                <div class='container'>
                                    <div class='header'>
                                        <h1>Cảm ơn bạn đã phản hồi!</h1>
                                    </div>
                                    <div class='content'>
                                        <p>Xin chào {contact.FullName},</p>
                                        <p>Cảm ơn bạn đã phản hồi. Chúng tôi đã nhận được thông tin của bạn và sẽ xem xét nó cẩn thận.</p>
                                        <p>Chi tiết phản hồi:</p>
                                        <ul>
                                            <li><strong>Email:</strong> {contact.Email}</li>
                                            <li><strong>Tiêu đề:</strong> {contact.Subject}</li>
                                            <li><strong>Mô tả:</strong> {contact.Description}</li>
                                        </ul>
                                        <p>Chúng tôi sẽ liên hệ với bạn trong thời gian sớm nhất có thể.</p>
                                        <p>Xin cảm ơn và chúc bạn một ngày vui vẻ!</p>
                                    </div>
                                    <div class='footer'>
                                        <p>© 2023 Your Company. All rights reserved.</p>
                                    </div>
                                </div>
                            </body>
                            </html>";
                mail.IsBodyHtml = true;
                SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort);
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                smtpClient.EnableSsl = true;

                smtpClient.Send(mail);

                return true;
            }
            catch (Exception ex)
            {
                // Log lỗi hoặc xử lý lỗi tùy theo yêu cầu
                return true;
            }
        }
    }
}