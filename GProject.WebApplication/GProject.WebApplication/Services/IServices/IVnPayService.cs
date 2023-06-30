using GProject.WebApplication.Models.Payments;

namespace GProject.WebApplication.Services.IServices
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(PaymentInformationModel model, HttpContext context);
        bool PaymentExecute(IQueryCollection collections);
    }
}
