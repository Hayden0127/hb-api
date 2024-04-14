using Strateq.Core.Database.DbModel.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Database.DbModels
{
    public class Unit : DbModelBase
    {
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }
    }
}
