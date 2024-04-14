using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Strateq.Core.Database.DbModel.Base;

namespace Strateq.Core.Database.DbModel
{
    public class RequestLog : DbModelBase
    {
        [Key]
        public new long Id { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string Controller { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string Action { get; set; }
        [Column(TypeName = "nvarchar(MAX)")]
        public string Request { get; set; }
        [Column(TypeName = "nvarchar(MAX)")]
        public string Response { get; set; }
        [Column(TypeName = "nvarchar(5)")]
        public string Status { get; set; }
    }
}
