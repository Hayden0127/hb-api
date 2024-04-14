using Strateq.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Model
{
    public class DashboardDisplayModel : ResponseModelBase
    {
        public List<SiteDetails> SiteList { get; set; }
        public List<ChargerDetails> ChargerList { get; set; }
        public List<HistoryDetails> HistoryList { get; set; }
        public CurrentMonthSummary CurrentMonthSummaryList { get; set; }
    }

    public class SiteDetails
    {
        public int SiteId { get; set; }
        public string SiteName { get; set; }
        public string SiteLocation { get; set; }
        public string Status { get; set; }
        public string StationType { get; set; }
        public int TotalCharger { get; set; }
        public decimal Income { get; set; }
        public decimal TotalEnergy { get; set; }

    }

    public class ChargerDetails
    {
        public int CPConnectorId { get; set; }
        public string CPConnectorName { get; set; }
        public string ProductType { get; set; }
        public string ChargingPointSerialNumber { get; set; }
        public string Status { get; set; }
        public int CPDetailsId { get; set; }
        public int ConnectorId { get; set; }
        public string SiteName { get; set; }
        public int PowerOutput { get; set; }
        public decimal AverageChargeSpeed { get; set; }
        public decimal AveragePowerPercentage { get; set; } 
        public int TotalCharging { get; set; }
        public TimeSpan ChargingTime { get; set; }
    }

    public class HistoryDetails
    {
        public DateTime DateTime { get; set; }
        public string Event { get; set; }
        public int CPConnectorId { get; set; }
        public string CPConnectorName { get; set; }
        public string ProductType { get; set; }
        public string ChargingPointSerialNumber { get; set; }
        public string SiteName { get; set; }
        public string Status { get; set; }
    }
    public class CurrentMonthSummary
    {
        public decimal TotalEnergy { get; set; }
        public decimal HourUtilisation { get; set; }
        public int ActiveCharging { get; set; }
        public decimal Transaction { get; set; }
        public decimal TransactionPercentage { get; set; }
        public int TotalNumberOfTransaction { get; set; }
        public decimal TotalNumberOfTransactionPercentage { get; set; }
    }
}
