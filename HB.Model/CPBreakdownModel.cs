using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Model
{
    public class CPBreakdownDurationDetailsModel
    {
        public int CPDetailsId { get; set; }
        public int ConnectorId { get; set; }
        public int CPBreakdownErrorId { get; set; }
        public int Status { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string ProductType { get; set; }
    }
}
