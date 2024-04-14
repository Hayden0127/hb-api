using AutoMapper;
using HB.Database.DbModels;
using HB.Database.Repositories;
using HB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HB.Utilities;

namespace HB.Service
{
    public class CPBreakdownErrorService : ICPBreakdownErrorService
    {
        #region Fields
        private readonly ICPBreakdownErrorRepository _cpBreakdownErrorRepository;
        private readonly ICPTransactionRepository _cpTransactionRepository;
        private readonly ICPBreakdownDurationDetailsRepository _cpBreakdownDurationDetailsRepository;
        private readonly ICPDetailsRepository _cpDetailsRepository;
        private readonly ICPConnectorRepository _cpConnectorRepository;
        private readonly IProductTypeRepository _productTypeRepository;
        #endregion

        #region Ctor
        public CPBreakdownErrorService(
            ICPBreakdownErrorRepository cPBreakdownErrorRepository,
            ICPTransactionRepository cpTransactionRepository,
            ICPBreakdownDurationDetailsRepository cpBreakdownDurationDetailsRepository,
            ICPDetailsRepository cpDetailsRepository,
            ICPConnectorRepository cpConnectorRepository,
            IProductTypeRepository productTypeRepository)
        {
            _cpBreakdownErrorRepository = cPBreakdownErrorRepository;
            _cpTransactionRepository = cpTransactionRepository;
            _cpBreakdownDurationDetailsRepository = cpBreakdownDurationDetailsRepository;
            _cpDetailsRepository = cpDetailsRepository;
            _cpConnectorRepository = cpConnectorRepository;
            _productTypeRepository = productTypeRepository;
        }

        #endregion

        #region Methods
        public BreakdownErrorDetails GetErrorListByCPId(int id)
        {
            var response = new BreakdownErrorDetails();
            TimeZoneInfo my = TimeZoneInfo.FindSystemTimeZoneById("Singapore");

            var paymentFailedTransaction = _cpTransactionRepository.ToQueryable().Where(x => x.Status == SystemData.CPTransaction.Rejected).Select(x => x.TotalAmount).ToList();

            var cpIds = _cpDetailsRepository.ToQueryable()!.Where(x => x.CPSiteDetailsId == id).Select(x => x.Id).ToList();
            var errorList = (from error in _cpBreakdownErrorRepository.ToQueryable()
                             where cpIds.Contains(error.CPDetailsId) &&
                             error.TimeStamp >= DateTime.UtcNow.AddDays(-1)
                             orderby error.TimeStamp
                             select new BreakdownErrorDisplayModel
                             {
                                 BreakdownErrorId = error.Id,
                                 ErrorCode = error.ErrorCode,
                                 ErrorDescription = error.ErrorDescription,
                                 Status = error.Status,
                                 Severity = error.Severity,
                                 TimeOccur = TimeZoneInfo.ConvertTimeToUtc(error.TimeStamp, my)
                             }).ToList();

            response.FaultAndConnectivityLostCount = errorList.Count;
            response.ResolvedCount = errorList.Where(x => x.Status == SystemData.BreakdownErrorStatus.Fixed).Count();
            response.PaymentFailedAmount = paymentFailedTransaction.Sum();

            response.BreakdownErrorList = errorList;

            return response;
        }

        public List<CPBreakdownDurationDetailsModel> GetBreakdownDurationByCPId(int id)
        {
            TimeZoneInfo my = TimeZoneInfo.FindSystemTimeZoneById("Singapore");
            var t = DateTime.UtcNow.AddDays(-1);

            var query = (from error in _cpBreakdownErrorRepository.ToQueryable()
                        join duration in _cpBreakdownDurationDetailsRepository.ToQueryable()
                        on error.Id equals duration.CPBreakdownErrorId
                        join connector in _cpConnectorRepository.ToQueryable()
                        on new { error.CPDetailsId, error.ConnectorId } equals new { connector.CPDetailsId, connector.ConnectorId }
                        join pt in _productTypeRepository.ToQueryable()
                        on connector.ProductTypeId equals pt.Id
                        where error.CPDetailsId == id && error.TimeStamp >= DateTime.UtcNow.AddDays(-1)
                        select new CPBreakdownDurationDetailsModel{
                            CPDetailsId = error.CPDetailsId,
                            ConnectorId = error.ConnectorId,
                            CPBreakdownErrorId = error.Id,
                            Status = error.Status,
                            StartTime = TimeZoneInfo.ConvertTimeFromUtc(duration.StartTime,my),
                            EndTime = TimeZoneInfo.ConvertTimeFromUtc(duration.EndTime,my),
                            ProductType = pt.Name
                        }).ToList();
            return query;
        }

        #endregion
    }
}
