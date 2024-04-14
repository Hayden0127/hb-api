using Microsoft.EntityFrameworkCore;
using Strateq.Core.Database.DbModel.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace HB.Database.DbModels
{
    public class CPDetails : DbModelBase
    {
        [ForeignKey("Id")]
        public int CPSiteDetailsId { get; set; }
        public CPSiteDetails CPSiteDetails { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public string Status { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string SerialNo { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string WebSocketId { get; set; }
        public DateTime HeartbeatDateTime { get; set; }
        [ForeignKey("Id")]
        public int? PricingPlanId { get; set; }
        public PricingPlan PricingPlan { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? AssignedOn { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ModifiedOn { get; set; }
        [Column(TypeName = "bit")]
        public bool IsOnline { get; set; }
    }
}
