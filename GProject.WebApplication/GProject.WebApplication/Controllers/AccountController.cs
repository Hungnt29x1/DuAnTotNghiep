using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.AspNetCore.Authorization;
using GProject.WebApplication.Models;
using GProject.WebApplication.Helpers;
using GProject.Data.DomainClass;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Mail;
using System.Net;
using GProject.WebApplication.Services;
using Microsoft.AspNetCore.Http;
using Twilio.TwiML.Messaging;

namespace GProject.WebApplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Customer> _userManager;
        private readonly SignInManager<Customer> _signInManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<Customer> userManager, SignInManager<Customer> signInManager, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("mess")))
                ViewData["Mess"] = HttpContext.Session.GetString("mess");
            HttpContext.Session.Remove("mess");
            ViewBag.returnUrl = returnUrl;
            return View();
        }

		[HttpPost]
        public async Task<IActionResult> Login(UserInfoDTO user, string returnUrl = null)
        {
            try
            {
                #region Đăng nhập khi đã tạo đc user
                ViewBag.Error = "Đăng nhập không thành công! Vui lòng nhập lại thông tin đăng nhập!";
                //--Kiểm tra dữ liệu đầu vào
                var Employees = await Commons.GetAll<Employee>(String.Concat(Commons.mylocalhost, "Employee/get-all-Employee"));
                Employee Emp = Employees.FirstOrDefault(c => c.Email.ToLower() == user.Email.ToLower() && c.Password == user.password && c.Status == 1);

                var Customers = await Commons.GetAll<Customer>(String.Concat(Commons.mylocalhost, "Customer/get-all-Customer"));
                Customer Cus = Customers.FirstOrDefault(c => c.Email.ToLower() == user.Email.ToLower() && c.Password == user.password && c.Status == 1);
                if (Emp != null)
                {
                    Commons.setObjectAsJson(HttpContext.Session, "userLogin", Emp);
                    var claims = new List<Claim>();
                    claims.Add(new Claim("username", Emp.Name));
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, Emp.EmployeeId));
                    claims.Add(new Claim(ClaimTypes.Email, Emp.EmployeeId));
                    claims.Add(new Claim(ClaimTypes.Name, Emp.Name));
                    claims.Add(new Claim(ClaimTypes.Country, user.Image != null ? user.Image : ""));
                    if (Emp.Position == Data.Enums.EmployeePosition.Manager)
                    {
                        HttpContext.Session.SetString("myRole", "manager");
                        claims.Add(new Claim(ClaimTypes.Role, "manager"));
                    }
                    else
                    {
                        HttpContext.Session.SetString("myRole", "employee");
                        claims.Add(new Claim(ClaimTypes.Role, "employee"));
                    }
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    await HttpContext.SignInAsync(claimsPrincipal);                   
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl) && !returnUrl.NullToString().Contains("Account"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {                       
                        return RedirectToAction("Index", "Dashboard");
                    }
                }
                else if (Cus != null)
                {
                    Commons.setObjectAsJson(HttpContext.Session, "userLogin", Cus);
                    HttpContext.Session.SetString("myRole", "customer");
                    var claims = new List<Claim>();
                    claims.Add(new Claim("username", Cus.Name));
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, Cus.Id.ToString()));
                    claims.Add(new Claim(ClaimTypes.Email, Cus.Email));
                    claims.Add(new Claim(ClaimTypes.Name, Cus.Name));
                    claims.Add(new Claim(ClaimTypes.Role, "customer"));
                    claims.Add(new Claim(ClaimTypes.Country, user.Image != null ? user.Image : ""));
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    await HttpContext.SignInAsync(claimsPrincipal);
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl) && !returnUrl.NullToString().Contains("Account"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
						return RedirectToAction("Index", "Product");
                    }
                }
                HttpContext.Session.SetString("mess", "Failed");
                return RedirectToAction("Login", "Account", new { returnUrl = returnUrl });
                #endregion
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [GProject.WebApplication.Services.Authorize]
        public async Task<IActionResult> Logout()
        {
			await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            HttpContext.Session.Remove("userLogin");
            HttpContext.Session.Remove("myRole");
            return Redirect("/");
        }
        [HttpGet]
        public ActionResult GetLocalhost(string str)
        {
            Commons.mylocalhost = str + "/api/";
            return Ok();
        }

        [HttpGet]
        public ActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { ReturnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
			if (remoteError != null)
			{
				ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
				return RedirectToAction(nameof(Login));
			}

			var info = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);
			var claims = info.Principal.Claims.ToList();
			// Lấy thông tin người dùng từ Google
			var userName = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
			var userEmail = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

			var lstCustomer = await Commons.GetAll<Customer>(String.Concat(Commons.mylocalhost, "Customer/get-all-Customer"));
            var existCus = lstCustomer.FirstOrDefault(c => c.Email.ToLower() == info.Principal.FindFirstValue(ClaimTypes.Email).ToLower());
            if (existCus == null)
            {
				existCus = new Customer()
				{
					Id = Guid.NewGuid(),
					Name = info.Principal.FindFirstValue(ClaimTypes.Name),
					Email = info.Principal.FindFirstValue(ClaimTypes.Email),
					CustomerId = Commons.RandomString(15),
					Password = "123456",
					CreateDate = DateTime.Now,
					DateOfBirth = DateTime.Now,
					PhoneNumber = "",
					Sex = 0,
					Address = "",
					Status = 1,
					Description = "",
					GoogleId = "",
					Image = "_customer.png"
				};
				//-- Gửi request cho api sử lí
				bool result = await Commons.Add_or_UpdateAsync(existCus, Commons.mylocalhost + "Customer/add-Customer");
				if (!result)
				{
					return RedirectToAction("Login", "Account", new { returnUrl = returnUrl });
				}
            }
			var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
			var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
			await HttpContext.SignInAsync(claimsPrincipal);
			Commons.setObjectAsJson(HttpContext.Session, "userLogin", existCus);
			if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl) && !returnUrl.NullToString().Contains("Account"))
			{
				return Redirect(returnUrl);
			}
			else
			{
				return RedirectToAction("Index", "Product");
			}
		}

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        public void SetViewBagInt(int? selected_id = null)
        {
            List<SexDTO> SexSTO = new List<SexDTO>()
                {
                    new SexDTO { ID = 0, Sex = "Nam"},
                    new SexDTO { ID = 1, Sex = "Nữ"},
                };
            ViewBag.Sex = new SelectList(SexSTO, "ID", "Sex", selected_id);
        }
        public IActionResult Register()
        {
            SetViewBagInt();
            return View();
        }
        public async Task<ActionResult> SaveRegister(CustomerDTO Customer)
        {
            try
            {
                string image = "";
                if (Customer.Image_Upload != null)
                {
                    string full_path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", Customer.Image_Upload.FileName);
                    using (var file = new FileStream(full_path, FileMode.Create))
                    {
                        Customer.Image_Upload.CopyTo(file);
                    }
                    image = Customer.Image_Upload.FileName;
                }
                SetViewBagInt(Customer.Sex);
                //-- Parse lại dữ liệu từ ViewModel
                var cus = new Customer()
                {
                    Id = Customer.Id,
                    Name = Customer.Name,
                    Email = Customer.Email,
                    CustomerId = Commons.RandomString(10),
                    Password = Customer.Password,
                    CreateDate = DateTime.Now,
                    DateOfBirth = Customer.DateOfBirth,
                    PhoneNumber = Customer.PhoneNumber,
                    Sex = Customer.Sex,
                    Address = Customer.Address,
                    Status = 1,
                    Description = Customer.Description,
                    Image = image
                };

                //-- Gửi request cho api sử lí
                bool result = await Commons.Add_or_UpdateAsync(cus, Commons.mylocalhost + "Customer/add-Customer");
                if (!result)
                    HttpContext.Session.SetString("mess", "Failed");
                else
                    HttpContext.Session.SetString("mess", "Success");
                return RedirectToAction("Login");
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult> CheckPhone(string PhoneNumber)
        {
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("myRole")) && HttpContext.Session.GetString("myRole").NullToString() == "customer")
                    return Json(new { success = false });
                var lstObjs = await Commons.GetAll<Customer>(String.Concat(Commons.mylocalhost, "Customer/get-all-Customer"));
                var employees = await Commons.GetAll<Employee>(String.Concat(Commons.mylocalhost, "Employee/get-all-Employee"));

                var existNameCustomer = lstObjs.Any(x => x.PhoneNumber == PhoneNumber );
                var existNameEmployee = employees.Any(x => x.PhoneNumber == PhoneNumber);

                if (!existNameCustomer && !existNameEmployee)
                    return Json(new { success = true });
                else
                    return Json(new { success = false });

            }
            catch (Exception)
            {
                return Json(new { success = false });
            }
        }
        [HttpPost]
        public async Task<ActionResult> CheckEmail(string Email)
        {
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("myRole")) && HttpContext.Session.GetString("myRole").NullToString() == "customer")
                    return Json(new { success = false });
                var lstObjs = await Commons.GetAll<Customer>(String.Concat(Commons.mylocalhost, "Customer/get-all-Customer"));
                var employees = await Commons.GetAll<Employee>(String.Concat(Commons.mylocalhost, "Employee/get-all-Employee"));
                var existNameCustomer = lstObjs.Any(x => x.Email.ToLower() == Email.ToLower());
                var existNameEmployee = employees.Any(x => x.Email.ToLower() == Email.ToLower());

                if (!existNameCustomer && !existNameEmployee)
                    return Json(new { success = true });
                else
                    return Json(new { success = false });
            }
            catch (Exception)
            {
                return Json(new { success = false });
            }
        }

        [HttpGet]
        [Route("Thong-tin-khach-hang.html", Name = ("ProfileCustomer"))]
        public async Task<ActionResult> UpdateProfileCustomer()
        {

            var customer = HttpContext.Session.GetObjectFromJson<Customer>("userLogin");

            if (customer == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var lstCustomer = await Commons.GetAll<Customer>(String.Concat(Commons.mylocalhost, "Customer/get-all-Customer"));
            var dataCustomer = lstCustomer.FirstOrDefault(c => c.Id == customer.Id);

            var userProfile = new UserProfileDTO()
            {
                Image = dataCustomer.Image,
                Name = dataCustomer.Name,
                Email = dataCustomer.Email,
                DateOfBirth = dataCustomer.DateOfBirth,
                PhoneNumber = dataCustomer.PhoneNumber,
                Address = dataCustomer.Address
            };
            return View(userProfile);
        }
        [HttpPost]
        [Route("Thong-tin-khach-hang.html", Name = ("ProfileCustomer"))]
        public async Task<ActionResult> UpdateProfileCustomer(UserProfileDTO user)
        {
            var customer = HttpContext.Session.GetObjectFromJson<Customer>("userLogin");
            if (user.Image_Upload != null)
            {
                string full_path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", user.Image_Upload.FileName);
                using (var file = new FileStream(full_path, FileMode.Create))
                {
                    user.Image_Upload.CopyTo(file);
                }
                customer.Image = user.Image_Upload.FileName;
            }
            else
            {
                customer.Image = customer.Image;
            }
            customer.Name = user.Name;
            customer.Email = user.Email;
            customer.DateOfBirth = user.DateOfBirth;
            customer.PhoneNumber = user.PhoneNumber;
            customer.Address = user.Address;
            customer.UpdateDate = DateTime.Now;
            string url = Commons.mylocalhost + "Customer/update-Customer";
            //Gửi request cho API để cập nhập thông tin
            bool result = await Commons.Add_or_UpdateAsync(customer, url);
            if (!result)
            {
                HttpContext.Session.SetString("mess", "Failed");
            }
            else
            {
                // Lưu thông tin người dùng mới vào session
                HttpContext.Session.SetString("userLogin", JsonConvert.SerializeObject(customer));
                HttpContext.Session.SetString("mess", "Success");
            }
            return RedirectToAction("Index", "Product");
        }

        [HttpGet]
        [Route("Thong-tin-nhan-vien.html", Name = ("ProfileEmployee"))]
        public async Task<ActionResult> UpdateProfileEmployee()
        {
            var employee = HttpContext.Session.GetObjectFromJson<Employee>("userLogin");

            if (employee == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var lstEmployee = await Commons.GetAll<Customer>(String.Concat(Commons.mylocalhost, "Employee/get-all-Employee"));
            var dataEmployee = lstEmployee.FirstOrDefault(c => c.Id == employee.Id);

            var userProfile = new UserProfileDTO()
            {
                Image = dataEmployee.Image,
                Name = dataEmployee.Name,
                Email = dataEmployee.Email,
                DateOfBirth = dataEmployee.DateOfBirth,
                PhoneNumber = dataEmployee.PhoneNumber,
                Address = dataEmployee.Address
            };
            return View(userProfile);
        }
        [HttpPost]
        [Route("Thong-tin-nhan-vien.html", Name = ("ProfileEmployee"))]
        public async Task<ActionResult> UpdateProfileEmployee(UserProfileDTO user)
        {
            var employee = HttpContext.Session.GetObjectFromJson<Employee>("userLogin");
            if (user.Image_Upload != null)
            {
                string full_path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", user.Image_Upload.FileName);
                using (var file = new FileStream(full_path, FileMode.Create))
                {
                    user.Image_Upload.CopyTo(file);
                }
                employee.Image = user.Image_Upload.FileName;
            }
            else
            {
                employee.Image = employee.Image;
            }
            employee.Name = user.Name;
            employee.Email = user.Email;
            employee.DateOfBirth = user.DateOfBirth;
            employee.PhoneNumber = user.PhoneNumber;
            employee.Address = user.Address;
            employee.UpdateDate = DateTime.Now;
            string url = Commons.mylocalhost + "Employee/update-Employee";
            //Gửi request cho API để cập nhập thông tin
            bool result = await Commons.Add_or_UpdateAsync(employee, url);
            if (!result)
            {
                HttpContext.Session.SetString("mess", "Failed");
            }
            else
            {
                // Lưu thông tin người dùng mới vào session
                HttpContext.Session.SetString("userLogin", JsonConvert.SerializeObject(employee));
                HttpContext.Session.SetString("mess", "Success");
            }
            return Redirect("Thong-tin-nhan-vien.html");
        }

        [HttpGet]
        [Route("Doi-mat-khau-khach-hang.html", Name = ("ChangepasswordCustomer"))]
        public async Task<ActionResult> ChangePasswordCustomer()
        {
            return View();
        }

        [HttpPost]
        [Route("Doi-mat-khau-khach-hang.html", Name = ("ChangepasswordCustomer"))]
        public async Task<ActionResult> ChangePasswordCustomer(ChangePasswordDTO changePasswordDTO)
        {
            var customer = HttpContext.Session.GetObjectFromJson<Customer>("userLogin");
            if (changePasswordDTO.CurrentPassword != customer.Password)
            {
                ViewData["checkCurrentPass"] = "Mật khẩu hiện tại không đúng";
                return RedirectToAction("ChangePasswordCustomer", "Account");
            }
            customer.Password = changePasswordDTO.NewPassword;
            customer.UpdateDate = DateTime.Now;
            string url = Commons.mylocalhost + "Customer/update-Customer";
            //Gửi request cho API để cập nhập thông tin
            bool result = await Commons.Add_or_UpdateAsync(customer, url);
            if (!result)
            {
                HttpContext.Session.SetString("mess", "Failed");
            }
            else
            {
                // Lưu thông tin người dùng mới vào session
                HttpContext.Session.SetString("userLogin", JsonConvert.SerializeObject(customer));
                HttpContext.Session.SetString("mess", "Success");
            }
            return RedirectToAction("Index", "Product");
        }

        [HttpGet]
        [Route("Doi-mat-khau-nhan-vien.html", Name = ("ChangepasswordEmployee"))]
        public async Task<ActionResult> ChangePasswordEmployee()
        {
            return View();
        }

        [HttpPost]
        [Route("Doi-mat-khau-nhan-vien.html", Name = ("ChangepasswordEmployee"))]
        public async Task<ActionResult> ChangePasswordEmployee(ChangePasswordDTO changePasswordDTO)
        {
            var employee = HttpContext.Session.GetObjectFromJson<Employee>("userLogin");
            if (changePasswordDTO.CurrentPassword != employee.Password)
            {
                ViewData["checkCurrentPass"] = "Mật khẩu hiện tại không đúng";
                return RedirectToAction("ChangePasswordEmployee", "Account");
            }
            employee.Password = changePasswordDTO.NewPassword;
            employee.UpdateDate = DateTime.Now;
            string url = Commons.mylocalhost + "Employee/update-Employee";
            //Gửi request cho API để cập nhập thông tin
            bool result = await Commons.Add_or_UpdateAsync(employee, url);
            if (!result)
            {
                HttpContext.Session.SetString("mess", "Failed");
                return RedirectToAction("ChangePasswordEmployee", "Account");
            }
            else
            {
                // Lưu thông tin người dùng mới vào session
                HttpContext.Session.SetString("userLogin", JsonConvert.SerializeObject(employee));
                HttpContext.Session.SetString("mess", "Success");
            }
            return RedirectToAction("Index", "Color");
        }

        [HttpGet]
        [Route("quen-mat-khau.html", Name = ("ForgotPassword"))]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [Route("quen-mat-khau.html", Name = ("ForgotPassword"))]
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email, string phoneNumber)
        {
            var lstCustomer = await Commons.GetAll<Customer>(String.Concat(Commons.mylocalhost, "Customer/get-all-Customer"));
            var userCustomer = lstCustomer.FirstOrDefault(c => c.Email == email && c.PhoneNumber == phoneNumber);

            var lstEmployee = await Commons.GetAll<Employee>(String.Concat(Commons.mylocalhost, "Employee/get-all-Employee"));
            var userEmployee = lstEmployee.FirstOrDefault(c => c.Email == email && c.PhoneNumber == phoneNumber);

            var code = GenerateRandomCode(6);

            if (userCustomer != null)
            {
                userCustomer.Password = code;
                string url = Commons.mylocalhost + "Customer/update-Customer";
                //Gửi request cho API để cập nhập thông tin
                bool result = await Commons.Add_or_UpdateAsync(userCustomer, url);

                var subject = "Reset your password";
                var body = $"Mật khẩu của bạn là: {code}. ";
                if (GuiMail(email, subject, body))
                {
                    HttpContext.Session.SetString("mess", "Success");
                }
                else
                    HttpContext.Session.SetString("mess", "Failed");
                HttpContext.Session.SetString("mess", "Success");
            }
            else if (userEmployee != null)
            {
                userEmployee.Password = code;
                string url = Commons.mylocalhost + "Employee/update-Employee";
                //Gửi request cho API để cập nhập thông tin
                bool result = await Commons.Add_or_UpdateAsync(userEmployee, url);

                var subject = "Reset your password";
                var body = $"Mật khẩu của bạn là: {code}. ";
                if (GuiMail(email, subject, body))
                {
                    HttpContext.Session.SetString("mess", "Success");
                }
                else
                    HttpContext.Session.SetString("mess", "Failed");
            }
            else
            {
                HttpContext.Session.SetString("mess", "Failed");
            }
            return RedirectToAction("ForgotPassword", "Account");
        }

        private string GenerateRandomCode(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public bool GuiMail(string to, string subject, string body)
        {
            try
            {
                string fromEmail = "hungntph17579@fpt.edu.vn";
                string password = "rzzkasitfwitfxhi";
                MailMessage message = new MailMessage();
                message.From = new MailAddress(fromEmail);
                message.Subject = subject;
                message.To.Add(new MailAddress(to));
                message.Body = body;
                message.IsBodyHtml = true;
                var smtpClient = new SmtpClient()
                {
                    Host = "smtp.gmail.com",
                    Port = 587,// cổng kết nối smtp
                    Credentials = new NetworkCredential(fromEmail, password),
                    EnableSsl = true, // mã hóa dữ liệu khi gửi mail
                    DeliveryMethod = SmtpDeliveryMethod.Network,//Đặt phthuc gửi mail là Network
                    UseDefaultCredentials = false,
                };
                smtpClient.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
