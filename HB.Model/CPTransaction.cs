using Strateq.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Model
{

    public class StartTransactionResponseModel :  ResponseModelBase
    {
        public int TransactionId { get; set; }

    }

    public class StartTransactionRequestDetail
    {
        public int connectorId { get; set; }
        public string idTag { get; set; }
        public int meterStart { get; set; }
        public int reservationId { get; set; }
        public string timestamp { get; set; }
        public string webSocketId { get; set; }
    }

    public class StopTransactionReq
    {
        public int transactionId { get; set; }
        public string idTag { get; set; }
        public int meterStop { get; set; }
        public string timestamp { get; set; }

    }

    public class TransactionDashboardDisplayModel
    {
        public IList<TransactionListModel> TransactionList { get; set; }
        public TransactionChartDisplayModel TransactionChart { get; set; }
    }

    public class TransactionListModel
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public string ProductType { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal TotalMeterValue { get; set; }
        public decimal TotalHoursTaken { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public DateTime? StartTime { get; set; }
    }

    public class TransactionChartDisplayModel
    {
        public decimal TotalOrderAmount { get; set; }
        public decimal TotalOrderDiff { get; set; }
        public IList<MonthlyAmountDisplayModel> TotalOrderAmountList { get; set; }
        public decimal PaymentSucceedAmount { get; set; }
        public decimal PaymentSucceedDiff { get; set; }
        public IList<MonthlyAmountDisplayModel> PaymentSucceedAmountList { get; set; }
        public decimal PaymentFailedAmount { get; set; }
        public decimal PaymentFailedDiff { get; set; }
        public IList<MonthlyAmountDisplayModel> PaymentFailedAmountList { get; set; }
    }

    public class MonthlyAmountDisplayModel
    {
        public int Month { get; set; }
        public decimal Amount { get; set; }
    }

    public class RemoteStartTransactionReq
    {
        public int connectorId { get; set; }
        public string idTag { get; set; }
        public ChargingProfile chargingProfile { get; set; }
    }
    public class ChargingSchedule
    {
        public int duration { get; set; }
        public DateTime startSchedule { get; set; }
        public string chargingRateUnit { get; set; }
        public ChargingSchedulePeriod chargingSchedulePeriod { get; set; }
        public decimal minChargingRate { get; set; }
    }

    public class ChargingSchedulePeriod
    {
        public int startPeriod { get; set; }
        public decimal limit { get; set; }
        public int numberPhases { get; set; }
    }
    public class ChargingProfile
    {
        public int chargingProfileId { get; set; }
        public int? transactionId { get; set; }
        public int stackLevel { get; set; }
        public string ChargingProfilePurpose { get; set; }
        public string ChargingProfileKind { get; set; }
        public string RecurrencyKind { get; set; }
        public DateTime validFrom { get; set; }
        public DateTime validTo { get; set; }
        public ChargingSchedule chargingSchedule { get; set; }
    }

    public class RemoteStopTransactionReq
    {
        public int transactionid { get; set; }
    }

    public class MeterValuesReq
    {
        public int connectorId { get; set; }
        public int? transactionId { get; set; }
        public List<MeterValues> meterValue { get; set; } = new();
    }

    public class MeterValues
    {
        public DateTime timestamp { get; set; }
        public List<SampledValue> sampledValue { get; set; } = new();
    }

    public class SampledValue
    {
        public string value { get; set; }
        public string context { get; set; }
        public string format { get; set; }
        public string measurand { get; set; }
        public string phase { get; set; }
        public string location { get; set; }
        public string unit { get; set; }
    }

    public class MeterValuesRequestModel
    {
        public MeterValuesReq MeterValuesReq { get; set; }
        public string UniqueId { get; set; }
        public string WebSocketId { get; set; }
    }

    public class UpdateFirmwareReq
    {
        public string location { get; set; }
        public int retries { get; set; }
        public DateTime retrieveDate { get; set; }
        public int retryInterval { get; set; }

    }

}
