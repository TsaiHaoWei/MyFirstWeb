using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyFirstWeb.Models
{
    public class User
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
    }
}