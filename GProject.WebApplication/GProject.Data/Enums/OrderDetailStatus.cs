using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GProject.Data.Enums
{
    public enum OrderDetailStatus
    {
        [Description("Đang chờ")]
        Watting = 0,

        [Description("Đang bàn giao")]
        Handing = 1,

        [Description("Đang giao hàng")]
        Delivery = 2,

        [Description("Đã hoàn thành")]
        Accomplished = 3,

        [Description("Đã trả hàng")]
        Returned = 4,

        [Description("Đã hủy")]
        Canceled = 5
    }
}
