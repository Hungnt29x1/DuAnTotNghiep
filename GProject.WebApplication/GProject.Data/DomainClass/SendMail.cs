using GProject.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GProject.Data.DomainClass
{
    public class SendMail
    {
        public Guid? Id { get; set; }
        public string Sender { get; set; }
        public string Recipient { get; set; }
        public string CCEmail { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string? FileName { get; set; }
        public Byte? FileAttact { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int SortOrder { get; set; }
        public SendMailStatus Status { get; set; }
        public string? Description { get; set; }
    }
}
