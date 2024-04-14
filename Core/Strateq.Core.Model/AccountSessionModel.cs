using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strateq.Core.Model
{
    public class AccountSessionModel
    {
        /// <summary>User Email to get/set session key and value</summary>
        /// <example>example@gmail.com</example>
        public string Email { get; set; }

        /// <summary>User's Name in session value</summary>
        /// <example>testing123</example>
        public string UserName { get; set; }

        /// <summary>User's Access Token in session value</summary>
        public string AccessToken { get; set; }

        /// <summary>User Role</summary>
        public int UserRole { get; set; }

        /// <summary>Array of system permission</summary>
        public int[] PermissionId { get; set; }

        /// <summary>User Password Status</summary>
        public bool TemporaryPassword { get; set; }
    }
}
