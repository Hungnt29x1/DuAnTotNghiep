using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GProject.WebApplication.Helpers
{
    
    public static class Commons
    {
        /// <summary>
        /// Tghoong tin người dùng khi đăng nhập vào,nếu chưa đăng nhập thì mặc định là ""
        /// </summary>
        public static string mylocalhost = "https://localhost:7009/api/";
        public static string email = "nhamvdph18699@fpt.edu.vn";
        public static string passemail = "14082002";
        public static Guid id = Guid.Empty;

        public static T? ConverObject<T>(object obj)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(obj));
        }

        /// <summary>
        /// Tạo mã ngẫu nhiên
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string RandomString(int length)
        {
            const string valid = "QWERTYUIOPASDFGHJKLZXCVBNM0123456789";
            StringBuilder res = new StringBuilder();
            Random rd = new Random();
            while (0 < length--)  //Chạy đến khi đủ độ dài mật khẩu mong muốn
            {
                res.Append(valid[rd.Next(valid.Length)]); //Add thêm kí từ random trong valid
            }
            return res.ToString();
        }

        /// <summary>
        /// Convert sang string
        /// </summary>
        /// <param name="vobjValue"></param>
        /// <returns></returns>
        public static string? NullToString(this object? @objValue)
        {
            return @objValue == null ? String.Empty : @objValue.ToString();
        }

        /// <summary>
        /// Convert to double
        /// </summary>
        /// <param name="vobjValue"></param>
        /// <returns></returns>
        public static Double NullToDouble(this object? @objValue)
        {
            if (@objValue == null) return 0;
            //--
            _ = double.TryParse(objValue.NullToString(), out double _rs);
            return _rs;
        }

        /// <summary>
        /// Convert to double
        /// </summary>
        /// <param name="vobjValue"></param>
        /// <returns></returns>
        public static Decimal NullToDecimal(this object @objValue)
        {
            _ = decimal.TryParse(objValue.NullToString(), out decimal _rs);
            return _rs;
        }

        /// <summary>
        /// Convert to int32
        /// </summary>
        /// <param name="vobjValue"></param>
        /// <returns></returns>
        public static int NullToInt(this object @objValue)
        {
            Int32.TryParse(objValue.NullToString(), out int _rs);
            return _rs;
        }

        /// <summary>
        /// Convert to long
        /// </summary>
        /// <param name="vobjValue"></param>
        /// <returns></returns>
        public static long NullToLong(this object @objValue)
        {
            Int64.TryParse(objValue.NullToString(), out Int64 _rs);
            return _rs;
        }

        /// <summary>
        /// Kiểm tra định dạng Json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryParseJson<T>(this string @this, out T result)
        {
            //--
            try
            {
                bool success = true;
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore, //Bỏ qua những propertie NULL
                    Error = (sender, args) => { success = false; args.ErrorContext.Handled = true; },
                    MissingMemberHandling = MissingMemberHandling.Error
                };
                var _rs = JsonConvert.DeserializeObject<T>(@this, settings);
                if (_rs != null) result = _rs; else result = default;
                return success;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static async Task<List<T>> GetAll<T>(string url)
        {
            try
            {
                List<T> objs = new List<T>();
                var Rest = new RestSharpHelper(url);
                var Response = await Rest.RequestBaseAsync(url, RestSharp.Method.Get);
                if (Response.StatusCode == HttpStatusCode.OK && !string.IsNullOrWhiteSpace(Response.Content))
                {
                    Response.Content.TryParseJson(out List<T> result);
                    objs = result;
                }
                Console.WriteLine(objs);
                return objs;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static async Task<bool> Add_or_UpdateAsync<T>(this T obj, string url)
        {
            try
            {
                var Rest = new RestSharpHelper(url);
                var Response = await Rest.RequestBaseAsync(url, RestSharp.Method.Post, null, null, obj);
                return Response.IsSuccessful;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void setObjectAsString(this ISession session, string key, string value)
        {
            session.SetString(key, value);
        }

        public static string GetObjectFromString(ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : value;
        }
        public static void setObjectAsJson(ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }

        public static List<T> GetListFromJsonData<T>(string path)
        {
            try
            {
                List<T> objects = JsonConvert.DeserializeObject<List<T>>(File.ReadAllText(path));
                return objects;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static List<T> GetListFromJsonData3<T>(string path)
        {
            try
            {
                List<T> objects = JsonConvert.DeserializeObject<List<T>>(path);
                return objects;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static List<T> GetListFromJsonData2<T>(string path)
        {
            try
            {
                using (StreamReader getfile = File.OpenText(path))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    List<T> objects = (List<T>)serializer.Deserialize(getfile, typeof(List<T>));
                    return objects;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static string GetEnumDescription<T>(T value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (attributes != null && attributes.Any())
            {
                return attributes.First().Description;
            }
            return value.ToString();
        }
    }
}
