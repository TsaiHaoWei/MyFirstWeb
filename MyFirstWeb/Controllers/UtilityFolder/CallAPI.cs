using DataBaseController;
using MyFirstWeb.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using System.Web;

namespace MyFirstWeb.Controllers.UtilityFolder
{
    public class CallAPI
    {
        private string APIpath = "http://localhost:44354/api/product/";
        public List<Product> APIGetProduct()
        {
            HttpClient httpClient = new HttpClient();
           // HttpResponseMessage httpResponseMessage = httpClient.GetAsync("http://localhost:44354/api/product/getProduct").Result;
            HttpResponseMessage httpResponseMessage = httpClient.GetAsync(Path.Combine(APIpath, "getProduct")).Result;

            int statusCode = (int)httpResponseMessage.StatusCode;
            Console.WriteLine($"Http 狀態碼: {statusCode}");

            string content = httpResponseMessage.Content.ReadAsStringAsync().Result;
            Console.WriteLine($"Http 回應內容: {content}");

            return statusCode ==200? JsonConvert.DeserializeObject<List<Product>>(content):new List<Product>();
             //JsonConvert.SerializeObject(object)
        }
        public List<ProductMarketprice> APIGetProducPrice()
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage httpResponseMessage = httpClient.GetAsync(Path.Combine(APIpath, "getproductprice")).Result;

            int statusCode = (int)httpResponseMessage.StatusCode;
            Console.WriteLine($"Http 狀態碼: {statusCode}");

            string content = httpResponseMessage.Content.ReadAsStringAsync().Result;
            Console.WriteLine($"Http 回應內容: {content}");

            return statusCode == 200 ? JsonConvert.DeserializeObject<List<ProductMarketprice>>(content) : new List<ProductMarketprice>();
            //JsonConvert.SerializeObject(object)
        }
        public bool APIInsertProduct(Product product) {

            try
            {
                //方法一，传json参数
                var d = new
                {
                    ProductName = product.ProductName
                };
                var data = JsonConvert.SerializeObject(d);
                HttpContent httpContent = new StringContent(data);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                using (HttpClient httpClient = new HttpClient())
                {
                    string responseJson = httpClient.PostAsync(Path.Combine(APIpath, "createproduct"), httpContent)
                       .Result.Content.ReadAsStringAsync().Result;
                    Debug.WriteLine(responseJson);
                    return true;

                }
            }
            catch {
                return false;
            }
       
        }
        public bool APIInsertProductPrice(ProductMarketprice productMarketPrice)
        {
            try
            {
                var d = new
                {
                    ProductName = productMarketPrice.ProductName,
                    Price = productMarketPrice.Price,
                    Location = productMarketPrice.Location,
                    Capacity = productMarketPrice.Capacity
                };
                var data = JsonConvert.SerializeObject(d);
                HttpContent httpContent = new StringContent(data);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                using (HttpClient httpClient = new HttpClient())
                {
                    string responseJson = httpClient.PostAsync(Path.Combine(APIpath, "CreateProductPrice"), httpContent)
                       .Result.Content.ReadAsStringAsync().Result;
                    Debug.WriteLine(responseJson);
                }
                return true;
            }
            catch
            {
                return false;

            }
           
        }

    } 
}