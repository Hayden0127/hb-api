using AutoMapper.Configuration.Conventions;
using Strateq.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Model
{
    public class HeartBeatConf: ResponseModelBase
    {
        public DateTime currentTime { get; set; }
    }

    public class ResetReq
    {
        public string type { get; set; }
    }

    public class TriggerMessageReq
    {
        public string requestedMessage { get; set; }
        public int? connectorId { get; set; }
    }

    public class ChangeAvailabilityReq
    {
        public int connectorId { get; set; }
        public string type { get; set; }
    }
}
