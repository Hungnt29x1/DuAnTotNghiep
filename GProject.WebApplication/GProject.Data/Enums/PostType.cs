using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GProject.Data.Enums
{
    public enum PostType
    {
        [Description("Tin tức mới")]
        News = 0,

        [Description("Tin tức nổi bật")]
        Hightlight = 1,

        [Description("Tin tức sự kiện")]
        Events = 2,

        [Description("Tin tức khác")]
        Other = 3
    }
}
