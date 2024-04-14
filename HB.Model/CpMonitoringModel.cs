using AutoMapper.Configuration.Conventions;
using HB.Database.DbModels;
using HB.Model;
using Strateq.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Model
{
    public class ChargePointMarkerListResponseModel: ResponseModelBase
    {
        public List<ChargePointMarkerModel> ChargePointMarkerList { get; set; }
    }

    public class ChargePointMarkerModel
    {
        public int Id { get; set; }
        public string SiteName { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public string Address { get; set; }
        public string MaintenanceProgram { get; set; }
        public string Status { get; set; }
        public bool Online { get; set; }
    }

    public class CPMonitoringDisplayModel : ResponseModelBase
    {
        public CPMonitoringDetails CPMonitoringDetails { get; set; } = new();
        public ToDateChargingDetails ToDateChargingDetails { get; set; } = new();
        public PowerUtilizationDetails PowerUtilizationDetails { get; set; } = new();
        public BreakdownErrorDetails BreakdownErrorDetails { get; set; } = new();
    }

    public class CPMonitoringDetails
    {
        public string SiteName { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public string Address { get; set; }
        public string MaintenanceProgram { get; set; }
        public string Status { get; set; }
        public int TotalOrders { get; set; }
        public int CompletedOrders { get; set; }
        public int IncompleteOrders { get; set; }
        public List<ChargingPointCurrentStatus> ChargingPointCurrentStatusList { get; set; } = new();
    }

    public class ChargingPointCurrentStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public bool IsOnline { get; set; }
        public int TotalConnectors { get; set; }
    }

    public class ToDateChargingDetails
    {
        public decimal CO2ReductionKg { get; set; }
        public decimal FuelReplacedLitre { get; set; }
        public int ChargingSession { get; set; }
        public decimal ChargingSessionPercentage { get; set; }
        public List<CPConnectorStatus> CPConnectorStatus { get; set; } = new();
        public int SuccessTransactionCount { get; set; }
        public decimal SuccessTransactionPercentage { get; set; }
        public int FailedTransactionCount { get; set; }
        public decimal FailedTransactionPercentage { get; set; }
    }

    public class CPConnectorStatus
    {
        public string ColorCode { get; set; }
        public string Status { get; set; }
        public int Count { get; set; }
        public decimal Percentage { get; set; }
    }

    public class PowerUtilizationDetails
    {
        public decimal TotalConsumptionKwh { get; set; }
        public decimal GeneratedRevenue { get; set; }
        public decimal EstimatedSavings { get; set; }
        public List<PowerUtilizationChartDetails> PowerUtilizationChartDetails { get; set; } = new();
    }

    public class PowerUtilizationChartDetails
    {
        public string Time { get; set; }
        public decimal KiloWattPerHour { get; set; }
        public decimal ExcessPowerPerHour { get; set; }
    }

    public class BreakdownErrorDetails{
        public int FaultAndConnectivityLostCount { get; set; }
        public int ResolvedCount { get; set; }
        public decimal PaymentFailedAmount { get; set; }
        public List<BreakdownErrorDisplayModel> BreakdownErrorList { get; set; }
    }

    public class BreakdownErrorDisplayModel
    {
        public int BreakdownErrorId { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorDescription { get; set; }
        public int Status { get; set; }
        public int Severity { get; set; }
        public string Workaround { get; set; }
        public DateTime TimeOccur { get; set; }
    }

    public class ResetRequest
    {
        public string type { get; set; }
    }

    public class BootNotificationModel
    {
        public string chargePointVendor { get; set; }
        public string chargePointModel { get; set; }
        public string chargePointSerialNumber { get; set; }
        public string chargeBoxSerialNumber { get; set; }
        public string firmwareVersion { get; set; }
        public string iccid { get; set; }
        public string imsi { get; set; }
        public string meterType { get; set; }
        public string meterSerialNumber { get; set; }
        public string webSocketId { get; set; }

    }

    public class SearchSiteRequest : PaginationQueryStringParameters
    {
        public int UserAccontId { get; set; }
        public string SearchString { get; set; }
        public string OrderColumn { get; set; }
        public string OrderBy { get; set; }

    }

    public class ViewAllSiteListingRequestModel : RequestModelBase
    {
        public SearchSiteRequest SearchSiteRequest { get; set; }
    }

    public class CPSiteDetailsDisplayModel
    {
        public int CPId { get; set; }
        public string SiteName { get; set; }
        public string CPName { get; set; }
        public string CPArea { get; set; }
        public string Status { get; set; }
        public int TotalConnector { get; set; }
        public decimal IncomeOfTheDay { get; set; }
        public int? LoadLimit { get; set; }
    }

    public class PagedCPSiteDetailsList : PagerModel
    {
        public List<CPSiteDetailsDisplayModel> CPSiteDetailsList { get; set; }
    }

    public class ViewAllSiteListingResponseModel : ResponseModelBase
    {
        public PagedCPSiteDetailsList CPSiteDetailsList { get; set; }
    }

    public class ChargePointSummaryDisplayResponseModel: ResponseModelBase
    {
        public int TotalCharge { get; set; }
        public decimal TotalEVUsage { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    public class SearchCPSiteDisplayRequestModel : RequestModelBase
    {
        public int UserAccountId { get; set; }
        public string SearchKeyword { get; set; }
        public int? SelectedCPSiteId { get; set; }
    }

    public class SearchCPSiteDisplayResponseModel : ResponseModelBase
    {
        public List<ChargePointSiteDisplaySummary> ChargePointSiteDisplaySummaryListing { get; set; } = new();
    }

    public class ChargePointSiteDisplaySummary
    {
        public string CPSiteName { get; set; }
        public string CPSiteLocation { get; set; }
        public string CPSiteStatus { get; set; }
        public int TotalChargePoint { get; set; }
        public decimal ToDateCPIncome { get; set; }
        public bool IsSelected { get; set; }
    }

    public class LocationCoordinates
    {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
    public class CPSiteDetailsRequestModel
    {
        public int Id { get; set; }
        public string PersonInCharge { get; set; }
        public string Email { get; set; }
        public string OfficeNo { get; set; }
        public string MobileNo { get; set; }
        public string OperationalStatus { get; set; }
        public string MaintenanceProgram { get; set; }
        public string Description { get; set; }
        public DateTime UpdatedOn { get; set; }

    }

    public class CPDetailsReturnModel
    {
        public List<CPDetailsViewModel> CPDetails { get; set; }
        public int TotalCharge { get; set; }
        public decimal TotalEVUsage { get; set; }
        public decimal Revenue { get; set; }
    }

    public class CPDashboardRequestModel: RequestModelBase
    {
        public int Id { get; set; }
    }

    public class CPDashboardResponseModel : ResponseModelBase
    {
        public KwhConsumptionGraphDetails GraphDetails { get; set; }
        public decimal TotalKwhConsumed { get; set; }
        public decimal TotalRevenue { get; set; }
        public List<CPTransactionHistoryDetails> TransactionHistoryDetails { get; set; }
        public BreakdownErrorDetails BreakdownErrorDetails { get; set; }
        public CPDetailsViewModel CPDetails { get; set; }
    }

    public class CPTransactionHistoryDetails
    {
        public int TransactionId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TimeIn { get; set; }
        public string TimeOut { get; set; }
        public decimal KwhConsumed { get; set; }
        public decimal Amount { get; set; }
        public string PaymentStatus { get; set; }
    }

    public class KwhConsumptionGraphDetails
    {
        public List<string> Time { get; set; }
        public List<decimal> KwhConsumed { get; set; }
    }

    public class CPDetailsViewModel
    {
        public int Id { get; set; }
        public string Charger { get; set; }
        public string SerialNo { get; set; }
        public int TotalCharge { get; set; }
        public decimal UnitUsage { get; set; }
        public decimal Revenue { get; set; }
        public string OperationalStatus { get; set; }

        public string SiteName { get; set; }
        public string CreatedOn { get; set; }
        public string UpdatedOn { get; set; }
        public string PlanAssigned { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
    }

    public class ConnectorDetails
    {
        public int Id { get; set; }
        public string ConnectorName { get; set; }
        public string ProductType { get; set; }
        public int PowerOutput { get; set; }
        public string Status { get; set; }
        public int PricingPlanId { get; set; }
        public bool IsLock { get; set; }
    }

    public class ConnectorListResponseModel: ResponseModelBase
    {
        public List<ConnectorDetails> ConnectorDetails { get; set; }
    }

    public class UpdateCPConnectorRequestModel : RequestModelBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProductTypeId { get; set; }
        public int PricingPlanId { get; set; }
        public int PowerOutput { get; set; }
    }

    public class UpdateCPConnectorResponseModel : ResponseModelBase
    {
        public CPConnector ConnectorDetails { get; set; }
    }

    public class UpdateCPDetailsRequestModel: RequestModelBase
    {
        public int CPId { get; set; }
        public string Name { get; set; }
        public string SerialNo { get; set; }
    }

    public class UpdateCPDetailsResponseModel: ResponseModelBase
    {
        public CPDetails CPDetails { get; set; } 
    }

    public class LockUnlockRequestModel: RequestModelBase
    {
        public List<int> Ids { get; set; }
        public bool Lock { get; set; }
    }

    public class UnlockConnectorReq
    {
        public int connectorId { get; set; }
    }

    public class RemoteStopTransactionRequestModel
    {
        public int ConnectorId { get; set; }
        public int CPId { get; set; }
    }

    public class GetLocalListVersionConf
    {
        public int listVersion { get; set; }
    }

    public class SendLocalListReq
    {
        public int listVersion { get; set; }
        public AuthorizationData localAuthorizationList { get; set; }
        public string updateType { get; set; }
    }

    public class AuthorizationData
    {
        public string idTag { get; set; }
        public IdTagInfo idTagInfo { get; set; }
    }

    public class IdTagInfo
    {
        public DateTime? expiryDate { get; set; }
        public string parentIdTag { get; set; }
        public string status { get; set; }
    }

    public class GetLocalListVersionRequestModel
    {
        public int MessageType { get; set; }
        public int ListVersion { get; set; }
        public string WebSocketId { get; set; }
    }

    public class SendLocalListRequestModel
    {
        public int ListVersion { get; set; }
        public int UpdateType { get; set; }
    }

    public class FirmwareStatusNotificationModel
    {
        public string status { get; set; }
        public string websocketId { get; set; }
    }
}
