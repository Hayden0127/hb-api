using Microsoft.EntityFrameworkCore;
using Strateq.Core.Database.DbModel.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace HB.Database.DbModels
{
    public class GuestDetails : DbModelBase
    {
        [ForeignKey("Id")]
        public int BookingId { get; set; }
        public Booking Booking { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string Address { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Email { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string FirstName { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string LastName { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Phone { get; set; }

    }
}
