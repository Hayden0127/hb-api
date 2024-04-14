using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using Strateq.Core.Database.DbModel.Base;

namespace Strateq.Core.Database.DbModel
{
    public class AuditLog : DbModelBase
    {
        [Key]
        public new long Id { get; set; }
        [Column(TypeName = "varchar(150)")]
        public string AffectedTable { get; set; }
        [Column(TypeName = "bigint")]
        public int? AffectedId { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string Operation { get; set; }
    }
}
