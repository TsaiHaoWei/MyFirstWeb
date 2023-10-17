using Dapper;
using DataBaseController;
using Microsoft.Ajax.Utilities;
using MyFirstWeb.Controllers.UtilityFolder;
using MyFirstWeb.Models;
using MyFirstWeb.Models.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;

namespace MyFirstWeb.Controllers
{
    public class HomeController : Controller
    {
        //First Page絕對不會是Post
        public string SysConnectString;

        public ActionResult HomeOverview()
        {
            return View();
        }
        private LoginUserModel CookieCheck()
        {
            //獲取cookie中的數據
            HttpCookie cookie = Request.Cookies.Get("EmailCookie");

            //判斷cookie是否空值
            if (cookie != null)
            {
                //把保存的用戶名和密碼賦值給對應的文本框
                //用戶名
                LoginUserModel logcookie = new LoginUserModel();
                logcookie.LoginEmail = cookie.Values["LoginEmail"].ToString();
                return logcookie;
            }
            else
                return null;

        }
        public ActionResult HomePage()
        {
            LoginUserModel logcookie = CookieCheck();
            if (logcookie == null)
                return View(new LoginUserModel());
            else
                return View(logcookie);
        }

        [HttpPost]
        public ActionResult HomePage(FormCollection post)
        {
            DBAccess DB = new DBAccess();

            string LEmail = post["LoginEmail"];//name:
            string LPassword = post["LoginPassword"];
            string REmail = post["RegisterEmail"];//name:
            string RPassword = post["RegisterPassword"];
            string RUserName = post["RegisterUserName"];
            string FUserName = post["ForgotUserName"];
            string FEmail = post["ForgotEmail"];
            string NewPassword = post["NewPassword"];
            string LoginCheck = post["Check"];

            Debug.WriteLine($"Res Email:{LEmail},ResPassword:{LPassword} LoginChec:{LoginCheck}");
            //Register
            if (!string.IsNullOrEmpty(REmail) && !string.IsNullOrEmpty(RPassword) && !string.IsNullOrEmpty(RUserName))
            {
                if (DB.GetUser().FindAll(var => var.Email.Equals(REmail)).Count == 0)
                {
                    User Account = new User();
                    Account.UserId = DB.StroreProcedure("UserID").ToString();
                    Account.UserName = RUserName;
                    Account.Password = RPassword;
                    Account.Email = REmail;
                    if (DB.SetUser(Account))
                    {
                        TempData["message"] = "註冊成功";

                    }
                }
                else
                {
                    TempData["message"] = "已有相同的註冊郵件";
                }

            }
            else if (!string.IsNullOrEmpty(LEmail) && !string.IsNullOrEmpty(LPassword))
            {
                List<User> UserList = DB.GetUser();
                if (UserList.Find(var => var.Email.Equals(LEmail) && var.Password.Equals(LPassword)) != null)
                {

                    if (!string.IsNullOrEmpty(LoginCheck))
                    {
                        HttpCookie hc = new HttpCookie("EmailCookie");

                        //在cookie對像中保存用戶名和密碼
                        hc["LoginEmail"] = LEmail;
                        //設置過期時間
                        hc.Expires = DateTime.Now.AddDays(2);
                        //保存到客戶端
                        Response.Cookies.Add(hc);
                    }
                    else
                    {
                        HttpCookie hc = new HttpCookie("EmailCookie");
                        //判斷hc是否空值
                        //if (hc != null)
                        //{
                        //    //設置過期時間
                        //    hc.Expires = DateTime.Now.AddDays(-1);
                        //    //保存到客戶端
                        //    Response.Cookies.Add(hc);
                        //}

                    }
                    Session["LoginEmail"] = LEmail;

                    return RedirectToAction("NuNugame2", "Home");
                    //return RedirectToAction("NuNuGaming", "Home");
                }
                else
                {
                    TempData["message"] = "登入資料錯誤";

                }


            }
            else
            {
                if (!string.IsNullOrEmpty(FUserName) && !string.IsNullOrEmpty(FEmail) && !string.IsNullOrEmpty(NewPassword))
                {
                    User Account = new User();


                    Account.UserName = FUserName;
                    Account.Password = NewPassword;
                    Account.Email = FEmail;
                    if (DB.UpdateUser(Account))
                    {
                        TempData["message"] = "更改成功";
                    }
                }

            }
            LoginUserModel logcookie = CookieCheck();
            if (logcookie == null)
                return View(new LoginUserModel());
            else
                return View(logcookie);

        }


        public ActionResult NuNuGaming()
        {
            return View();
        }
        [HttpPost]
        public ActionResult NuNuGaming(FormCollection post)
        {

            string b1 = post["name"];

            List<string> B1List = Regex.Split(b1, Environment.NewLine).ToList();
            string c1 = post["colorlist1"];
            List<string> C1List = Regex.Split(c1, Environment.NewLine).ToList();
            string b2 = post["bodylist2"];
            List<string> B2List = Regex.Split(b2, Environment.NewLine).ToList();
            string c2 = post["colorlist2"];
            List<string> C2List = Regex.Split(c2, Environment.NewLine).ToList();
            List<Player> PList = new List<Player>();
            for (int i = 0; i < 2; i++)
            {
                Player p = new Player();
                p.BodyList = new List<string>();
                p.ColorList = new List<string>();
                p.PlayerID = i == 0 ? "A" : "B";
                foreach (var v in i == 0 ? B1List : B2List)
                {
                    if (v != null)
                        p.BodyList.Add(v);
                }
                foreach (var v in i == 0 ? C1List : C2List)
                {
                    if (v != null)
                        p.ColorList.Add(v);
                }
                PList.Add(p);
            }
            // ViewBag.list = PList;
            Debug.WriteLine(PList.Count);
            return View(PList);
        }


