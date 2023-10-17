using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyFirstWeb.Models.ViewModel
{
    public class LoginUserModel
    {
        public string LoginEmail { get; set; }
        public string LoginPassword { get; set; }
        public string RegisterEmail { get; set; }
          
        public string RegisterPassword { get; set; }
        public string RegisterUserName { get; set; }
        public string ForgotUserName { get; set; }

        public string ForgotEmail { get; set; }
        public string NewPassword { get;set; }
        public string Check { get; set; }
    }
}