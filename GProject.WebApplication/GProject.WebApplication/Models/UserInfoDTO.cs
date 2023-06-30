using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace GProject.WebApplication.Models
{
    public class UserInfoDTO
    {
        [DataMember(EmitDefaultValue = false)]
        //[Required(ErrorMessage = "Email không được bỏ trống")]
        [StringLength(100, ErrorMessage = "Email không được vượt quá 100 kí tự")]
        public string Email { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string? UserName { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string? phone_number { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string? address { get; set; }
        [DataMember(EmitDefaultValue = false)]
        //[Required(ErrorMessage = "Mật khẩu không được bỏ trống")]
        [StringLength(50, ErrorMessage = "Mật khẩu không được vượt quá 50 kí tự")]
        public string? password { get; set; }
        [DataMember(EmitDefaultValue = false)]
        //[Required(ErrorMessage = "Mật khẩu không được bỏ trống")]
        [StringLength(50, ErrorMessage = "Mật khẩu không được vượt quá 50 kí tự")]
        public string? new_password { get; set; }
        [DataMember(EmitDefaultValue = false)]
        //[Required(ErrorMessage = "Mật khẩu không được bỏ trống")]
        [StringLength(50, ErrorMessage = "Mật khẩu không được vượt quá 50 kí tự")]
        public string? confirm_password { get; set; }
        public string? Image { get; set; }
    }

}