        public ActionResult Elements()
        {
            return View();
        }
        public ActionResult NuNugame2()
        {
            return View();
        }
        [HttpPost]
        public ActionResult NuNugame2(FormCollection post)
        {
            DBAccess DB = new DBAccess();

            string name = post["name"];
            string email = post["email"];
            string playitem = post["category"];
            string radio = post["priority"]; //post[name]

            Debug.WriteLine($"{name}{email} radio:{radio} platitem:{playitem} Platynumber:{radio}");
            List<PlayerItem> list = DB.GetPlayerItem(playitem);
            Debug.WriteLine($"playitemCount:{list.Count}");
            //List<PlayerRecord> listPlayerReocrd= DB.GetPlayerRecord().FindAll(var=>var.name.Equals(name) & var.Email.Equals(email));
            List<Player> PList = new List<Player>();

            for (int i = 1; i <= int.Parse(radio); i++)
            {
                Player p = new Player();
                p.BodyList = new List<string>();
                p.ColorList = new List<string>();
                foreach (var item in list)
                {
                    p.BodyList.Add(item.Bodylist);
                    p.ColorList.Add(item.Colorlist);
                }
                p.PlayerID = $"第{i}位玩家";
                PList.Add(p);
            }

            foreach (var item in PList) {
                Debug.WriteLine($"player:{item.PlayerID}");
                Debug.WriteLine($"playerLSIT:{item.ColorList.Count}{item.BodyList.Count}");
            }
            return View(PList);
        }


        public ActionResult NuNugameAPI()
        {
            CallAPI APIC = new CallAPI();

            List<Product> Productlist = APIC.APIGetProduct();

            ViewBag.Productlist = Productlist;

            //Product p = new Product();
            //p.ProductName = "測試";
            //    APIC.APIInsertProductPrice(, "");
            //  APIC.APIInsertProduct(p);
            //Debug.WriteLine($"Product數量:{Productlist.Count}   ProductPrice數量:{ProductPricelist.Count}  ");
            return View();
        }
        [HttpPost]
        public ActionResult NuNugameAPI(FormCollection post)
        {
            Debug.WriteLine("剷平名:" + post["productnamesearch"]);
            Debug.WriteLine(post["pName"].Trim());

            CallAPI APIC = new CallAPI();
            string Prodcutname = string.Empty;
            if (string.IsNullOrEmpty(post["productnamesearch"]))
            {
                Prodcutname = post["search_name"];
                if (!string.IsNullOrEmpty(Prodcutname))
                {
                    Product p = new Product();
                    p.ProductName = Prodcutname;
                    APIC.APIInsertProduct(p);
                    Debug.WriteLine("產品代號新增完成");
                }
            }
            else
            {
                Prodcutname = post["pName"].Trim();
            }
            Debug.WriteLine("productname finally:" + Prodcutname);
            string location = post["Location"];
            string capacity = post["Capacity"];
            string price = post["price"];
            if (!string.IsNullOrEmpty(Prodcutname) &&
                !string.IsNullOrEmpty(location) &&
                !string.IsNullOrEmpty(capacity) &&
                !string.IsNullOrEmpty(price))
            {
                ProductMarketprice P_Price = new ProductMarketprice();
                P_Price.ProductName = Prodcutname;
                P_Price.Location = location;
                P_Price.Capacity = capacity;
                P_Price.Price = price;

                APIC.APIInsertProductPrice(P_Price);
                Debug.WriteLine("產品市價新增完成");
                TempData["message"] = "產品市價新增完成";
            }
            else
                TempData["message"] = "資料填寫不完整";

            //re setting
            List<Product> Productlist = APIC.APIGetProduct();

            ViewBag.Productlist = Productlist;
            return View();
        }
        [HttpPost]
        public ActionResult APIAjaxSearch(string id = "")
        {
            CallAPI APIC = new CallAPI();
            List<ProductMarketprice> ProductPricelist = APIC.APIGetProducPrice().FindAll(var => var.ProductID.Equals(id));
            foreach (var item in ProductPricelist)
            {
                item.ProductName = APIC.APIGetProduct().Find(var => var.ProductID.Equals(item.ProductID)).ProductName;
            }
            string result = "";

            if (ProductPricelist.Count > 3)
            {
                result = JsonConvert.SerializeObject(ProductPricelist.OrderBy(var => var.Price).ToList().GetRange(0, 3));
                return Json(result);

            }
            else
            {
                if (ProductPricelist.Count == 0)
                    result = "";
                else
                    result = JsonConvert.SerializeObject(ProductPricelist);
                return Json(result);

            }
            //ViewBag.ListMarketPrice = ProductPricelist.OrderByDescending(var => var.Price).ToList();


        }

        public ActionResult Advice()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Advice(FormCollection post)
        {
            string name = post["name"];
            string cellphone = post["cellphone"];
            string topic = post["topic"];
            string Content = post["Content"];
            DateTime now = DateTime.Now;
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(topic))
            {
                Advice advice = new Advice();
                advice.name = name;
                advice.cellphone = cellphone;
                advice.topic = topic;
                advice.content = Content;
                advice.date = now;
                DBAccess DB = new DBAccess();
                DB.SetAdvice(advice);
                TempData["message"] = "意見函已傳送";
            }
            else
                TempData["message"] = "未留下正確姓名及主題";

            return View();
        }

    }
}



