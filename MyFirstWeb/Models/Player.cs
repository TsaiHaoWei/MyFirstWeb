using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyFirstWeb.Models
{
    public class Player
    {
       public string PlayerID { get; set; }
       public List<string> BodyList { get; set; }
        public List<string> ColorList { get; set; }


    }
}