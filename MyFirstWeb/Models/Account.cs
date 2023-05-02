using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyFirstWeb.Models
{
    public class Account
    {
        public string id { get; set; }

        public string account { get; set; }
        public string password { get; set; }
        public string city { get; set; }
        public string village { get; set; }
        public string address { get; set; }
    }
}