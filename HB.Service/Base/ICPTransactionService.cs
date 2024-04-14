using HB.Model;
using Strateq.Core.Model;

namespace HB.Service
{
    public interface ICPTransactionService
    {
        Task<StartTransactionResponseModel> StartTransaction(StartTransactionRequestDetail model);
        Task<ResponseModelBase> StopTransaction(StopTransactionReq model);
        TransactionDashboardDisplayModel GetPaymentDashboardDetails(int id);
        Task<ResponseModelBase> InsertMeterValuesRecords(MeterValuesRequestModel request);
        int GetActiveTransactionIdByConnectorId(RemoteStopTransactionRequestModel request);
    }
}