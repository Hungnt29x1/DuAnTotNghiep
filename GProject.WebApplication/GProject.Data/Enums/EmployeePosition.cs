using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GProject.Data.Enums
{
    public enum EmployeePosition
    {
        [Description("Nhân viên")]
        Empployee = 0,

        [Description("Quản lí")]
        Manager = 1
    }
}
