using Strateq.Core.Database.DbModel.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Database.DbModels
{
    public class PricingPlan : DbModelBase
    {
        [Column(TypeName = "nvarchar(200)")]
        public string PlanName { get; set; }

        [ForeignKey("Id")]
        public int PriceVariesId { get; set; }
        public PriceVaries PriceVaries { get; set; }

        [Column(TypeName = "decimal(10,5)")]
        public decimal FixedFee { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ModifiedOn { get; set; }

        [ForeignKey("Id")]
        public int UserAccountId { get; set; }
        public UserAccount UserAccount { get; set; }

        [Column(TypeName = "int")]
        public int? PerBlock { get; set; }
    }
}
