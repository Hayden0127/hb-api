using Microsoft.EntityFrameworkCore;
using Strateq.Core.Database.DbModel.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace HB.Database.DbModels
{
    public class UserAccountAuthorizationToken : DbModelBase
    {
        [ForeignKey("Id")]
        public int UserAccountId { get; set; }
        public UserAccount UserAccountAccount { get; set; }
        [Column(TypeName = "nvarchar(300)")]
        public string RefreshToken { get; set; }
    }
}
