using MyFirstWeb.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Web;

namespace MyFirstWeb.Controllers.UtilityFolder
{
    public class CallAPI
    {
        public List<Product> APIGetProduct()
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage httpResponseMessage = httpClient.GetAsync("http://localhost:44354/api/product/getProduct").Result;

            int statusCode = (int)httpResponseMessage.StatusCode;
            Console.WriteLine($"Http 狀態碼: {statusCode}");

            string content = httpResponseMessage.Content.ReadAsStringAsync().Result;
            Console.WriteLine($"Http 回應內容: {content}");

            return statusCode ==200? JsonConvert.DeserializeObject<List<Product>>(content):new List<Product>();
             //JsonConvert.SerializeObject(object)
        }      

     
    } 
}