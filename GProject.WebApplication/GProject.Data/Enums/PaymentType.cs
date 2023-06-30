using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GProject.Data.Enums
{
    public enum PaymentType
    {
        [Description("Thanh toán khi nhận hàng")]
        PayReceive = 0,

        [Description("Thanh toán ngay khi đặt hàng")]
        PayOrder = 1
    }
}
