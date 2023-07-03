using GProject.Data.DomainClass;
using GProject.WebApplication.Helpers;
using GProject.WebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using GProject.Data.MyRepositories.IRepositories;
using System.Net.Mail;
using System.Net;
using GProject.Data.Enums;
using GProject.WebApplication.Models.Payments;
using GProject.WebApplication.Services.IServices;
using X.PagedList;

namespace GProject.WebApplication.Controllers
{
    public class OrderController : Controller
    {
        private readonly IVnPayService _vnPayService;
        private ISendMailRepository sendMailRepository;
        private IOrderRepository orderRepository;
        private IOrderDetailRepository orderDetailRepository;
        private IProductVariationRepository productVariationRepository;
        private IProductRepository productRepository;
        private IColorRepository colorRepository;
        private ISizeRepository sizeRepository;
        private ICustomerRepository customerRepository;
        public OrderController(IVnPayService vnPayService)
        {
            _vnPayService = vnPayService;
            sendMailRepository = new SendMailRepository();
            orderRepository = new OrderRepository();
            orderDetailRepository = new OrderDetailRepository();
            productVariationRepository = new ProductVariationRepository();
            customerRepository = new CustomerRepository();
            productRepository = new ProductRepository();
            colorRepository = new ColorRepository();
            sizeRepository = new SizeRepository();
        }

        public async Task<ActionResult> Index(string sId, string sName, string sEmail, string sPhone, int? sPaymentType, string? sortOrder, int? page, int? history)
        {
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("myRole")) && HttpContext.Session.GetString("myRole").NullToString() == "customer")
                    return RedirectToAction("AccessDenied", "Account");
                int valsPaymentType = sPaymentType.HasValue ? sPaymentType.Value : -1;
                var lstObjs = await Commons.GetAll<Order>(String.Concat(Commons.mylocalhost, "Order/get-all-Order"));


                ViewData["totalInPro"] = lstObjs.Where(x => x.Status == OrderStatus.InProgress).ToList().Count();
                ViewData["totalConfi"] = lstObjs.Where(x => x.Status == OrderStatus.Confirmed).ToList().Count();
                ViewData["totalDelivery"] = lstObjs.Where(x => x.Status == OrderStatus.Delivery).ToList().Count();
                ViewData["totalAcc"] = lstObjs.Where(x => x.Status == OrderStatus.Accomplished).ToList().Count();
                ViewData["totalXCan"] = lstObjs.Where(x => x.Status == OrderStatus.Canceled).ToList().Count();
                ViewData["totalXacNhan"] = lstObjs.Where(x => x.Status == OrderStatus.DaXacNhan).ToList().Count();

                ViewData["InProgress"] = sortOrder == "InProgress" ? "NotSort" : "InProgress";
                ViewData["Confirmed"] = sortOrder == "Confirmed" ? "NotSort" : "Confirmed";
                ViewData["Delivery"] = sortOrder == "Delivery" ? "NotSort" : "Delivery";
                ViewData["Accomplished"] = sortOrder == "Accomplished" ? "NotSort" : "Accomplished";
                ViewData["Returned"] = sortOrder == "InProgress" ? "NotSort" : "Returned";
                ViewData["XacNhan"] = sortOrder == "DaXacNhan" ? "NotSort" : "DaXacNhan";
                ViewData["Canceled"] = sortOrder == "Canceled" ? "NotSort" : "Canceled";


                switch (sortOrder)
                {
                    case "InProgress":
                        lstObjs = lstObjs.Where(x => x.Status == OrderStatus.InProgress).ToList();
                        break;
                    case "Confirmed":
                        lstObjs = lstObjs.Where(x => x.Status == OrderStatus.Confirmed).ToList();
                        break;
                    case "Delivery":
                        lstObjs = lstObjs.Where(x => x.Status == OrderStatus.Delivery).ToList();
                        break;
                    case "Accomplished":
                        lstObjs = lstObjs.Where(x => x.Status == OrderStatus.Accomplished).ToList();
                        break;
                    case "Returned":
                        lstObjs = lstObjs.Where(x => x.Status == OrderStatus.Returned).ToList();
                        break;
                    case "Canceled":
                        lstObjs = lstObjs.Where(x => x.Status == OrderStatus.Canceled).ToList();
                        break;
                    case "DaXacNhan":
                        lstObjs = lstObjs.Where(x => x.Status == OrderStatus.DaXacNhan).ToList();
                        break;
                    case "NotSort":
                        lstObjs = lstObjs.ToList();
                        break;
                    default: break;
                }

                if (!string.IsNullOrEmpty(sId))
                    lstObjs = lstObjs.Where(c => c.OrderId.ToLower().Contains(sId.ToLower())).ToList();
                if (!string.IsNullOrEmpty(sName))
                    lstObjs = lstObjs.Where(c => c.ShippingFullName.ToLower().Contains(sName.ToLower())).ToList();
                if (!string.IsNullOrEmpty(sEmail))
                    lstObjs = lstObjs.Where(c => c.ShippingEmail.ToLower().Contains(sEmail.ToLower())).ToList();
                if (!string.IsNullOrEmpty(sPhone))
                    lstObjs = lstObjs.Where(c => c.ShippingPhone.ToLower().Contains(sPhone.ToLower())).ToList();
                if (valsPaymentType != -1)
                    lstObjs = lstObjs.Where(c => (int)c.PaymentType == sPaymentType).ToList();
                if (history == 1)
                    lstObjs = lstObjs.Where(c => c.HistoryLogChange != null).ToList();
                if (history == 0)
                    lstObjs = lstObjs.Where(c => c.HistoryLogChange == null).ToList();

                lstObjs = lstObjs.OrderBy(c => c.Status).ThenByDescending(c => c.CreateDate).ToList();

                // const int pageSize = 2;
                // if (pg < 1)
                //     pg = 1;
                //var pager = new Pager(lstObjs.Count(), pg, pageSize);
                // var lstData = lstObjs.Skip((pg - 1) * pageSize).Take(pageSize).ToList();
                if (page == null) page = 1;
                var pageNumber = page ?? 1;
                var pageSize = 5;
                var data = new OrderDto() { Orders = lstObjs.ToList() };

