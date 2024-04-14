using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Strateq.Core.Database.DbModel.Base;

namespace Strateq.Core.Database.DbModel
{
    public class AuditLogDetail : DbModelBase
    {
        [Key]
        public new long Id { get; set; } = 0;
        [Column(TypeName = "bigint")]
        public long AuditLogId { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string ColumnName { get; set; }
        [Column(TypeName = "varchar(MAX)")]
        public string OriginalValue { get; set; }
        [Column(TypeName = "varchar(MAX)")]
        public string NewValue { get; set; }
    }
}
