using Dapper;
using DataBaseController;
using MyFirstWeb.Controllers.UtilityFolder;
using MyFirstWeb.Models;
using MyFirstWeb.Models.UtilityFolder;
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
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;

namespace MyFirstWeb.Controllers
{
    public class HomeController : Controller
    {
        //First Page絕對不會是Post
        public string SysConnectString;
        private DBAccess DB = new DBAccess();

        public ActionResult HomeOverview()
        {
            return View();
        }
        public ActionResult HomePage()
        {
            return View();
        }
        [HttpPost]
        public ActionResult HomePage(FormCollection post)
        {
            string LEmail = post["LoginEmail"];//name:
            string LPassword = post["LoginPassword"];
            string REmail = post["RegisterEmail"];//name:
            string RPassword = post["RegisterPassword"];
            string RUserName = post["RegisterUserName"];
            string FUserName = post["ForgotUserName"];
            string FEmail = post["ForgotEmail"];
            string NewPassword = post["NewPassword"];
            string LoginCheck = post["Check"];

            Debug.WriteLine($"Res Email:{REmail},ResPassword:{RPassword}");
            //Register
            if (!string.IsNullOrEmpty(REmail) && !string.IsNullOrEmpty(RPassword) && !string.IsNullOrEmpty(RUserName))
            {
                if (DB.GetAccount().FindAll(var => var.Email.Equals(REmail)).Count == 0)
                {
                    User Account = new User();
                    Account.UserId = DB.StroreProcedure("id").ToString();
                    Account.UserName = RUserName;
                    Account.Password = RPassword;
                    Account.Email = REmail;
                    if (DB.SetAccount(Account))
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
                List<User> UserList = DB.GetAccount();
                if (UserList.Find(var => var.Email.Equals(LEmail) && var.Password.Equals(LPassword)) != null)
                {
                    return RedirectToAction("NuNuGaming", "Home");
                }
                else
                {
                    TempData["message"] = "登入資料錯誤";

                }


            }
            else {
                if (!string.IsNullOrEmpty(FUserName) && !string.IsNullOrEmpty(FEmail) && !string.IsNullOrEmpty(NewPassword))
                {
                    User Account = new User();
                    Account.UserName = FUserName;
                    Account.Password = NewPassword;
                    Account.Email = FEmail;
                    if (DB.UpdateAccount(Account))
                    {
                        TempData["message"] = "更改成功";
                    }
                }

            }
            return View();

        }
        public ActionResult NuNuGaming()
        {
            return View();
        }
        [HttpPost]
        public ActionResult NuNuGaming(FormCollection post)
        {

            string b1 = post["bodylist1"];

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

        public ActionResult Game2()
        {
            CallAPI APIC = new CallAPI();
            Debug.WriteLine("資料庫獨到資料:" + APIC.APIGetProduct().Count);
            return View();
        }


        //傳輸方式後端傳至前端 
        //{
        //    1.ViewBag
        //    2.View(Model)
        //    }
        //    前傳至後端
        //    {
        //         1. Parameter get
        //         2.FormCollection Parameter get
        //string id = post["id"];
        //string name = post["name"];
        //int score = Convert.ToInt32(post["score"]);
        //        3. Model(userData) Parameter get
        //}

      
    }
}



