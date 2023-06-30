using GProject.Data.DomainClass;
using GProject.WebApplication.Helpers;
using Quartz;

namespace GProject.WebApplication.Services
{
    public class ReturnProductJob : IJob
    {

        public async Task Execute(IJobExecutionContext context)
        {
            var lstCartDetail = await Commons.GetAll<CartDetail>(String.Concat(Commons.mylocalhost, "Cart/get-all-cart-detail"));
            var lstProductvariation = await Commons.GetAll<ProductVariation>(String.Concat(Commons.mylocalhost, "ProductVariation/get-all-ProductVariation"));

            lstCartDetail = lstCartDetail.Where(x => x.TimeOut < DateTime.Now).ToList();

            if(lstCartDetail.Count > 0)
            {
                foreach (var item in lstCartDetail)
                {
                    ProductVariation productVariation = lstProductvariation.FirstOrDefault(c => c.Id == item.ProductVariationId);
                    if (productVariation != null)
                    {
                        productVariation.QuantityInStock = productVariation.QuantityInStock + item.Quantity;
                        await Commons.Add_or_UpdateAsync(productVariation, String.Concat(Commons.mylocalhost, "ProductVariation/update-ProductVariation"));
                    }          

                    //-- Remove khỏi CartDetail
                    string urlRemoveCartDetail = string.Concat(Commons.mylocalhost, "Cart/delete-cart-detail?id=", item.CartId, "&productVariation_id=", item.ProductVariationId);
                    var RestRemoveCartDetail = new RestSharpHelper(urlRemoveCartDetail);
                    var resRemoveCartDetail = await RestRemoveCartDetail.RequestBaseAsync(urlRemoveCartDetail, RestSharp.Method.Delete);
                }
            }
            
        }
    }
}
