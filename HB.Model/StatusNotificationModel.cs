using Strateq.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Model
{
    public class UpdateCPStatusRequestModel : RequestModelBase
    {
        public StatusNotification StatusNotification { get; set; }
    }

    public class StatusNotification
    {
        public int ConnectorId { get; set; }
        public string ErrorCode { get; set; }
        public string Info { get; set; }
        public string Status { get; set; }
        public string Timestamp { get; set; }
        public string VendorId { get; set; }
        public string VendorErrorCode { get; set; }
        public string WebSocketId { get; set; }
    }

}
