using Microsoft.EntityFrameworkCore;
using Strateq.Core.Database.DbModel.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace HB.Database.DbModels
{
    public class UserAccount : DbModelBase
    {
        [Column(TypeName = "nvarchar(50)")]
        public string Email { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string FullName { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string SaltHashPassword { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string Salt { get; set; }

        [Column(TypeName = "bit")]
        public bool IsTemporaryPassword { get; set; }
    }
}
