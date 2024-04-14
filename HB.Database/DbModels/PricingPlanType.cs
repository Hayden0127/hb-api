using Strateq.Core.Database.DbModel.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Database.DbModels
{
    public class PricingPlanType : DbModelBase
    {
        [ForeignKey("Id")]
        public int PricingPlanId { get; set; }
        public PricingPlan PricingPlan { get; set; }

        [Column(TypeName = "decimal(10,5)")]
        public decimal PriceRate { get; set; }

        [Column(TypeName = "decimal(10,5)")]
        public decimal FixedFee { get; set; }

        [ForeignKey("Id")]
        public int UnitId { get; set; }
        public Unit Unit { get; set; }

        [ForeignKey("Id")]
        public int ProductTypeId { get; set; }
        public ProductType ProductType { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ModifiedOn { get; set; }
    }
}
