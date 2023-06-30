using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GProject.Data.Enums
{
    public enum PromotionStatus
    {
        [Description("Chưa diễn ra")]
        NotHappen = 0,

        [Description("Đang diễn ra")]
        Happenning = 1,

        [Description("Đã kết thúc")]
        Happend = 2
    }
}
