using GProject.WebApplication.Models.DeliveryAddressAndShippingFee;
using Newtonsoft.Json;
using System.Text;

namespace GProject.WebApplication.Services
{
    public class DeliveryAddressAndShippingFeeService
    {
        private const string apiKey = "a5cac09d-cea5-11ed-a3ed-eac62dba9bd9";
        public DeliveryAddressAndShippingFeeService()
        {

        }

        public async Task<List<DeliveryAddressDto>> GetDataProvincesAsync()
        {
            List<DeliveryAddressDto> deliveryAddressList = new List<DeliveryAddressDto>();

            var client = new HttpClient();

            string url = "https://online-gateway.ghn.vn/shiip/public-api/master-data/province";


            client.DefaultRequestHeaders.Add("Token", apiKey);

            HttpResponseMessage response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"API returned {response.StatusCode}");
            }

            string result = await response.Content.ReadAsStringAsync();

            dynamic province = JsonConvert.DeserializeObject(result);

            foreach (var item in province.data)
            {
                DeliveryAddressDto deliveryAddress = new DeliveryAddressDto();
                deliveryAddress.ProvinceID = item.ProvinceID;
                deliveryAddress.ProvinceName = item.ProvinceName;

                deliveryAddressList.Add(deliveryAddress);
            }

            return deliveryAddressList;
        }

        public async Task<List<DeliveryAddressDto>> GetDataDistrictsAsync(int id)
        {
            if(id == 0) return new List<DeliveryAddressDto>();
            List<DeliveryAddressDto> deliveryAddressList = new List<DeliveryAddressDto>();

            var client = new HttpClient();

            string url = "https://online-gateway.ghn.vn/shiip/public-api/master-data/district";

            var dataSearchByProvinceId = new
            {
                province_id = id,
            };

            var json = JsonConvert.SerializeObject(dataSearchByProvinceId);
            var dataJson = new StringContent(json, Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Add("Token", apiKey);

            HttpResponseMessage response = await client.PostAsync(url, dataJson);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"API returned {response.StatusCode}");
            }

            string result = await response.Content.ReadAsStringAsync();

            dynamic districts = JsonConvert.DeserializeObject(result);

            foreach (var item in districts.data)
            {
                DeliveryAddressDto deliveryAddress = new DeliveryAddressDto();
                deliveryAddress.DistrictID = item.DistrictID;
                deliveryAddress.DistrictName = item.DistrictName;

                deliveryAddressList.Add(deliveryAddress);
            }

            return deliveryAddressList;
        }


        public async Task<List<DeliveryAddressDto>> GetDataWardAsync(int id)
        {
            if(id == 0) return new List<DeliveryAddressDto> { };
            List<DeliveryAddressDto> deliveryAddressList = new List<DeliveryAddressDto>();

            var client = new HttpClient();

            string url = "https://online-gateway.ghn.vn/shiip/public-api/master-data/ward?district_id=";


            client.DefaultRequestHeaders.Add("Token", apiKey);

            HttpResponseMessage response = await client.GetAsync(url + id);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"API returned {response.StatusCode}");
            }

            string result = await response.Content.ReadAsStringAsync();

            dynamic ward = JsonConvert.DeserializeObject(result);

            foreach (var item in ward.data)
            {
                DeliveryAddressDto deliveryAddress = new DeliveryAddressDto();
                deliveryAddress.WardCode = item.WardCode;
                deliveryAddress.WardName = item.WardName;

                deliveryAddressList.Add(deliveryAddress);
            }

            return deliveryAddressList;
        }


        public async Task<float> ShippingFee(int to_district_id, string to_ward_code)
        {
            List<ShippingPriceDto> ShippingPriceList = new List<ShippingPriceDto>();

            var client = new HttpClient();

            string url = "https://online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/fee";

            var request = new HttpRequestMessage(HttpMethod.Post, url);

            var dataAddress = new
            {
                from_district_id = 3440,
                service_id = 100039,
                to_district_id = to_district_id,
                to_ward_code = to_ward_code,
                height = 80,
                length = 20,
                weight = 200,
                width = 20
            };

            var json = JsonConvert.SerializeObject(dataAddress);

            request.Headers.Add("Token", apiKey);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"API returned {response.StatusCode}");
            }

            string result = await response.Content.ReadAsStringAsync();

            dynamic shippingOrder = JsonConvert.DeserializeObject(result);

            return Convert.ToSingle(shippingOrder.data.total);
        }
    }
}
