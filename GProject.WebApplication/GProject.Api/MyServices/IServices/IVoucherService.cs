using GProject.Data.DomainClass;

namespace GProject.Api.MyServices.IServices
{
    public interface IVoucherService
    {
        public bool Create(Voucher obj);

        public bool Update(Voucher obj);

        public bool Delete(int id);

        public bool UpdateNumber(int id);

        public bool UpdateStatus(int id);

        public List<Voucher> GetAll();
    }
}
