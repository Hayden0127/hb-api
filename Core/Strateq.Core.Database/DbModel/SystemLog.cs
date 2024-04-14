using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Strateq.Core.Database.DbModel.Base;

namespace Strateq.Core.Database.DbModel
{
    public class SystemLog : DbModelBase
    {
        [Key]
        public new long Id { get; set; }
        [Column(TypeName = "bigint")]
        public long RequestLogId { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string Controller { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string Action { get; set; }
        [Column(TypeName = "nvarchar(MAX)")]
        public string Detail { get; set; }
        [Column(TypeName = "nvarchar(20)")]
        public string Type { get; set; }
    }
}
