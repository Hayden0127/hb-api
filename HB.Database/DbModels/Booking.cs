using Microsoft.EntityFrameworkCore;
using Strateq.Core.Database.DbModel.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace HB.Database.DbModels
{
    public class Booking : DbModelBase
    {
        [ForeignKey("Id")]
        public int UserAccountId { get; set; }
        public UserAccount UserAccount { get; set; }
        public int Adult { get; set; }
        public int Children { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Person { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Price { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Size { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Src { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Type { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Bed { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime CheckInDate { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime CheckOutDate { get; set; }

    }
}
