using Microsoft.EntityFrameworkCore;
using Strateq.Core.Database.DbModel.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace HB.Database.DbModels
{
    public class PaymentDetails : DbModelBase
    {
        [ForeignKey("Id")]
        public int BookingId { get; set; }
        public Booking Booking { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string CardNumber { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string CVV { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string NameOnCard { get; set; }
        public string ExpirationDate { get; set; }

    }
}
