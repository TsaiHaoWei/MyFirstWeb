using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyFirstWeb.Models
{
    public class Advice
    {
        public string name { get; set; }
        public string cellphone { get; set; }
        public string topic { get; set; }
        public string content { get; set; }
        public DateTime date { get; set; }
    }
}