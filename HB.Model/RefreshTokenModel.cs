using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Model
{
    public class RefreshTokenModel
    {
        /// <summary>
        /// User's Expired Access Token
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// User's Current Refresh Token
        /// </summary>
        public string RefreshToken { get; set; }
    }

    public class RefreshResponseModel
    {
        /// <summary>
        /// User's new Access Token
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// User's new Refresh Token
        /// </summary>
        public string RefreshToken { get; set; }
    }
}
