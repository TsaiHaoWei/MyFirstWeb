using Dapper;
using DataBaseController;
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
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace MyFirstWeb.Controllers
{
    public class HomeController : Controller
    {
        
        public string SysConnectString;
        private DBAccess DB = new DBAccess();

        //public ActionResult Index()
        //{
        //    DateTime date = DateTime.Now;
        //    Student data = new Student("6","TestNAme",80);

        //    List<Student> list = new List<Student>();
        //    list.Add(new Student("1", "小明", 80));
        //    list.Add(new Student("2", "小華", 70));
        //    list.Add(new Student("3", "小英", 60));
        //    list.Add(new Student("4", "小李", 50));
        //    list.Add(new Student("5", "小張", 90));

        //    //ViewData和ViewBag內的資料都是透過Key/Value的方法來存取，但請注意在同個頁面中他們的key值還是不能重複，否則將會出現問題，後面的值會把前面的值蓋過去，導致讀出來的資料是有問題的。
        //    ViewBag.Date = date;
        //    ViewBag.Student = data;
        //    ViewBag.List = list;          

        //    return View(data);

        //}
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

            Debug.WriteLine($"Res Email:{REmail},ResPassword:{ RPassword}");
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


        public ActionResult HomeOverview()
        {
            return View();
        }
        public ActionResult Index()
        {
            ViewBag.CityList = DB.GetCity();
            ViewBag.VillageList = new Village();
            return View(new UserData()); //前端以 @model接收
        }
        [HttpPost]
        public ActionResult Index(UserData user)
        {
          //  Debug.WriteLine($"p1:{user.password1}     p2:{user.password2}");

            if (!string.IsNullOrEmpty(user.password1) && user.password1.Equals(user.password2))
            {                
                Account a = new Account {id = DB.StroreProcedure("id").ToString(),account = user.account,password = user.password1,city =user.city,village = user.village,address = user.address };
                DB.SetAccount(a);
                //return RedirectToAction("SimpleAlertPostFinish", "Home");
                Response.Redirect("~/Home/Login");
                return new EmptyResult();
            }
            else
            {
                List<City> cityList = DB.GetCity();
                List<Village> villageList = new List<Village>();
                if (!string.IsNullOrWhiteSpace(user.city))
                    villageList = DB.GetVillage(user.city);
                ViewBag.CityList = cityList;
                ViewBag.VillageList = villageList;
                ViewBag.Msg = "密碼輸入錯誤";
                return View(new UserData());
            }
        }
        [HttpPost]
        public ActionResult Village(string id = "") //AJax 
        {
            //Debug.WriteLine(id+"sdf165sd4gs4dfg4s3df4g34d3f4g13");
            List<Village> list = DB.GetVillage(id);
            string result = "";
            if (list == null)
            {
                //讀取資料庫錯誤
                return Json(result);
            }
            else
            {
                result = JsonConvert.SerializeObject(list);
                return Json(result);
            }
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
                foreach (var v in i == 0 ?B1List:B2List)
                {
                    if(v!=null)
                        p.BodyList.Add(v);
                }
                foreach (var v in i == 0 ? C1List : C2List)
                {
                    if(v!=null)
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
            return View();
        }
        public ActionResult Test()
        {
            return View();
        }
       

        /// <summary>
        /// 三種傳輸方式
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        //public ActionResult Transcripts(string id, string name, int score)
        //{
        //    Student data = new Student(id, name, score);
        //    return View(data);
        //}
        //[HttpPost]
        //public ActionResult Transcripts(FormCollection post)
        //{
        //    string id = post["id"];
        //    string name = post["name"];
        //    int score = Convert.ToInt32(post["score"]);
        //    Student data = new Student(id, name, score);
        //    return View(data);
        //}
        [HttpPost]
        public ActionResult Transcripts(Student data)
        {
            return View(data);
        }
   
      
      
    }
}



///   Newtonsoft.Json序列化
//List<Student> lstStuModel = new List<Student>()
//    {
//        new Student(){id="1",name="張飛",score = 30},
//        new Student(){id="2",name="潘金蓮",score = 80}
//    };
//DataInterChange DataExchange = new DataInterChange();
//var v = DataExchange.JsonFomat<Student>(true, lstStuModel, null);
//Debug.WriteLine(((string) v).ToString());
// List<Student> listStudent = DataExchange.JsonFomat<Student>(false, null, " [ { 'id': '1', 'name': '這是第1個項目','score':80 }, { 'id': 2, 'name': '這是第2個項目','score':40 } ] ");
//Debug.WriteLine(string.Format("反序列化： ID={0},Name={1},Sex={2}", listStudent[0].id, listStudent[0].name, listStudent[0].score));  