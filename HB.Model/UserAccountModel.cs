using Strateq.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Model
{
    public class NewUserSignUpRequestModel: RequestModelBase
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
    }

    public class UserLoginRequestModel : RequestModelBase
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UserAccountResponseModel : ResponseModelBase
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public string AccessToken { get; set; }
    }

}
