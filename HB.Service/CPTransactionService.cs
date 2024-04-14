using AutoMapper;
using HB.Database.DbModels;
using HB.Database.Repositories;
using HB.Model;
using HB.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Strateq.Core.API.Exceptions;
using Strateq.Core.Model;
using Strateq.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Service
{
    public class CPTransactionService : ICPTransactionService
    {
        #region Fields
        private readonly ICPTransactionRepository _cpTransactionRepository;
        private readonly ICPConnectorRepository _cpConnectorRepository;
        private readonly ICPSiteDetailsRepository _cpSiteDetailsRepository;
        private readonly ICPDetailsRepository _cpDetailsRepository;
        private readonly IRunningSequenceService _runningSequenceService;
        private readonly IProductTypeRepository _productTypeRepository;
        private readonly IMeterValueRepository _meterValueRepository;
        private readonly IMapper _mapper;
        private readonly IPriceVariesRepository _priceVariesRepository;
        private readonly IPricingPlanRepository _pricingPlanRepository;
        private readonly IPricingPlanTypeRepository _pricingPlanTypeRepository;
        private readonly IUnitRepository _unitRepository;
        private ISystemLogService _logger;
        #endregion

        #region Ctor
        public CPTransactionService(
            ICPTransactionRepository cpTransactionRepository,
            ICPConnectorRepository cpConnectorRepository,
            ICPSiteDetailsRepository cpSiteDetailsRepository,
            ICPDetailsRepository cpDetailsRepository,
            IRunningSequenceService runningSequenceService,
            IProductTypeRepository productTypeRepository,
			IMeterValueRepository meterValueRepository,
            IMapper mapper, 
            IPriceVariesRepository priceVariesRepository,
            IPricingPlanRepository pricingPlanRepository,
            IPricingPlanTypeRepository pricingPlanTypeRepository,
            IUnitRepository unitRepository,
            ISystemLogService logger)
        {

            _cpTransactionRepository = cpTransactionRepository;
            _cpConnectorRepository = cpConnectorRepository;
            _cpSiteDetailsRepository = cpSiteDetailsRepository;
            _cpDetailsRepository = cpDetailsRepository;
            _runningSequenceService = runningSequenceService;
            _productTypeRepository = productTypeRepository;
            _mapper = mapper;
            _priceVariesRepository = priceVariesRepository;
            _pricingPlanRepository = pricingPlanRepository;
            _pricingPlanTypeRepository = pricingPlanTypeRepository;
            _unitRepository = unitRepository;
			_meterValueRepository = meterValueRepository;
            _logger = logger;
        }

        #endregion

        #region Methods

        public async Task<StartTransactionResponseModel> StartTransaction(StartTransactionRequestDetail model)
        {
            var transactionId = await _runningSequenceService.GetTransactionIdAsync();
            var updateCPDetails = _cpDetailsRepository.ToQueryable().Where(x => x.WebSocketId == model.webSocketId).FirstOrDefault();
            var updateCPConnectors = _cpConnectorRepository.ToQueryable().Where(x => x.CPDetailsId == updateCPDetails!.Id && x.ConnectorId == model.connectorId && x.IsLock == false).FirstOrDefault();

            StartTransactionResponseModel startTransactionResponseModel = new StartTransactionResponseModel()
            {
                Success = true,
                StatusCode = SystemData.StatusCode.Success,
                TransactionId = int.Parse(transactionId)
            };

            if (updateCPDetails == null || updateCPConnectors == null)
            {
                return startTransactionResponseModel = new StartTransactionResponseModel()
                {
                    Success = false,
                    StatusCode = SystemData.StatusCode.NotFound,
                    TransactionId = 0
                };
            };

           
            CPTransaction cPTransaction = new CPTransaction()
            {
                CPDetailsId = updateCPDetails.Id,
                ConnectorId = model.connectorId,
                Country = _cpSiteDetailsRepository.ToQueryable().Where(x => x.Id == updateCPDetails!.CPSiteDetailsId).Select(x => x.Country).FirstOrDefault(),
                TransactionId = int.Parse(transactionId),
                MeterStartValue = model.meterStart,
                StartTime = DateTimeOffset.Parse(model.timestamp).UtcDateTime,
                Status = SystemData.CPTransaction.Started,
                ProductTypeId = _cpConnectorRepository.ToQueryable().Where(x => x.CPDetailsId == updateCPDetails.Id).Select(x => x.ProductTypeId).FirstOrDefault(),
            };

            cPTransaction = await _cpTransactionRepository.AddAndSaveChangesAsync(cPTransaction);

           
            if (updateCPDetails == null)
            {
                return startTransactionResponseModel = new StartTransactionResponseModel()
                {
                    Success = false,
                    StatusCode = SystemData.StatusCode.NotFound,
                    TransactionId = 0
                };
            };

            if (updateCPDetails != null)
            {
                updateCPConnectors.IsLock = true;
                await _cpConnectorRepository.UpdateAndSaveChangesAsync(updateCPConnectors);
            }

            return startTransactionResponseModel;
        }

        public async Task<ResponseModelBase> StopTransaction(StopTransactionReq model)
        {
            ResponseModelBase responseModelBase = new ResponseModelBase()
            {
                Success = true,
                StatusCode = SystemData.StatusCode.Success,
            };

            var cPTransaction = _cpTransactionRepository.ToQueryable().Where(x => x.TransactionId == model.transactionId).FirstOrDefault();
            if (cPTransaction == null)
            {
                return responseModelBase = new ResponseModelBase()
                {
                    Success = false,
                    StatusCode = SystemData.StatusCode.NotFound
                };
            };

            var updateCPDetails = _cpDetailsRepository.ToQueryable().Where(x => x.Id == cPTransaction.CPDetailsId).FirstOrDefault();
            if (updateCPDetails == null)
            {
                return responseModelBase = new ResponseModelBase()
                {
                    Success = false,
                    StatusCode = SystemData.StatusCode.NotFound
                };
            };

            var updateCPConnectors = _cpConnectorRepository.ToQueryable().Where(x => x.CPDetailsId == updateCPDetails!.Id && x.ConnectorId == cPTransaction.ConnectorId).FirstOrDefault();
           
            cPTransaction.TotalAmount = 0;
            cPTransaction.Status = SystemData.CPTransaction.Complete;
            cPTransaction.MeterStopValue = model.meterStop;
            cPTransaction.EndTime = DateTimeOffset.Parse(model.timestamp).UtcDateTime;
            cPTransaction.TotalMeterValue = model.meterStop - cPTransaction.MeterStartValue;
            cPTransaction.TotalHoursTaken = (decimal)cPTransaction.EndTime.Subtract(cPTransaction.StartTime).TotalHours;
            cPTransaction.PaymentDate = DateTime.UtcNow;

            if (updateCPDetails.PricingPlanId != null && updateCPDetails.PricingPlanId != 0 && updateCPConnectors != null)
            {
                var pptQuery = (from ppt in _pricingPlanTypeRepository.ToQueryable()
                               join u in _unitRepository.ToQueryable()
                               on ppt.UnitId equals u.Id
                               join pp in _pricingPlanRepository.ToQueryable()
                               on ppt.PricingPlanId equals pp.Id
                               join pt in _productTypeRepository.ToQueryable()
                               on ppt.ProductTypeId equals pt.Id
                               where ppt.PricingPlanId == updateCPDetails.PricingPlanId &&  ppt.ProductTypeId == updateCPConnectors.ProductTypeId
                               select new PricingPlanTypeDetails()
                               {
                                   Id = ppt.Id,
                                   ProductTypeId = ppt.ProductTypeId,
                                   PriceRate = ppt.PriceRate,
                                   UnitId = ppt.UnitId,
                                   UnitName = u.Name,
                                   ProductTypeName = pt.Name,
                                   FixedFee = pp.FixedFee,
                                   PerBlock = pp.PerBlock
                               }).FirstOrDefault();

                if(pptQuery!= null)
                {
                    if (pptQuery.PerBlock!= null && pptQuery.PerBlock > 0)
                    {
                        switch (pptQuery.UnitName)
                        {
                            case SystemData.Unit.FOC:
                                cPTransaction.TotalAmount = 0;
                                break;
                            case SystemData.Unit.kWh:
                                cPTransaction.TotalAmount = (Math.Ceiling(Convert.ToDecimal(cPTransaction.TotalMeterValue / pptQuery.PerBlock))) * pptQuery.PriceRate + pptQuery.FixedFee;
                                break;
                            case SystemData.Unit.Minutes:
                                cPTransaction.TotalAmount = (Math.Ceiling(Convert.ToDecimal(cPTransaction.TotalHoursTaken * 60 / pptQuery.PerBlock))) * pptQuery.PriceRate + pptQuery.FixedFee;
                                break;
                            case SystemData.Unit.Hours:
                                cPTransaction.TotalAmount = (Math.Ceiling(Convert.ToDecimal(cPTransaction.TotalHoursTaken / pptQuery.PerBlock))) * pptQuery.PriceRate + pptQuery.FixedFee;
                                break;
                        }
                        
                    }
                    else
                    {
                        switch (pptQuery.UnitName)
                        {
                            case SystemData.Unit.FOC:
                                cPTransaction.TotalAmount = 0;
                                break;
                            case SystemData.Unit.kWh:
                                cPTransaction.TotalAmount = (cPTransaction.TotalMeterValue * pptQuery.PriceRate + pptQuery.FixedFee);
                                break;
                            case SystemData.Unit.Minutes:
                                cPTransaction.TotalAmount = (cPTransaction.TotalHoursTaken * 60 * pptQuery.PriceRate + pptQuery.FixedFee);
                                break;
                            case SystemData.Unit.Hours:
                                cPTransaction.TotalAmount = (cPTransaction.TotalHoursTaken * pptQuery.PriceRate + pptQuery.FixedFee);
                                break;
                        }

                    }
                 
                }
            }

            cPTransaction = await _cpTransactionRepository.UpdateAndSaveChangesAsync(cPTransaction);

            if (updateCPDetails == null || updateCPConnectors ==null)
            {
                return responseModelBase = new ResponseModelBase()
                {
                    Success = false,
                    StatusCode = SystemData.StatusCode.NotFound
                };
            };

            if (updateCPDetails != null)
            {
                updateCPConnectors.IsLock = false;
                await _cpConnectorRepository.UpdateAndSaveChangesAsync(updateCPConnectors);
            }

            return responseModelBase;
        }

        public virtual decimal CalculateTotalAmountPerTransaction(decimal totalMeterValue)
        {
            const decimal flatRatePerKiloWatt = 0.80M;
            return totalMeterValue * flatRatePerKiloWatt;
        }

        public TransactionDashboardDisplayModel GetPaymentDashboardDetails(int id)
        {
            var transDashboard = new TransactionDashboardDisplayModel();
            TimeZoneInfo my = TimeZoneInfo.FindSystemTimeZoneById("Singapore");

            var cpTransactionList = (from cpsd in _cpSiteDetailsRepository.ToQueryable()
                                     join cpd in _cpDetailsRepository.ToQueryable()
                                     on cpsd.Id equals cpd.CPSiteDetailsId
                                     join t in _cpTransactionRepository.ToQueryable()
                                     on cpd.Id equals t.CPDetailsId
                                     join pt in _productTypeRepository.ToQueryable()
                                     on t.ProductTypeId equals pt.Id
                                     where cpsd.UserAccountId == id
                                    select new TransactionListModel
                                    {
                                       Id = t.Id,
                                       TransactionId = t.TransactionId,
                                       ProductType = pt.Name,
                                       PaymentDate = t.Status == SystemData.CPTransaction.Started ? null : TimeZoneInfo.ConvertTimeFromUtc(t.PaymentDate, my),
                                       TotalMeterValue = t.TotalMeterValue,
                                       TotalHoursTaken  = t.TotalHoursTaken,
                                       TotalAmount = t.TotalAmount,
                                       Status = t.Status,
                                       CreatedOn = TimeZoneInfo.ConvertTimeFromUtc(t.CreatedOn, my),
                                       IsActive = t.IsActive,
                                       StartTime =t.StartTime
                                    }).ToList();

            var transChart = new TransactionChartDisplayModel();
            decimal totalOrderAmount = decimal.Zero;
            decimal totalCurrPeriodTotalOrderAmt = decimal.Zero;
            decimal totalPrevPeriodTotalOrderAmt = decimal.Zero;
            decimal diffTotalOrder = decimal.Zero;
            IList<MonthlyAmountDisplayModel> totalOrderAmountList = new List<MonthlyAmountDisplayModel>();

            decimal totalPaidAmount = decimal.Zero;
            decimal totalCurrPeriodTotalPaidAmt = decimal.Zero;
            decimal totalPrevPeriodTotalPaidAmt = decimal.Zero;
            decimal diffTotalPaid = decimal.Zero;
            IList<MonthlyAmountDisplayModel> totalPaidAmountList = new List<MonthlyAmountDisplayModel>();

            decimal totalRejectAmount = decimal.Zero;
            decimal totalCurrPeriodTotalRejectAmt = decimal.Zero;
            decimal totalPrevPeriodTotalRejectAmt = decimal.Zero;
            decimal diffTotalReject = decimal.Zero;
            IList<MonthlyAmountDisplayModel> totalRejectAmountList = new List<MonthlyAmountDisplayModel>();

            var currDate = DateTime.UtcNow;
            var convertedCurrDate = TimeZoneInfo.ConvertTimeFromUtc(currDate, TimeZoneInfo.FindSystemTimeZoneById("Singapore"));
            var currDateTime = convertedCurrDate;
            var prevOneMonthDateTime = currDateTime.AddDays(-30);
            var prevTwoMonthDateTime = prevOneMonthDateTime.AddDays(-30);

            foreach (var trans in cpTransactionList)
            {
                if(trans.Status == SystemData.CPTransaction.Complete)
                {
                    totalOrderAmount += trans.TotalAmount;
                    totalPaidAmount += trans.TotalAmount;

                    if(trans.PaymentDate >= prevOneMonthDateTime && trans.PaymentDate <= currDateTime)
                    {
                        totalCurrPeriodTotalOrderAmt += trans.TotalAmount;
                        totalCurrPeriodTotalPaidAmt += trans.TotalAmount;
                    }
                    else if(trans.PaymentDate >= prevTwoMonthDateTime && trans.PaymentDate < prevOneMonthDateTime)
                    {
                        totalPrevPeriodTotalOrderAmt += trans.TotalAmount;
                        totalPrevPeriodTotalPaidAmt += trans.TotalAmount;
                    }
                }
                else if (trans.Status == SystemData.CPTransaction.Rejected)
                {
                    totalOrderAmount += trans.TotalAmount;
                    totalRejectAmount += trans.TotalAmount;

                    if(trans.PaymentDate >= prevOneMonthDateTime && trans.PaymentDate <= currDateTime)
                    {
                        totalCurrPeriodTotalOrderAmt += trans.TotalAmount;
                        totalCurrPeriodTotalRejectAmt += trans.TotalAmount;
                    }
                    else if(trans.PaymentDate >= prevTwoMonthDateTime && trans.PaymentDate < prevOneMonthDateTime)
                    {
                        totalPrevPeriodTotalOrderAmt += trans.TotalAmount;
                        totalPrevPeriodTotalRejectAmt += trans.TotalAmount;
                    }
                }
            }

            if(totalPrevPeriodTotalOrderAmt > 0)
                diffTotalOrder = (totalCurrPeriodTotalOrderAmt - totalPrevPeriodTotalOrderAmt) / totalPrevPeriodTotalOrderAmt;

            if(totalPrevPeriodTotalPaidAmt > 0)
                diffTotalPaid = (totalCurrPeriodTotalPaidAmt - totalPrevPeriodTotalPaidAmt) / totalPrevPeriodTotalPaidAmt;

            if(totalPrevPeriodTotalRejectAmt > 0)
                diffTotalReject = (totalCurrPeriodTotalRejectAmt - totalPrevPeriodTotalRejectAmt) / totalPrevPeriodTotalRejectAmt;
           
            var groupedAmtByMonth = cpTransactionList.AsEnumerable().Where(x => x.PaymentDate != null && (x.PaymentDate?.Year) == convertedCurrDate.Year).GroupBy(x => new { x.PaymentDate?.Month }).ToList();
            
            foreach(var month in groupedAmtByMonth)
            {
                var totalPaidMonth = month.Where(x => x.Status == SystemData.CPTransaction.Complete).Select(x => x.TotalAmount).Sum();
                var totalRejectMonth = month.Where(x => x.Status == SystemData.CPTransaction.Rejected).Select(x => x.TotalAmount).Sum();
                var totalOrderMonth = totalPaidMonth + totalRejectMonth;

                var monthlyTotalAmount = new MonthlyAmountDisplayModel
                {
                    Month = int.Parse(month.Key.Month.ToString() ?? string.Empty),
                    Amount = totalOrderMonth
                };

                var monthlyTotalPaidAmount = new MonthlyAmountDisplayModel
                {
                    Month = int.Parse(month.Key.Month.ToString() ?? string.Empty),
                    Amount = totalPaidMonth
                };

                var monthlyTotalRejectAmount = new MonthlyAmountDisplayModel
                {
                    Month = int.Parse(month.Key.Month.ToString() ?? string.Empty ),
                    Amount = totalRejectMonth
                };

                totalOrderAmountList.Add(monthlyTotalAmount);
                totalPaidAmountList.Add(monthlyTotalPaidAmount);
                totalRejectAmountList.Add(monthlyTotalRejectAmount);
            }

            transChart.TotalOrderAmount = totalOrderAmount;
            transChart.TotalOrderDiff = diffTotalOrder;
            transChart.TotalOrderAmountList = totalOrderAmountList;
            transChart.PaymentSucceedAmount = totalPaidAmount;
            transChart.PaymentSucceedDiff = diffTotalPaid;
            transChart.PaymentSucceedAmountList = totalPaidAmountList;
            transChart.PaymentFailedAmount = totalRejectAmount;
            transChart.PaymentFailedDiff = diffTotalReject;
            transChart.PaymentFailedAmountList = totalRejectAmountList;

            transDashboard.TransactionList = cpTransactionList;
            transDashboard.TransactionChart = transChart;

            return transDashboard;
        }

        public async Task<ResponseModelBase> InsertMeterValuesRecords(MeterValuesRequestModel request)
        {
            ResponseModelBase response = new ResponseModelBase();
            var cpDetails = _cpDetailsRepository.ToQueryable().Where(x => x.WebSocketId == request.WebSocketId).FirstOrDefault();
            
            if (cpDetails == null)
            {
                return response = new ResponseModelBase()
                {
                    Success = false,
                    StatusCode = SystemData.StatusCode.NotFound
                };
            }

            List<MeterValue> meterValueList = new List<MeterValue>();
            foreach(var meterValue in request.MeterValuesReq.meterValue)
            {
                foreach(var sampledValue in meterValue.sampledValue)
                {
                    var newMeterValue = new MeterValue
                    {
                        CPDetailsId = cpDetails.Id,
                        ConnectorId = request.MeterValuesReq.connectorId,
                        TransactionId = request.MeterValuesReq.transactionId ?? null,
                        UniqueId = request.UniqueId,
                        TimeStamp = meterValue.timestamp,
                        CurrentMeterValue = Int32.Parse(sampledValue.value),
                        Context = sampledValue.context,
                        Measurand = sampledValue.measurand,
                        Unit = sampledValue.unit
                    };
                    _mapper.Map<SampledValue, MeterValue>(sampledValue, newMeterValue);
                    meterValueList.Add(newMeterValue);
                }
            }
            await _meterValueRepository.AddRangeAndSaveChangesAsync(meterValueList);

            response.Success = true;
            response.StatusCode = SystemData.StatusCode.Success;
            return response;
        }

        public int GetActiveTransactionIdByConnectorId(RemoteStopTransactionRequestModel request)
        {
            var cpTransactionQuery = _cpTransactionRepository.ToQueryable().Where(x => x.ConnectorId == request.ConnectorId && x.CPDetailsId == request.CPId && x.Status == SystemData.CPTransaction.Started).OrderByDescending(x => x.StartTime).FirstOrDefault();

            if (cpTransactionQuery == null)
            {
                _logger.LogInformation($"CP_{request.CPId} Transaction for ConnectorID {request.ConnectorId} not found");
                throw new Exception("CP Transaction not found");
            }

            return cpTransactionQuery.TransactionId;
        }
        #endregion

    }
}
