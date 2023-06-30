using System.ComponentModel;

namespace GProject.WebApplication.Models.Enums
{
    public enum ActiveDTO
    {
        [Description("Không sử dụng")]
        Active = 0,

        [Description("Sử dụng")]
        InActive = 1,
    }
}
