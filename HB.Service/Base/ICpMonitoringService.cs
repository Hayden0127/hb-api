using HB.Database.DbModels;
using HB.Model;
using Microsoft.Extensions.Configuration;
using Strateq.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Service
{
    public interface ICPMonitoringService
    {
        ChargePointMarkerListResponseModel GetAllChargePointMarkerByUserAccountId(int id);
        ChargePointSummaryDisplayResponseModel GetCPSiteSummaryDisplayByUserAccountId(int id, int duration);
        SearchCPSiteDisplayResponseModel SearchCPSiteDisplayByUserAccountId(SearchCPSiteDisplayRequestModel model);
        Task UpdateCPStatusAsync(StatusNotification req);
        Task<CPMonitoringDisplayModel> GetCPMonitoringChartDetailsAsync(int id);
        Task<ResponseModelBase> BootNotification(BootNotificationModel model);
        Task<DashboardDisplayModel> GetDashboardDetailsAsync(int userId, int pastDays);
        Task<HeartBeatConf> UpdateCPHeartBeatAsync(string webSocketId);
        ViewAllSiteListingResponseModel ViewAllSiteListing(ViewAllSiteListingRequestModel model);
        Task<CPSiteDetails> GetCPSiteDetailsByIdAsync(int id);
        Task<CPSiteDetails> UpdateCPSiteDetailsAsync(CPSiteDetailsRequestModel model);
        Task<CPSiteDetails> DeleteCPSiteDetailsByIdAsync(int id);
        CPDetailsReturnModel GetCPDetailsBySiteId(int id);
        CPDashboardResponseModel DisplayChargePointDashboard(int cpId);
        ConnectorListResponseModel GetConnectorDetails(int id);
        UpdateCPConnectorResponseModel UpdateCPConnector(UpdateCPConnectorRequestModel request);
        Task<ResponseModelBase> DeleteCPConnector(int id);
        Task<UpdateCPDetailsResponseModel> UpdateCPDetailsAsync(UpdateCPDetailsRequestModel request);
        Task<ResponseModelBase> DeleteCPDetailsAsync(int id);
        Task<ResponseModelBase> LockUnlockCPConnectorAsync(LockUnlockRequestModel request);
        Task<CPDetails> UpdateLocalListVersion(GetLocalListVersionConf conf, string webSocketId);
        Task<ResponseModelBase> FirmwareStatusNotification(FirmwareStatusNotificationModel model);
    }
}

