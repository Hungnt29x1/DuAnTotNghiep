using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GProject.Data.Enums
{
    public enum OrderStatus
    {
        [Description("Chờ xác nhận")]
        InProgress = 0,

        [Description("Đang vận chuyển")]
        Confirmed = 1,

        [Description("Đang giao hàng")]
        Delivery = 2,

        [Description("Hoàn thành")]
        Accomplished = 3,

        [Description("Trả hàng")]
        Returned = 4,

        [Description("Đã hủy")]
        Canceled = 5,

        [Description("Đã xác nhận")]
        DaXacNhan = 6,
    }
}