                //this.ViewBag.Pager = pager;
                this.ViewData[nameof(sId)] = (object)sId;
                this.ViewData[nameof(sName)] = (object)sName;
                this.ViewData[nameof(sEmail)] = (object)sEmail;
                this.ViewData[nameof(sPhone)] = (object)sPhone;
                this.ViewData[nameof(sPaymentType)] = (object)valsPaymentType;
                //-- truyền vào message nếu có thông báo
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("mess")))
                    ViewData["Mess"] = HttpContext.Session.GetString("mess");
                HttpContext.Session.Remove("mess");
                //List<Order> lsOrder = lstObjs;

                return View(lstObjs.ToPagedList(pageNumber, pageSize));

            }
            catch (Exception ex)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

        }

        public async Task<ActionResult> Detail(Guid id)
        {
            try
            {
                GProject.WebApplication.Services.OrderService pService = new GProject.WebApplication.Services.OrderService();
                return View(await pService.ShowMyOrder(id));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return RedirectToAction("AccessDenied", "Account");
            }
        }

        public async Task<ActionResult> Update(Guid? id)
        {
            try
            {
                GProject.WebApplication.Services.OrderService pService = new GProject.WebApplication.Services.OrderService();
                var Data = await pService.ShowMyOrder(id);
                ViewData["Data"] = Data;
                Order order = Data.Select(c => c.Order).FirstOrDefault();
                OrderViewModel orderViewModel = new OrderViewModel();
                orderViewModel.Id = order.Id;
                orderViewModel.CustomerId = order.CustomerId;
                orderViewModel.EmployeeId = order.EmployeeId;
                orderViewModel.DeliverId = order.DeliverId;
                orderViewModel.OrderId = order.OrderId;
                orderViewModel.CreateDate = order.CreateDate;
                orderViewModel.PaymentDate = order.PaymentDate;
                orderViewModel.PaymentType = order.PaymentType;
                orderViewModel.ShippingFullName = order.ShippingFullName;
                orderViewModel.ShippingCountry = order.ShippingCountry;
                orderViewModel.ShippingCity = order.ShippingCity;
                orderViewModel.ShippingDistrict = order.ShippingDistrict;
                orderViewModel.ShippingTown = order.ShippingTown;

                orderViewModel.ShippingAddress = order.ShippingAddress;
                orderViewModel.ShippingPhone = order.ShippingPhone;
                orderViewModel.ShippingEmail = order.ShippingEmail;
                orderViewModel.VoucherId = order.VoucherId;
                orderViewModel.DiscountRate = order.DiscountRate;
                orderViewModel.ShippingFee = order.ShippingFee;
                orderViewModel.TotalMoney = order.TotalMoney;
                orderViewModel.Status = order.Status;
                orderViewModel.Description = order.Description;
                orderViewModel.ReasonForChange = order.ReasonForChange;
                orderViewModel.HistoryLogChange = order.HistoryLogChange;

                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("mess")))
                    ViewData["Mess"] = HttpContext.Session.GetString("mess");
                HttpContext.Session.Remove("mess");
                return View(orderViewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return RedirectToAction("AccessDenied", "Account");
            }
        }
        public ActionResult ChangeStatus(string id, int status)
        {
            try
            {
                var employee = HttpContext.Session.GetObjectFromJson<Employee>("userLogin");
                var order = orderRepository.GetAll().Where(c => c.Id == new Guid(id)).FirstOrDefault();

                if (order != null)
                {
                    int oriStatus = (int)order.Status;
                    var LstorderDetail = orderDetailRepository.GetAll().Where(c => c.OrderId == order.Id).ToList();
                    order.Status = (Data.Enums.OrderStatus)status;
                    if (!orderRepository.Update(order))
                        HttpContext.Session.SetString("mess", "Failed");
                    else
                    {
                        switch (status)
                        {
                            case 6:
                                ChangQuantityInStock(oriStatus, status, LstorderDetail);
                                EMailSender(Commons.email, Commons.passemail, "huylqph18771@fpt.edu.vn", "Dream Fashion xin thông báo tới quý khách hàng", $"Thông báo đơn hàng {order.OrderId} được đặt vào {order.CreateDate} của bạn đã được xác nhận, chúng tôi sẽ giao hàng trong thời gian sớm nhất. Cảm ơn bạn đã sử dụng dịch vụ của chúng tôi!", employee.Email);
                                break;
                            case 5:
                                EMailSender(Commons.email, Commons.passemail, "huylqph18771@fpt.edu.vn", "Dream Fashion xin thông báo tới quý khách hàng", $"Thông báo đơn hàng {order.OrderId} được đặt vào {order.CreateDate} của bạn đã được hủy, chúng tôi xin lỗi vì sự bất tiện này. Cảm ơn bạn đã sử dụng dịch vụ của chúng tôi!", employee.Email);
                                //-- Hủy đơn hàng, trả lại số lượng còn lại của sản phẩm
                                ChangQuantityInStock(oriStatus, status, LstorderDetail);
                                break;
                            default:
                                ChangQuantityInStock(oriStatus, status, LstorderDetail);
                                break;
                        }

                        HttpContext.Session.SetString("mess", "Success");
                    }


                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return RedirectToAction("AccessDenied", "Account");
            }
        }

        /// <summary>
        /// Nếu mà đơn hàng đang 
        /// </summary>
        /// <param name="OriStatus"></param>
        /// <returns></returns>
        public void ChangQuantityInStock(int OriStatus, int changStatusVal, List<OrderDetail> orderDetails)
        {
            List<ProductVariation> variations = productVariationRepository.GetAll();
            //Nếu trạng thái đang là Hủy, và muốn đổi sang trạng thái khác Hủy, thì
            //cộng thêm số lượng ở orderDetail và giảm ở ProdVariation
            if (OriStatus == 5 && changStatusVal != 5)
            {
                foreach (var item in orderDetails)
                {
                    var prodVariation = variations.Where(c => c.Id == item.ProductVariationId).FirstOrDefault();
                    if (prodVariation != null)
                    {
                        prodVariation.QuantityInStock = prodVariation.QuantityInStock - item.Quantity;
                        productVariationRepository.Update(prodVariation);
                    }
                };
            }

            //Nếu trạng thái hiện tại khác hủy và muốn chuyển sang hủy, 
            //thì lại cộng thêm số lượng cho prodVariation
            if (OriStatus != 5 && changStatusVal == 5)
            {
                foreach (var item in orderDetails)
                {
                    var prodVariation = variations.Where(c => c.Id == item.ProductVariationId).FirstOrDefault();
                    if (prodVariation != null)
                    {
                        prodVariation.QuantityInStock = prodVariation.QuantityInStock + item.Quantity;
                        productVariationRepository.Update(prodVariation);
                    }
                };
            }
        }
        private static int _selectVoucher;
        private static string _cGiamGia;
        private static string _cShippingFee;
        private static string _cTotalMoney;
        private static string _ShippingEmail;
        private static string _cDescription;
        private static int _PaymentType;
        private static string _pTotalMoney;
        private static Guid? _customerId;
        private static string _ShippingFullName;
        private static string _ShippingPhone;
        private static string _Province;
        private static string _District;
        private static string _Wards;
        private static string _ShippingAddress;

        [HttpPost]
        public async Task<ActionResult> Order(
            int selectVoucher,
            string DiscountCode,
            string cGiamGia,
            string idDeliveryAddress,
            string cShippingFee,
            string cTotalMoney,
            string ShippingEmail,
            string cDescription,
            string ShippingFullName,
            string ShippingPhone,
            string Province,
            string District,
            string Wards,
            string ShippingAddress,
            int PaymentType = 0
        )
        {
            try
            {
                int voucherId = 0;
                var voucherList = (await Commons.GetAll<Voucher>(String.Concat(Commons.mylocalhost, "Voucher/get-all"))).ToList();
                if (selectVoucher != 0)
                    voucherId = selectVoucher;
                else
                {
                    if (!string.IsNullOrEmpty(DiscountCode))
                    {
                        var voucher = voucherList.FirstOrDefault(x => x.VoucherId == DiscountCode);
                        voucherId = voucher.Id;
                    }
                }
                List<ProdOrder> prodOrders = HttpContext.Session.GetObjectFromJson<List<ProdOrder>>("productOrders");

                var deliveryAddressesList = await Commons.GetAll<DeliveryAddress>(String.Concat(Commons.mylocalhost, "DeliveryAddress/get-all"));
                var dataDeliveryAddress = deliveryAddressesList.FirstOrDefault(x => x.Id == Convert.ToInt32(idDeliveryAddress));
                var lstCartDetail = await Commons.GetAll<CartDetail>(String.Concat(Commons.mylocalhost, "Cart/get-all-cart-detail"));
                var customer = HttpContext.Session.GetObjectFromJson<Customer>("userLogin");
                var getAllCustomer = await Commons.GetAll<Customer>(String.Concat(Commons.mylocalhost, "Customer/get-all-Customer"));
                var lstProductvariation = await Commons.GetAll<ProductVariation>(String.Concat(Commons.mylocalhost, "ProductVariation/get-all-ProductVariation"));

                if (customer != null)
                {
                    foreach (var item in prodOrders)
                    {
                        var cartDetail = lstCartDetail.FirstOrDefault(x => x.CartId == new Guid(item.cartId));
                        if (cartDetail != null)
                        {
                            if (cartDetail.Quantity != item.quantity && cartDetail.Quantity > item.quantity)
                            {
                                var quantity = cartDetail.Quantity - item.quantity;

                                ProductVariation productVariation = lstProductvariation.FirstOrDefault(c => c.Id == new Guid(item.prodVariationId));
                                productVariation.QuantityInStock = productVariation.QuantityInStock + quantity;
                                await Commons.Add_or_UpdateAsync(productVariation, String.Concat(Commons.mylocalhost, "ProductVariation/update-ProductVariation"));
                            }
                            if (cartDetail.Quantity != item.quantity && cartDetail.Quantity < item.quantity)
                            {
                                var quantity = item.quantity - cartDetail.Quantity;

                                ProductVariation productVariation = lstProductvariation.FirstOrDefault(c => c.Id == new Guid(item.prodVariationId));
                                productVariation.QuantityInStock = productVariation.QuantityInStock - quantity;
                                await Commons.Add_or_UpdateAsync(productVariation, String.Concat(Commons.mylocalhost, "ProductVariation/update-ProductVariation"));
                            }
                        }
                    }
                }


                if (customer == null)
                {
                    var checkCustomer = getAllCustomer.FirstOrDefault(x => x.PhoneNumber == ShippingPhone && x.Email == ShippingEmail);

                    if (checkCustomer != null)
                        _customerId = checkCustomer.Id;
                    else
                    {
                        var cus = new Customer()
                        {
                            Name = ShippingFullName,
                            Email = ShippingEmail,
                            CustomerId = Commons.RandomString(10),
                            Password = Commons.RandomString(8),
                            CreateDate = DateTime.Now,
                            DateOfBirth = DateTime.Now,
                            PhoneNumber = ShippingPhone,
                            Sex = 0,
                            Address = ShippingAddress,
                            Status = 1,
                            Description = null,
                            Image = "_customer.png"
                        };
                        await Commons.Add_or_UpdateAsync(cus, Commons.mylocalhost + "Customer/add-Customer");

                        var getCustomerNew = await Commons.GetAll<Customer>(String.Concat(Commons.mylocalhost, "Customer/get-all-Customer"));
                        _customerId = (getCustomerNew.FirstOrDefault(x => x.PhoneNumber == ShippingPhone && x.Email == ShippingEmail)).Id;

                    }

                    // Commons.setObjectAsJson(HttpContext.Session, "userNotLogin", new Customer()
                    //     {
                    //         Name = ShippingFullName,
                    //         Email = ShippingEmail,
                    //         Id = _customerId,
                    //     });

                    _ShippingFullName = ShippingFullName;
                    _ShippingPhone = ShippingPhone;
                    _ShippingAddress = ShippingAddress;
                    _Province = Province;
                    _District = District;
                    _Wards = Wards;
                }
                else
                {
                    _customerId = customer.Id;

                    _ShippingFullName = dataDeliveryAddress.Name;
                    _ShippingPhone = dataDeliveryAddress.PhoneNumber;
                    _Province = dataDeliveryAddress.ProvinceName;
                    _District = dataDeliveryAddress.DistrictName;
                    _Wards = dataDeliveryAddress.WardName;
                    _ShippingAddress = dataDeliveryAddress.Address;
                }


                if (PaymentType == 1)
                {
                    _selectVoucher = voucherId;
                    _cGiamGia = cGiamGia;
                    _cShippingFee = cShippingFee;
                    _cTotalMoney = cTotalMoney;
                    _ShippingEmail = ShippingEmail;
                    _cDescription = cDescription;
                    _PaymentType = PaymentType;

                    PaymentInformationModel model = new PaymentInformationModel()
                    {
                        Name = _ShippingFullName,
                        OrderDescription = "Thanh toán đơn hàng",
                        Amount = Convert.ToSingle(cTotalMoney),
                        OrderType = "electric"
                    };
                    var url = _vnPayService.CreatePaymentUrl(model, HttpContext);

                    return Redirect(url);
                }


                GProject.WebApplication.Services.OrderService pService = new GProject.WebApplication.Services.OrderService();
                bool result = await pService.AddToOrder(
                    voucherId,
                    cGiamGia,
                    cShippingFee,
                    cTotalMoney,
                    _ShippingFullName,
                    _ShippingPhone,
                    _Province,
                    _District,
                    _Wards,
                    _ShippingAddress,
                    ShippingEmail,
                    cDescription,
                    PaymentType,
                    _customerId,
                    prodOrders);

                if (!result)
                    HttpContext.Session.SetString("mess", "Failed");
                else
                {

                    var dataVoucher = new UpdateNumberVoucherDto() { Id = voucherId };
                    await Commons.Add_or_UpdateAsync(dataVoucher, String.Concat(Commons.mylocalhost, "Voucher/update-number"));

                    HttpContext.Session.SetString("mess", "Success");
                    ViewData["Mess"] = HttpContext.Session.GetString("mess");
                }

                if (customer == null)
                {
                    List<CartDetail> cart_Details = HttpContext.Session.GetObjectFromJson<List<CartDetail>>("add_cart_details");
                    foreach (var item in prodOrders)
                    {
                        var delete_cart_Details = cart_Details.FirstOrDefault(x => x.ProductVariationId.ToString() == item.prodVariationId);
                        if (delete_cart_Details != null)
                            cart_Details.Remove(delete_cart_Details);
                    }

                    Commons.setObjectAsJson(HttpContext.Session, "add_cart_details", cart_Details);
                    //return RedirectToAction("ShowDetailMyCart", "Product");
                }

                HttpContext.Session.Remove("productOrders");
                return RedirectToAction("ViewOrderCustomer", "Order");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return RedirectToAction("AccessDenied", "Account");
            }
        }

        [HttpPost]
        public async Task<ActionResult> BuyNow(
            int selectVoucher,
            string DiscountCode,
            string cGiamGia,
            string? idDeliveryAddress,
            string cShippingFee,
            string pTotalMoney,
            string ShippingEmail,
            string cDescription,
            string ShippingFullName,
            string ShippingPhone,
            string Province,
            string District,
            string Wards,
            string ShippingAddress,
            int PaymentType = 0
        )
        {
            try
            {
                int voucherId = 0;
                var voucherList = (await Commons.GetAll<Voucher>(String.Concat(Commons.mylocalhost, "Voucher/get-all"))).ToList();
                if (selectVoucher != 0)
                    voucherId = selectVoucher;
                else
                {
                    if (!string.IsNullOrEmpty(DiscountCode))
                    {
                        var voucher = voucherList.FirstOrDefault(x => x.VoucherId == DiscountCode);
                        voucherId = voucher.Id;
                    }
                }
                List<ProdOrder> prodOrders = HttpContext.Session.GetObjectFromJson<List<ProdOrder>>("productOrders");
                var lstCartDetail = await Commons.GetAll<CartDetail>(String.Concat(Commons.mylocalhost, "Cart/get-all-cart-detail"));
                var getAllCustomer = await Commons.GetAll<Customer>(String.Concat(Commons.mylocalhost, "Customer/get-all-Customer"));
                var deliveryAddressesList = await Commons.GetAll<DeliveryAddress>(String.Concat(Commons.mylocalhost, "DeliveryAddress/get-all"));
                var dataDeliveryAddress = deliveryAddressesList.FirstOrDefault(x => x.Id == Convert.ToInt32(idDeliveryAddress));

                var customer = HttpContext.Session.GetObjectFromJson<Customer>("userLogin");


                if (customer == null)
                {
                    var checkCustomer = getAllCustomer.FirstOrDefault(x => x.PhoneNumber == ShippingPhone && x.Email == ShippingEmail);

                    if (checkCustomer != null)
                        _customerId = checkCustomer.Id;
                    else
                    {
                        var cus = new Customer()
                        {
                            Name = ShippingFullName,
                            Email = ShippingEmail,
                            CustomerId = Commons.RandomString(10),
                            Password = Commons.RandomString(8),
                            CreateDate = DateTime.Now,
                            DateOfBirth = DateTime.Now,
                            PhoneNumber = ShippingPhone,
                            Sex = 0,
                            Address = ShippingAddress,
                            Status = 1,
                            Description = null,
                            Image = "_customer.png"
                        };
                        await Commons.Add_or_UpdateAsync(cus, Commons.mylocalhost + "Customer/add-Customer");

                        var getCustomerNew = await Commons.GetAll<Customer>(String.Concat(Commons.mylocalhost, "Customer/get-all-Customer"));
                        _customerId = (getCustomerNew.FirstOrDefault(x => x.PhoneNumber == ShippingPhone && x.Email == ShippingEmail)).Id;
                    }

                    _ShippingFullName = ShippingFullName;
                    _ShippingPhone = ShippingPhone;
                    _ShippingAddress = ShippingAddress;
                    _Province = Province;
                    _District = District;
                    _Wards = Wards;
                }
                else
                {
                    _customerId = customer.Id;

                    _ShippingFullName = dataDeliveryAddress.Name;
                    _ShippingPhone = dataDeliveryAddress.PhoneNumber;
                    _Province = dataDeliveryAddress.ProvinceName;
                    _District = dataDeliveryAddress.DistrictName;
                    _Wards = dataDeliveryAddress.WardName;
                    _ShippingAddress = dataDeliveryAddress.Address;
                }


                if (PaymentType == 1)
                {
                    _selectVoucher = voucherId;
                    _cGiamGia = cGiamGia;
                    _cShippingFee = cShippingFee;
                    _pTotalMoney = pTotalMoney;
                    _ShippingEmail = ShippingEmail;
                    _cDescription = cDescription;
                    _PaymentType = PaymentType;

                    PaymentInformationModel model = new PaymentInformationModel()
                    {
                        Name = _ShippingFullName,
                        OrderDescription = "Thanh toán đơn hàng",
                        Amount = Convert.ToSingle(pTotalMoney),
                        OrderType = "electric"
                    };
                    var url = _vnPayService.CreatePaymentUrl(model, HttpContext);

                    return Redirect(url);
                }

                GProject.WebApplication.Services.OrderService pService = new GProject.WebApplication.Services.OrderService();
                bool result = await pService.BuyNow(
                    voucherId,
                    cGiamGia,
                    cShippingFee,
                    pTotalMoney,
                    _ShippingFullName,
                    _ShippingPhone,
                    _Province,
                    _District,
                    _Wards,
                    _ShippingAddress,
                    ShippingEmail,
                    cDescription,
                    PaymentType,
                    _customerId,
                    prodOrders);

                if (!result)
                    HttpContext.Session.SetString("mess", "Failed");
                else
                {
                    var dataVoucher = new UpdateNumberVoucherDto() { Id = voucherId };
                    await Commons.Add_or_UpdateAsync(dataVoucher, String.Concat(Commons.mylocalhost, "Voucher/update-number"));
                    HttpContext.Session.SetString("mess", "Success");
                }

                HttpContext.Session.Remove("productOrders");
                return RedirectToAction("ProductDetail", "Product", new { product_id = new Guid(prodOrders.First().productId) });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return RedirectToAction("AccessDenied", "Account");
            }
        }

        public bool EMailSender(string from, string password, string to, string subject, string body, string createby)
        {
            try
            {
                bool SendResult = SendMail(from, password, to, subject, body);
                GProject.Data.DomainClass.SendMail sendmail = new GProject.Data.DomainClass.SendMail()
                {
                    Id = Guid.NewGuid(),
                    Sender = from,
                    Recipient = to,
                    CCEmail = "",
                    Subject = subject,
                    Message = body,
                    FileName = "",
                    CreateBy = createby,
                    CreateDate = DateTime.Now,
                    SortOrder = 0,
                    Status = !SendResult ? Data.Enums.SendMailStatus.Unsuccessfully : Data.Enums.SendMailStatus.Successfully,
                    Description = ""
                };
                sendMailRepository.Add(sendmail);
                return SendResult;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool SendMail(string from, string pass, string to, string subject, string body)
        {

            try
            {
                MailMessage message = new MailMessage();
                message.From = new MailAddress(from);
                message.Subject = subject;
                message.To.Add(new MailAddress(to));
                message.Body = body;
                message.IsBodyHtml = true;

                var smtpClient = new SmtpClient()
                {
                    Host = "smtp.gmail.com",
                    Port = 587,// cổng kết nối smtp
                    Credentials = new NetworkCredential(from, pass),
                    EnableSsl = true, // mã hóa dữ liệu khi gửi mail
                    DeliveryMethod = SmtpDeliveryMethod.Network,//Đặt phthuc gửi mail là Network
                    UseDefaultCredentials = false,
                };
                smtpClient.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }


        [HttpGet]
        public async Task<ActionResult> CheckOrderStatus(Guid? accomplished, Guid? canceled)
        {
            try
            {
                if (accomplished.HasValue)
                {
                    var lstOrder = await Commons.GetAll<Order>(String.Concat(Commons.mylocalhost, "Order/order-accomplished?id=" + accomplished));
                    return RedirectToAction("ViewOrderCustomer", "Order");
                }
                if (canceled.HasValue)
                {
                    var order = orderRepository.GetAll().Where(c => c.Id == canceled).FirstOrDefault();
                    var LstorderDetail = orderDetailRepository.GetAll().Where(c => c.OrderId == order.Id).ToList();
                    ChangQuantityInStock((int)order.Status, 5, LstorderDetail);
                    var lstOrder = await Commons.GetAll<Order>(String.Concat(Commons.mylocalhost, "Order/order-canceled?id=" + canceled));
                    return RedirectToAction("ViewOrderCustomer", "Order");
                }
                return View();
            }
            catch (Exception)
            {

                return RedirectToAction("AccessDenied", "Account");
            }
        }

        public async Task<ActionResult> ViewOrderCustomer(string? sortOrder, string? orderId, string? phone, int pg = 1, int pageSize = 10)
        {
            try
            {
                GProject.WebApplication.Services.ProductService pService = new GProject.WebApplication.Services.ProductService();
                var customer = HttpContext.Session.GetObjectFromJson<Customer>("userLogin");


                //-- Lấy danh sách từ api
                var lstOrder = await Commons.GetAll<Order>(String.Concat(Commons.mylocalhost, "Order/get-all-Order"));
                var lstProductvariation = await pService.ShowMyOrder(null);
                //lstOrder = lstOrder.Where(x => x.CustomerId == customer.Id).ToList();

                ViewData["totalInPro"] = lstOrder.Where(x => x.Status == OrderStatus.InProgress).ToList().Count();
                ViewData["totalConfi"] = lstOrder.Where(x => x.Status == OrderStatus.Confirmed).ToList().Count();
                ViewData["totalDelivery"] = lstOrder.Where(x => x.Status == OrderStatus.Delivery).ToList().Count();
                ViewData["totalAcc"] = lstOrder.Where(x => x.Status == OrderStatus.Accomplished).ToList().Count();
                ViewData["totalXCan"] = lstOrder.Where(x => x.Status == OrderStatus.Canceled).ToList().Count();
                ViewData["totalXacNhan"] = lstOrder.Where(x => x.Status == OrderStatus.DaXacNhan).ToList().Count();

                ViewData["InProgress"] = sortOrder == "InProgress" ? "NotSort" : "InProgress";
                ViewData["Confirmed"] = sortOrder == "Confirmed" ? "NotSort" : "Confirmed";
                ViewData["Delivery"] = sortOrder == "Delivery" ? "NotSort" : "Delivery";
                ViewData["Accomplished"] = sortOrder == "Accomplished" ? "NotSort" : "Accomplished";
                ViewData["Returned"] = sortOrder == "InProgress" ? "NotSort" : "Returned";
                ViewData["Canceled"] = sortOrder == "Canceled" ? "NotSort" : "Canceled";
                ViewData["XacNhan"] = sortOrder == "DaXacNhan" ? "NotSort" : "DaXacNhan";

                if (sortOrder == null && orderId == null && phone == null)
                    lstOrder = lstOrder.ToList();

                switch (sortOrder)
                {
                    case "InProgress":
                        lstOrder = lstOrder.Where(x => x.Status == OrderStatus.InProgress).ToList();
                        break;
                    case "Confirmed":
                        lstOrder = lstOrder.Where(x => x.Status == OrderStatus.Confirmed).ToList();
                        break;
                    case "Delivery":
                        lstOrder = lstOrder.Where(x => x.Status == OrderStatus.Delivery).ToList();
                        break;
                    case "Accomplished":
                        lstOrder = lstOrder.Where(x => x.Status == OrderStatus.Accomplished).ToList();
                        break;
                    case "Returned":
                        lstOrder = lstOrder.Where(x => x.Status == OrderStatus.Returned).ToList();
                        break;
                    case "Canceled":
                        lstOrder = lstOrder.Where(x => x.Status == OrderStatus.Canceled).ToList();
                        break;
                    case "DaXacNhan":
                        lstOrder = lstOrder.Where(x => x.Status == OrderStatus.DaXacNhan).ToList();
                        break;
                    case "NotSort":
                        lstOrder = lstOrder.ToList();
                        break;
                    default:
                        lstOrder = lstOrder.ToList(); 
                        break;
                }

                if (orderId != null)
                {
                    lstOrder = lstOrder.Where(x => x.OrderId == orderId).ToList();
                    @ViewData["orderId"] = orderId;
                }

                if (phone != null)
                {
                    lstOrder = lstOrder.Where(x => x.ShippingPhone == phone).ToList();
                    @ViewData["phone"] = phone;
                }

                //if (pg < 1)
                //    pg = 1;

                //int recsCount = lstOrder.Count();
                //var pager = new Pager(recsCount, pg, pageSize);
                //int recSkip = (pg - 1) * pageSize;
                //lstOrder = lstOrder.Skip(recSkip).Take(pageSize).ToList();

                if (customer == null && (orderId != null || phone != null))
                {
                    return View(new OrderDTO() { OrderList = lstOrder.OrderByDescending(x => x.CreateDate).ToList(), ShowOrderDtoList = lstProductvariation });
                }

                if (customer == null)
                {
                    return View(new OrderDTO() { OrderList = new List<Order>(), ShowOrderDtoList = new List<ShowOrderDto>() });
                }

                var data = new OrderDTO() { OrderList = lstOrder.Where(x => x.CustomerId == customer.Id).OrderByDescending(x => x.CreateDate).ToList(), ShowOrderDtoList = lstProductvariation };

                //-- truyền vào message nếu có thông báo
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("mess")))
                    ViewData["Mess"] = HttpContext.Session.GetString("mess");
                HttpContext.Session.Remove("mess");
                return View(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> PaymentCallback()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);

            if (!response)
            {
                ViewBag.Message = response;
                HttpContext.Session.SetString("mess", "Failed");
                return RedirectToAction("Index", "Product");
            }

            List<ProdOrder> prodOrders = HttpContext.Session.GetObjectFromJson<List<ProdOrder>>("productOrders");
            GProject.WebApplication.Services.OrderService pService = new GProject.WebApplication.Services.OrderService();

            bool result = false;
            if (_pTotalMoney == null)
            {
                result = await pService.AddToOrder(
                    _selectVoucher,
                    _cGiamGia,
                    _cShippingFee,
                    _cTotalMoney,
                    _ShippingFullName,
                    _ShippingPhone,
                    _Province,
                    _District,
                    _Wards,
                    _ShippingAddress,
                    _ShippingEmail,
                    _cDescription,
                    _PaymentType,
                    _customerId,
                    prodOrders);
            }
            else if (_cTotalMoney == null)
            {
                result = await pService.BuyNow(
                    _selectVoucher,
                    _cGiamGia,
                    _cShippingFee,
                    _pTotalMoney,
                    _ShippingFullName,
                    _ShippingPhone,
                    _Province,
                    _District,
                    _Wards,
                    _ShippingAddress,
                    _ShippingEmail,
                    _cDescription,
                    _PaymentType,
                    _customerId,
                    prodOrders);

                if (!result)
                    HttpContext.Session.SetString("mess", "Failed");
                else
                {
                    var dataVoucher = new UpdateNumberVoucherDto() { Id = _selectVoucher };
                    await Commons.Add_or_UpdateAsync(dataVoucher, String.Concat(Commons.mylocalhost, "Voucher/update-number"));
                    HttpContext.Session.SetString("mess", "Success");
                }
                ViewBag.Message = response;
                return RedirectToAction("ProductDetail", "Product", new { product_id = new Guid(prodOrders.First().productId) });
            }

            if (!result)
                HttpContext.Session.SetString("mess", "Failed");
            else
            {
                var dataVoucher = new UpdateNumberVoucherDto() { Id = _selectVoucher };
                await Commons.Add_or_UpdateAsync(dataVoucher, String.Concat(Commons.mylocalhost, "Voucher/update-number"));
                HttpContext.Session.SetString("mess", "Success");
                ViewData["Mess"] = HttpContext.Session.GetString("mess");
            }

            ViewBag.Message = response;
            return RedirectToAction("ViewOrderCustomer", "Order");
        }

        public async Task<ActionResult> RemoveOrderDetail(Guid id, Guid prodId)
        {
            try
            {
                var employee = HttpContext.Session.GetObjectFromJson<Employee>("userLogin");
                bool result = true;
                var orDetail = orderDetailRepository.GetAll().FirstOrDefault(c => c.OrderId == id && c.ProductVariationId == prodId);
                if (orDetail != null)
                {
                    var prodvariation = productVariationRepository.GetAll().FirstOrDefault(c => c.Id == orDetail.ProductVariationId);
                    if (prodvariation != null)
                    {
                        prodvariation.QuantityInStock = prodvariation.QuantityInStock + orDetail.Quantity;
                        result = productVariationRepository.Update(prodvariation);
                    }
                    if (orderDetailRepository.Delete(orDetail))
                    {
                        var order = orderRepository.GetAll().FirstOrDefault(c => c.Id == orDetail.OrderId);
                        decimal totalPaymentChange = order.TotalMoney - orDetail.TotalMoney;
                        string strChange = $"Thời gian: {DateTime.Now} \nNgười thay đổi: {employee.Email}";
                        strChange += $"\n Sản phẩm {prodId} từ bị gỡ khỏi giỏ hàng";
                        strChange += $"\n Tổng tiền thanh toán từ {order.TotalMoney.ToString("#,##0.##")} -> {totalPaymentChange.ToString("#,##0.##")}";
                        order.TotalMoney = totalPaymentChange;
                        order.HistoryLogChange = strChange + order.HistoryLogChange.NullToString();
                        result = orderRepository.Update(order);
                    }

                }
                if (!result)
                    HttpContext.Session.SetString("mess", "Failed");
                else
                    HttpContext.Session.SetString("mess", "Success");
                return Json(result);
            }
            catch (Exception ex)
            {
                return RedirectToAction("AccessDenied", "Account");
            }
        }

        [HttpPost]
        public async Task<ActionResult> ChangeInfoOrder(OrderViewModel orderDTO)
        {
            try
            {
                var employee = HttpContext.Session.GetObjectFromJson<Employee>("userLogin");
                var order = orderRepository.GetAll().FirstOrDefault(c => c.Id == orderDTO.Id);

                #region Ghi log thay đổi 
                string strChange = $"Thời gian: {DateTime.Now} \nNgười thay đổi: {employee.Email}";
                if (order.ShippingFullName != orderDTO.ShippingFullName)
                    strChange += $"\n Tên người nhận từ {order.ShippingFullName} -> {orderDTO.ShippingFullName}";
                if (order.ShippingCity != orderDTO.ShippingCity || order.ShippingDistrict != orderDTO.ShippingDistrict || order.ShippingTown != orderDTO.ShippingTown || order.ShippingAddress != orderDTO.ShippingAddress)
                    strChange += $"\n Địa chỉ từ {order.ShippingAddress},{order.ShippingTown},{order.ShippingDistrict},{order.ShippingCity} -> " +
                        $"{orderDTO.ShippingAddress},{orderDTO.ShippingTown},{orderDTO.ShippingDistrict},{orderDTO.ShippingCity}";
                if (order.ShippingPhone != orderDTO.ShippingPhone)
                    strChange += $"\n Số điện thoại từ {order.ShippingPhone} -> {orderDTO.ShippingPhone}";
                if (order.ShippingEmail != orderDTO.ShippingEmail)
                    strChange += $"\n Email từ {order.ShippingEmail} -> {orderDTO.ShippingEmail}";
                if (order.DiscountRate != orderDTO.DiscountRate)
                    strChange += $"\n Mức giảm giá từ {order.DiscountRate} -> {orderDTO.DiscountRate}";
                if (order.ShippingFee != orderDTO.ShippingFee)
                    strChange += $"\n Phí giao hàng từ {order.ShippingFee} -> {orderDTO.ShippingFee}";
                if (order.TotalMoney != orderDTO.TotalMoney)
                    strChange += $"\n Tổng tiền từ {order.TotalMoney} -> {orderDTO.TotalMoney}";

                #endregion


                order.Id = orderDTO.Id;
                order.CustomerId = orderDTO.CustomerId;
                order.EmployeeId = orderDTO.EmployeeId;
                order.DeliverId = orderDTO.DeliverId;
                order.OrderId = orderDTO.OrderId;
                order.CreateDate = orderDTO.CreateDate;
                order.UpdateDate = DateTime.Now;
                order.PaymentDate = orderDTO.PaymentDate;
                order.PaymentType = orderDTO.PaymentType;
                order.ShippingFullName = orderDTO.ShippingFullName;
                order.ShippingCountry = orderDTO.ShippingCountry;
                order.ShippingCity = orderDTO.ShippingCity;
                order.ShippingDistrict = orderDTO.ShippingDistrict;
                order.ShippingTown = orderDTO.ShippingTown;
                order.ShippingAddress = orderDTO.ShippingAddress;
                order.ShippingPhone = orderDTO.ShippingPhone;
                order.ShippingEmail = orderDTO.ShippingEmail;
                order.VoucherId = orderDTO.VoucherId;
                order.DiscountRate = orderDTO.DiscountRate;
                order.ShippingFee = orderDTO.ShippingFee;
                order.TotalMoney = orderDTO.TotalMoney;
                order.Status = orderDTO.Status;
                order.Description = orderDTO.Description;

                strChange += $"<------------------------------------------>";
                order.HistoryLogChange = strChange + order.HistoryLogChange.NullToString();


                if (!orderRepository.Update(order))
                    HttpContext.Session.SetString("mess", "Failed");
                else
                    HttpContext.Session.SetString("mess", "Success");
                return RedirectToAction("Index", "Order");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return RedirectToAction("AccessDenied", "Account");
            }
        }

        public async Task<ActionResult> ChangeQuantityOrder(string orderid, string prodId, string quantity)
        {
            try
            {
                bool result = true;
                var employee = HttpContext.Session.GetObjectFromJson<Employee>("userLogin");
                string strChange = $"Thời gian: {DateTime.Now} \nNgười thay đổi: {employee.Email}";
                var orDetail = orderDetailRepository.GetAll().FirstOrDefault(c => c.OrderId == new Guid(orderid) && c.ProductVariationId == new Guid(prodId));

                if (orDetail != null)
                {
                    var prodvariation = productVariationRepository.GetAll().FirstOrDefault(c => c.Id == orDetail.ProductVariationId);
                    if (prodvariation != null)
                    {
                        prodvariation.QuantityInStock = prodvariation.QuantityInStock + quantity.NullToInt() - orDetail.Quantity;
                        result = productVariationRepository.Update(prodvariation);
                    }
                    var order = orderRepository.GetAll().FirstOrDefault(c => c.Id == new Guid(orderid));
                    decimal totalPaymentChange = (order.TotalMoney - orDetail.TotalMoney) + (int.Parse(quantity) * orDetail.Price);
                    strChange += $"\n Số lượng của sản phẩm {GetProductVariationById(new Guid(prodId))} từ {orDetail.Quantity} -> {quantity}";
                    strChange += $"\n Tổng tiền của sản phẩm {GetProductVariationById(new Guid(prodId))} từ {orDetail.TotalMoney.ToString("#,##0.##")} -> {(int.Parse(quantity) * orDetail.Price).ToString("#,##0.##")}";

                    orDetail.Quantity = int.Parse(quantity);
                    orDetail.TotalMoney = int.Parse(quantity) * orDetail.Price;
                    result = orderDetailRepository.Update(orDetail);
                    if (result)
                    {

                        strChange += $"\n Tổng tiền thanh toán từ {order.TotalMoney.ToString("#,##0.##")} -> {totalPaymentChange.ToString("#,##0.##")}";
                        strChange += $"<------------------------------------------>";
                        order.TotalMoney = totalPaymentChange;
                        order.HistoryLogChange = strChange + order.HistoryLogChange.NullToString();
                        result = orderRepository.Update(order);
                    }
                }
                return Json(result);
            }
            catch (Exception ex)
            {
                return RedirectToAction("AccessDenied", "Account");
            }
        }

        public async Task<JsonResult> GetLog(Guid id)
        {
            var jsonString = orderRepository.GetAll().FirstOrDefault(c => c.Id == id).HistoryLogChange.NullToString();
            return Json(jsonString);
        }

        public async Task<ActionResult> ShowProductForAdmin(Guid orderid, string prodName, Guid? category, int? brand, decimal? fPrice, decimal? tPrice, int? type, int pg = 1, int pageSize = 10, string Keyword = null)
        {

            try
            {
                TempData["orderId"] = orderid;
                GProject.WebApplication.Services.OrderService pService = new GProject.WebApplication.Services.OrderService();
                decimal valFromPrice = fPrice.HasValue ? fPrice.Value : -1;
                decimal valToPrice = tPrice.HasValue ? tPrice.Value : -1;
                Guid valCategory = category.HasValue ? category.Value : Guid.Empty;
                int valBrand = brand.HasValue ? brand.Value : -1;

                var customer = HttpContext.Session.GetObjectFromJson<Customer>("userLogin");
                var data = await pService.GetDataForIndex(prodName, valCategory, valBrand, valFromPrice, valToPrice, type.NullToString(), pg, pageSize, Keyword);
                this.ViewBag.Pager = data.pager;
                this.ViewData[nameof(prodName)] = (object)prodName;
                this.ViewData[nameof(fPrice)] = (object)valFromPrice;
                this.ViewData[nameof(tPrice)] = (object)valFromPrice;
                this.ViewData[nameof(brand)] = (object)valBrand;
                this.ViewData[nameof(category)] = (object)valCategory;
                this.ViewData[nameof(Keyword)] = (object)Keyword;
                return View(data.tuple);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return RedirectToAction("AccessDenied", "Account");
            }
        }



        [Route("/productdetail-for-admin/{product_id}")]
        public async Task<ActionResult> ProductDetailForAdmin(Guid product_id)
        {
            try
            {
                Guid orderid = (Guid)TempData["orderId"];
                TempData.Keep("orderId");

                Order order = orderRepository.GetAll().FirstOrDefault(c => c.Id == orderid);
                Customer customer = customerRepository.GetAll().FirstOrDefault(c => c.Id == order.CustomerId);
                GProject.WebApplication.Services.ProductService pService = new GProject.WebApplication.Services.ProductService();
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("mess")))
                    ViewData["Mess"] = HttpContext.Session.GetString("mess");
                return View(await pService.GetProductDetail(product_id, customer));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return RedirectToAction("AccessDenied", "Account");
            }
        }

        public async Task<ActionResult> AddToOrderForAdmin(string products)
        {
            try
            {
                var employee = HttpContext.Session.GetObjectFromJson<Employee>("userLogin");
                string strChange = $"Thời gian: {DateTime.Now} \nNgười thay đổi: {employee.Email}";
                Guid orderid = (Guid)TempData["orderId"];
                TempData.Keep("orderId");
                Order order = orderRepository.GetAll().FirstOrDefault(c => c.Id == orderid);
                ProdOrder prodOrder = JsonConvert.DeserializeObject<ProdOrder>(products);
                ProductVariation productVariation = productVariationRepository.GetAll().FirstOrDefault(c => c.ProductId == new Guid(prodOrder.productId) && c.ColorId == prodOrder.color && c.SizeId == prodOrder.size);
                OrderDetail orderDetail = orderDetailRepository.GetAll().FirstOrDefault(c => c.OrderId == orderid && c.ProductVariationId == productVariation.Id);


                if (orderDetail != null)
                {
                    strChange += $"\n Số lượng của sản phẩm {GetProductVariationById(productVariation.Id)} từ {orderDetail.Quantity} -> {orderDetail.Quantity + prodOrder.quantity}";
                    strChange += $"\n Tổng tiền của sản phẩm {GetProductVariationById(productVariation.Id)} từ {orderDetail.TotalMoney.ToString("#,##0.##")} -> {orderDetail.TotalMoney + prodOrder.totalMoneyItem.ToString("#,##0.##")}";
                    orderDetail.Quantity = orderDetail.Quantity + prodOrder.quantity;
                    orderDetail.TotalMoney = orderDetail.TotalMoney + prodOrder.totalMoneyItem.NullToDecimal();
                    if (orderDetailRepository.Update(orderDetail))
                    {
                        decimal totalPaymentChange = order.TotalMoney + prodOrder.totalMoneyItem.NullToDecimal();
                        strChange += $"\n Tổng tiền thanh toán từ {order.TotalMoney.ToString("#,##0.##")} -> {totalPaymentChange.ToString("#,##0.##")}";
                        strChange += $"<------------------------------------------>";
                        order.TotalMoney = totalPaymentChange;
                        order.HistoryLogChange = strChange + order.HistoryLogChange.NullToString();
                        if (!orderRepository.Update(order))
                            HttpContext.Session.SetString("mess", "Thêm sản phẩm vào đơn hàng thất bại");
                        else
                            HttpContext.Session.SetString("mess", "Thêm sản phẩm vào đơn hàng thành công");
                    }
                }
                else
                {
                    orderDetail = new OrderDetail();
                    orderDetail.OrderId = orderid;
                    orderDetail.ProductVariationId = productVariation.Id;
                    orderDetail.Price = prodOrder.price.NullToDecimal();
                    orderDetail.Quantity = prodOrder.quantity.NullToInt();
                    orderDetail.TotalMoney = prodOrder.totalMoneyItem.NullToDecimal();
                    orderDetail.Status = OrderDetailStatus.Watting;
                    if (orderDetailRepository.Add(orderDetail))
                    {
                        strChange += $"\n Thêm sản phẩm {GetProductVariationById(productVariation.Id)} vào đơn hàng với thông tin như sau:";
                        strChange += $"\n Đơn giá: {prodOrder.price.NullToDecimal().ToString("#,##0.##")}";
                        strChange += $"\n Số lượng: {prodOrder.quantity}";
                        strChange += $"\n Tổng tiền sản phẩm: {prodOrder.totalMoneyItem.NullToDecimal().ToString("#,##0.##")}";

                        decimal totalPaymentChange = order.TotalMoney + orderDetail.TotalMoney;
                        strChange += $"\n Tổng tiền thanh toán từ {order.TotalMoney.ToString("#,##0.##")} -> {totalPaymentChange.ToString("#,##0.##")}";
                        strChange += $"<------------------------------------------>";
                        order.TotalMoney = totalPaymentChange;
                        order.HistoryLogChange = strChange + order.HistoryLogChange.NullToString();
                        if (!orderRepository.Update(order))
                            HttpContext.Session.SetString("mess", "Thêm sản phẩm vào đơn hàng thất bại");
                        else
                            HttpContext.Session.SetString("mess", "Thêm sản phẩm vào đơn hàng thành công");
                    }
                }

                var prodvariation = productVariationRepository.GetAll().FirstOrDefault(c => c.Id == orderDetail.ProductVariationId);
                if (prodvariation != null)
                {
                    prodvariation.QuantityInStock = prodvariation.QuantityInStock - orderDetail.Quantity;
                    productVariationRepository.Update(prodvariation);
                }
                return Json(true);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private string GetProductVariationById(Guid? id)
        {
            var result = productRepository.GetAll()
                        .Join(productVariationRepository.GetAll(), prd => prd.Id, prv => prv.ProductId, (prd, prv) => new { prd, prv })
                        .Join(colorRepository.GetAll(), pv => pv.prv.ColorId, cl => cl.Id, (pv, cl) => new { pv.prd, pv.prv, cl })
                        .Join(sizeRepository.GetAll(), pvc => pvc.prv.SizeId, sz => sz.Id, (pvc, sz) => new { pvc.prd, pvc.prv, pvc.cl, sz })
                        .Where(pvcs => pvcs.prv.Id == id)
                        .Select(pvcs => new
                        {
                            product_name = pvcs.prd.Name,
                            color_name = pvcs.cl.Name,
                            size_code = pvcs.sz.Code,
                            product_code = pvcs.prd.ProductCode
                        })
                        .FirstOrDefault();

            return result != null ? $" (Mã sản phẩm: {result.product_code.NullToString()}, Tên sản phẩm: {result.product_name.NullToString()}, Màu sắc: {result.color_name.NullToString()}, Size: {result.size_code.NullToString()})" : "";
        }
    }
}