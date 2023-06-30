using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GProject.Data.Enums
{
    public enum SendMailStatus
    {
        [Description("Đã gửi")]
        Sent = 0,

        [Description("Gửi thành công")]
        Successfully = 1,

        [Description("Gửi không thành công")]
        Unsuccessfully = 2
    }
}
