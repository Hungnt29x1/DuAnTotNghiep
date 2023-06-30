using GProject.Api.MyServices.IServices;
using GProject.Data.DomainClass;
using GProject.Data.MyRepositories.IRepositories;
using GProject.Data.MyRepositories.Repositories;

namespace GProject.Api.MyServices.Services
{
    public class VoucherService : IVoucherService
    {
        private readonly IVoucherRepository _voucherRepository;
        public VoucherService()
        {
            _voucherRepository = new VoucherRepository();
        }
        public bool Create(Voucher obj)
        {
            if (obj == null) return false;
            obj = new Voucher()
            {
                VoucherId = obj.VoucherId,
                Name = obj.Name,
                DiscountRate = obj.DiscountRate,
                DiscountForm = obj.DiscountForm,
                MaximumDiscount = obj.MaximumDiscount,
                NumberOfVouchers = obj.NumberOfVouchers,
                MinimumOrder = obj.MinimumOrder,
                ExpirationDate = obj.ExpirationDate,
                CreateDate = obj.CreateDate,
                EmployeeId = obj.EmployeeId,
                Status = obj.Status
            };
            _voucherRepository.Add(obj);
            return true;
        }

        public bool Delete(int id)
        {            
            var result = _voucherRepository.GetAll().FirstOrDefault(x => x.Id == id);
            if (result == null) return false;
            _voucherRepository.Delete(result);
            return true;
        }

        public List<Voucher> GetAll()
        {
            return _voucherRepository.GetAll();
        }

        public bool Update(Voucher obj)
        {
            var result = _voucherRepository.GetAll().FirstOrDefault(x => x.Id == obj.Id);
            if (result == null) return false;
            result.VoucherId = obj.VoucherId;
            result.Name = obj.Name;
            result.DiscountRate = obj.DiscountRate;
            result.DiscountForm = obj.DiscountForm;
            result.NumberOfVouchers = obj.NumberOfVouchers;
            result.MaximumDiscount = obj.MaximumDiscount;
            result.MinimumOrder = obj.MinimumOrder;
            result.ExpirationDate = obj.ExpirationDate;
            result.CreateDate = obj.CreateDate;
            result.UpdateDate = DateTime.Now;
            result.Status= obj.Status;
            _voucherRepository.Update(result);
            return true;

        }

        public bool UpdateNumber(int id)
        {
            var result = _voucherRepository.GetAll().FirstOrDefault(x => x.Id == id);
            if (result == null) return false;
            result.NumberOfVouchers -= 1;
            result.UpdateDate = DateTime.Now;
            _voucherRepository.Update(result);
            return true;
        }

        public bool UpdateStatus(int id)
        {
            var result = _voucherRepository.GetAll().FirstOrDefault(x => x.Id == id);
            if (result == null) return false;
            if(result.Status == 0)
            {
                result.Status = 1;
            }
            else if(result.Status == 1)
            {
                result.Status = 0;
            }
            result.UpdateDate = DateTime.Now;
            _voucherRepository.Update(result);
            return true;
        }
    }
}
