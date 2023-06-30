using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GProject.Data.Enums
{
    public enum CartDetailStatus
    {
        [Description("Sẵn sàng")]
        Ready = 0,

        [Description("Để sau")]
        Later = 1
    }
}
