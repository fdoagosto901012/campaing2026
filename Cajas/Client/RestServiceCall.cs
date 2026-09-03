using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cajas.Client
{
    public static class RestServiceCall<T>
    {
        //private static readonly string BASE_URL = "http://api.taxistascancunoficial.com/";
        private static readonly string BASE_URL = "http://localhost:4444/";
        static HttpClient Client = new HttpClient() { Timeout = TimeSpan.FromSeconds(60) };

        public static async Task<object> Get(string endPoint, Action<T> onSuccess, Action<Exception> onError)
        {
            try
            {
                string url = BASE_URL + endPoint;
                var response = await Client.GetAsync(url);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrWhiteSpace(jsonData))
                    {
                        T responseObject = JsonConvert.DeserializeObject<T>(jsonData);
                        onSuccess?.Invoke(responseObject);
                        return responseObject;
                    }
                    else
                    {
                        Exception exception = new Exception("Resource Not Found");
                        onError?.Invoke(exception);
                    }
                }
                else
                {
                    Exception exception = new Exception("Request failed with status code " + response.StatusCode);
                    onError?.Invoke(exception);
                }
                return null;
            }
            catch (Exception exception)
            {
                onError?.Invoke(exception);
                return null;
            }
        }


        public static async Task<object> Post(string endPoint, StringContent content, Action<T> onSuccess, Action<Exception> onError)
        {
            try
            {
                string url = BASE_URL + endPoint;
                var response = await Client.PostAsync(url, content);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrWhiteSpace(jsonData))
                    {
                        T responseObject = JsonConvert.DeserializeObject<T>(jsonData);
                        onSuccess?.Invoke(responseObject);
                        return responseObject;
                    }
                    else
                    {
                        Exception exception = new Exception("Resource Not Found");
                        onError?.Invoke(exception);
                    }
                }
                else
                {
                    Exception exception = new Exception("Request failed with status code " + response.StatusCode);
                    onError?.Invoke(exception);
                }
                return null;
            }
            catch (Exception exception)
            {
                onError?.Invoke(exception);
                return null;
            }
        }
    }
}
