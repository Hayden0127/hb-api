using AutoMapper;
using HB.Database.DbModels;
using HB.Database.Repositories;
using HB.Model;
using HB.Utilities;
using HB.SmartSD.Integrator;
using DocumentFormat.OpenXml.Drawing.ChartDrawing;
using DocumentFormat.OpenXml.Vml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using Strateq.Core.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.InkML;
using Strateq.Core.Service;
using Azure.Storage.Blobs.Models;
using DocumentFormat.OpenXml.Spreadsheet;
using Strateq.Core.API.Exceptions;
using AutoMapper.Configuration.Annotations;
using Org.BouncyCastle.Ocsp;

namespace HB.Service
{
    public class CPMonitoringService : ICPMonitoringService
    {
        #region Fields

        private readonly ICPSiteDetailsRepository _cpSiteDetailsRepository;
        private readonly ICPDetailsRepository _cpDetailsRepository;
        private readonly ICPConnectorRepository _cpConnectorRepository;
        private readonly ICPBreakdownErrorRepository _cpBreakdownErrorRepository;
        private readonly ICPBreakdownErrorService _cpBreakdownErrorService;
        private readonly ICPTransactionRepository _cpTransactionRepository;
        private readonly ICPBreakdownDurationDetailsRepository _cpBreakdownDurationDetailsRepository;
        private readonly IMapper _mapper;
        private readonly ISystemLogService _logger;
        private readonly IProductTypeRepository _productTypeRepository;
        private readonly IOnBoardingService _onBoardingService;
        private readonly ServiceHelper _smartSDServiceHelper;

        #endregion

        #region Ctor
        public CPMonitoringService(ICPSiteDetailsRepository cpSiteDetailsRepository,
            ICPDetailsRepository cpDetailsRepository,
            ICPConnectorRepository cpConnectorRepository,
            ICPBreakdownErrorRepository cpBreakdownErrorRepository,
            ICPBreakdownErrorService cPBreakdownErrorService,
            ICPTransactionRepository cpTransactionRepository,
            ICPBreakdownDurationDetailsRepository cpBreakdownDurationDetailsRepository,
            IMapper mapper,
            ISystemLogService logger, 
            IProductTypeRepository productTypeRepository,
            IOnBoardingService onBoardingService)
        {
            _cpSiteDetailsRepository = cpSiteDetailsRepository;
            _cpDetailsRepository = cpDetailsRepository;
            _cpConnectorRepository = cpConnectorRepository;
            _cpBreakdownErrorRepository = cpBreakdownErrorRepository;
            _cpBreakdownErrorService = cPBreakdownErrorService;
            _cpTransactionRepository = cpTransactionRepository;
            _cpBreakdownDurationDetailsRepository = cpBreakdownDurationDetailsRepository;
            _mapper = mapper;
            _logger = logger;
            _smartSDServiceHelper = new ServiceHelper(logger);
            _productTypeRepository = productTypeRepository;
            _onBoardingService = onBoardingService;
        }

        #endregion
        public ChargePointMarkerListResponseModel GetAllChargePointMarkerByUserAccountId(int id)
        {
            var cpMarker = from cpsd in _cpSiteDetailsRepository.ToQueryable()
                           where cpsd.UserAccountId == id
                           let cpDetails = _cpDetailsRepository.ToQueryable().Where(x => x.CPSiteDetails.Id == cpsd.Id).ToList()
                           let cpConnector = _cpConnectorRepository.ToQueryable().Where(x => (cpDetails.Select(x=>x.Id)).Contains(x.CPDetailsId)).ToList()
                           let unavailableCP = cpDetails.Select(x => x.Status).Contains(SystemData.CPStatus.Unavailable) || cpDetails.Select(x => x.Status).Contains(SystemData.CPStatus.Faulted)
                           let unavailableConnector = cpConnector.Select(x => x.Status).Contains(SystemData.CPConnectorStatus.OutOfService)
                           select new ChargePointMarkerModel
                           {
                               Id = cpsd.Id,
                               SiteName = cpsd.SiteName,
                               Longitude = cpsd.Longitude,
                               Latitude = cpsd.Latitude,
                               Address = cpsd.Address,
                               MaintenanceProgram = cpsd.MaintenanceProgram,
                               Status = cpsd.Status,
                               Online = (unavailableCP || unavailableConnector) == true ? false : true
                           };            

            ChargePointMarkerListResponseModel returnModel = new ChargePointMarkerListResponseModel
            {
                ChargePointMarkerList = cpMarker.ToList(),
                Success = true,
                StatusCode = SystemData.StatusCode.Success
            };

            return returnModel;
        }

        public ChargePointSummaryDisplayResponseModel GetCPSiteSummaryDisplayByUserAccountId(int id, int duration)
        {
            var totalCPCharge = from cpsd in _cpSiteDetailsRepository.ToQueryable()
                                join cpd in _cpDetailsRepository.ToQueryable()
                                on cpsd.Id equals cpd.CPSiteDetailsId
                                join cpt in _cpTransactionRepository.ToQueryable()
                                on cpd.Id equals cpt.CPDetailsId
                                where cpsd.UserAccountId == id && cpt.Status == SystemData.CPTransaction.Complete
                                select cpt;

            switch (duration)
            {
                case 0:
                    totalCPCharge = totalCPCharge.Where(x => x.CreatedOn.Date == DateTime.UtcNow.Date);
                    break;
                case 1:
                    totalCPCharge = totalCPCharge.Where(x => x.CreatedOn >= DateTime.UtcNow.AddMonths(-1) && x.CreatedOn <= DateTime.UtcNow);
                    break;
                case 2:
                    totalCPCharge = totalCPCharge.Where(x => x.CreatedOn >= DateTime.UtcNow.AddYears(-1) && x.CreatedOn <= DateTime.UtcNow);
                    break;
                default:
                    totalCPCharge = totalCPCharge.Where(x => x.CreatedOn.Date == DateTime.UtcNow.Date);
                    break;
            }

            ChargePointSummaryDisplayResponseModel returnModel = new ChargePointSummaryDisplayResponseModel
            {
                TotalCharge = totalCPCharge.Count(),
                TotalEVUsage = totalCPCharge.Sum(x => x.TotalMeterValue),
                TotalRevenue = totalCPCharge.Sum(x => x.TotalAmount),
                Success = true,
                StatusCode = SystemData.StatusCode.Success
            };

            return returnModel;
        }

        public SearchCPSiteDisplayResponseModel SearchCPSiteDisplayByUserAccountId(SearchCPSiteDisplayRequestModel model)
        {
            var cpSiteQuery = from cpsd in _cpSiteDetailsRepository.ToQueryable()
                              where cpsd.UserAccountId == model.UserAccountId &&
                              (cpsd.City.Contains(model.SearchKeyword) || cpsd.State.Contains(model.SearchKeyword))
                              select cpsd;

            List<CPSiteDetails> _tempList = new List<CPSiteDetails>();

            if (model.SelectedCPSiteId != null)
            {
                var selectedCPSiteQuery = _cpSiteDetailsRepository.ToQueryable().Where(x => x.Id == model.SelectedCPSiteId).FirstOrDefault();

                if (selectedCPSiteQuery != null)
                {
                    foreach (var cpSite in cpSiteQuery.ToList())
                    {
                        LocationCoordinates center = new LocationCoordinates { Longitude = selectedCPSiteQuery.Longitude, Latitude = selectedCPSiteQuery.Latitude };
                        LocationCoordinates location = new LocationCoordinates { Longitude = cpSite.Longitude, Latitude = cpSite.Latitude };

                        var distance = Distance(center, location);

                        if (distance <= 5)
                        {
                            _tempList.Add(cpSite);
                        }
                    }
                }                
            }
            else
            {
                _tempList = cpSiteQuery.ToList();
            }

            var displayResult = from cpSite in _tempList.AsQueryable()
                                let refCPIds = _cpDetailsRepository.ToQueryable().Where(x => x.CPSiteDetailsId == cpSite.Id).Select(x => x.Id).ToList()
                                select new ChargePointSiteDisplaySummary()
                                {
                                    CPSiteName = cpSite.SiteName,
                                    CPSiteLocation = $"{cpSite.City}, {cpSite.State}",
                                    CPSiteStatus = cpSite.OperationalStatus,
                                    TotalChargePoint = _cpDetailsRepository.ToQueryable()!.Where(x => x.CPSiteDetailsId == cpSite.Id).Count(),
                                    ToDateCPIncome = _cpTransactionRepository.ToQueryable()!.Where(x => refCPIds.Contains(x.CPDetailsId) && x.Status == SystemData.CPTransaction.Complete && x.CreatedOn.Date == DateTime.UtcNow.Date).Sum(x => x.TotalAmount),
                                    IsSelected = cpSite.Id == model.SelectedCPSiteId
                                };

            SearchCPSiteDisplayResponseModel returnModel = new SearchCPSiteDisplayResponseModel
            {
                ChargePointSiteDisplaySummaryListing = displayResult.ToList(),
                Success = true,
                StatusCode = SystemData.StatusCode.Success
            };

            return returnModel;
        }

        public async Task UpdateCPStatusAsync(StatusNotification req)
        {
            var cpDetails = _cpDetailsRepository.ToQueryable().Where(x => x.WebSocketId.ToLower() == req.WebSocketId.ToLower()).FirstOrDefault();

            if (cpDetails == null)
            {
                await _logger.LogInformation($"Id Tag {req.WebSocketId} not found");
                throw new Exception("CPDetails not found");
            }
            await _logger.LogInformation($"UpdateCPStatus SerialNo - {cpDetails.SerialNo}");

            if (req.ConnectorId == 0)
            {
                cpDetails.Status = req.Status;
                await _cpDetailsRepository.UpdateAndSaveChangesAsync(cpDetails);

                var cpConnectorDetails = _cpConnectorRepository.ToQueryable().Where(x => x.CPDetailsId == cpDetails.Id).ToList();
                cpConnectorDetails.ForEach(x => x.Status = req.Status);
                await _cpConnectorRepository.UpdateRangeAndSaveChangesAsync(cpConnectorDetails);
            }
            else if (req.ConnectorId > 0)
            {
                var cpConnectorDetails = _cpConnectorRepository.ToQueryable().Where(x => x.CPDetailsId == cpDetails.Id && x.ConnectorId == req.ConnectorId).FirstOrDefault();

                if (cpConnectorDetails == null)
                {
                    await _logger.LogInformation($"Id Tag {req.VendorId} with Connector Id {req.ConnectorId} not found");
                    throw new Exception("CP Connector not found");
                }               

                cpConnectorDetails.Status = req.Status;
                await _cpConnectorRepository.UpdateAndSaveChangesAsync(cpConnectorDetails);
            }

            var dateTime = DateTimeOffset.Parse(req.Timestamp).DateTime;
            var utcDatetime = TimeZoneInfo.ConvertTimeToUtc(dateTime);
            var breakdownError = new CPBreakdownError();

            if (req.ErrorCode != SystemData.CPErrorCode.NoError && (req.Status == SystemData.CPStatus.Faulted || req.Status == SystemData.CPStatus.Unavailable))
            {
                switch (req.ErrorCode)
                {
                    case SystemData.CPErrorCode.ConnectorLockFailure:
                        req.Info = SystemData.CPErrorCodeDescription.ConnectorLockFailure;
                        break;
                    case SystemData.CPErrorCode.EVCommunicationError:
                        req.Info = SystemData.CPErrorCodeDescription.EVCommunicationError;
                        break;
                    case SystemData.CPErrorCode.GroundFailure:
                        req.Info = SystemData.CPErrorCodeDescription.GroundFailure;
                        break;
                    case SystemData.CPErrorCode.HighTemperature:
                        req.Info = SystemData.CPErrorCodeDescription.HighTemperature;
                        break;
                    case SystemData.CPErrorCode.InternalError:
                        req.Info = SystemData.CPErrorCodeDescription.InternalError;
                        break;
                    case SystemData.CPErrorCode.LocalListConflict:
                        req.Info = SystemData.CPErrorCodeDescription.LocalListConflict;
                        break;
                    case SystemData.CPErrorCode.OtherError:
                        req.Info = SystemData.CPErrorCodeDescription.OtherError;
                        break;
                    case SystemData.CPErrorCode.OverCurrentFailure:
                        req.Info = SystemData.CPErrorCodeDescription.OverCurrentFailure;
                        break;
                    case SystemData.CPErrorCode.OverVoltage:
                        req.Info = SystemData.CPErrorCodeDescription.OverVoltage;
                        break;
                    case SystemData.CPErrorCode.PowerMeterFailure:
                        req.Info = SystemData.CPErrorCodeDescription.PowerMeterFailure;
                        break;
                    case SystemData.CPErrorCode.PowerSwitchFailure:
                        req.Info = SystemData.CPErrorCodeDescription.PowerSwitchFailure;
                        break;
                    case SystemData.CPErrorCode.ReaderFailure:
                        req.Info = SystemData.CPErrorCodeDescription.ReaderFailure;
                        break;
                    case SystemData.CPErrorCode.ResetFailure:
                        req.Info = SystemData.CPErrorCodeDescription.ResetFailure;
                        break;
                    case SystemData.CPErrorCode.UnderVoltage:
                        req.Info = SystemData.CPErrorCodeDescription.UnderVoltage;
                        break;
                    case SystemData.CPErrorCode.WeakSignal:
                        req.Info = SystemData.CPErrorCodeDescription.WeakSignal;
                        break;
                    case SystemData.CPErrorCode.VendorSpecific:
                        req.Info = SystemData.CPErrorCodeDescription.VendorSpecific;
                        break;
                }

                var newBreakdownError = new CPBreakdownError
                {
                    CPDetailsId = cpDetails.Id,
                    ConnectorId = req.ConnectorId,
                    ErrorCode = req.ErrorCode,
                    ErrorDescription = req.Info,
                    Status = SystemData.BreakdownErrorStatus.InProgress,
                    Severity = SystemData.BreakdownErrorSeverity.Unknown,
                    TimeStamp = utcDatetime,
                };

                breakdownError = await _cpBreakdownErrorRepository.AddAndSaveChangesAsync(newBreakdownError);

                var newBreakdownDuration = new CPBreakdownDurationDetails
                {
                    CPBreakdownErrorId = newBreakdownError.Id,
                    StartTime = utcDatetime,
                };

                await _cpBreakdownDurationDetailsRepository.AddAndSaveChangesAsync(newBreakdownDuration);

                if (string.IsNullOrEmpty(req.Info)) req.Info = req.ErrorCode;

                var cpSite = await _cpSiteDetailsRepository.FindByIdAsync(cpDetails.CPSiteDetailsId);
                var incident = new CreateIncidentsRequestModel
                {
                    Summary = $"{cpDetails.SerialNo}_{req.ErrorCode}",
                    Description = req.Info,
                    SiteId = cpSite.SmartSDSiteId == 0 ? null : cpSite.SmartSDSiteId
                };
                var response = await _smartSDServiceHelper.CreateIncidentRecord(incident);

                if (breakdownError == null)
                    throw new Exception("No Breakdown Error found");
                breakdownError.IncidentId = response.IncidentId;
                breakdownError.IncidentNo = response.IncidentNo;
                breakdownError.Status = response.StatusCode;
                await _cpBreakdownErrorRepository.UpdateAndSaveChangesAsync(breakdownError);
            }
        }

        public async Task<CPMonitoringDisplayModel> GetCPMonitoringChartDetailsAsync(int id)
        {
            CPMonitoringDisplayModel displayModel = new CPMonitoringDisplayModel();

            await UpdateCPStatusByHeartBeat();

            var cpQuery = (from cpsd in _cpSiteDetailsRepository.ToQueryable()
                           where cpsd.Id == id
                           select new CPMonitoringDetails()
                           {
                               SiteName = cpsd.SiteName,
                               Longitude = cpsd.Longitude,
                               Latitude = cpsd.Latitude,
                               Status = cpsd.Status,
                               Address = cpsd.Address,
                               MaintenanceProgram = cpsd.MaintenanceProgram,
                               TotalOrders = 0,
                               CompletedOrders = 0,
                               IncompleteOrders = 0,
                            }).FirstOrDefault();

            if (cpQuery == null)
            {
                displayModel.Success = false;
                displayModel.StatusCode = SystemData.StatusCode.NotFound;
                return displayModel;
            }

            cpQuery.ChargingPointCurrentStatusList = (from cp in _cpDetailsRepository.ToQueryable()
                                                    where cp.CPSiteDetailsId == id
                                                    select new ChargingPointCurrentStatus()
                                                    {
                                                        Id = cp.Id,
                                                        Name = cp.Name,
                                                        Status = cp.Status,
                                                        IsOnline = cp.IsOnline,
                                                        TotalConnectors = _cpConnectorRepository.ToQueryable().Where(x => x.CPDetailsId == cp.Id).Count()
                                                    }).ToList();

            var cpIds = cpQuery.ChargingPointCurrentStatusList.Select(x => x.Id).ToList();
            var cpTransactionQuery = _cpTransactionRepository.ToQueryable().Where(x => cpIds.Contains(x.CPDetailsId)).ToList();
            if (cpTransactionQuery.Count > 0)
            {
                cpQuery.TotalOrders = cpTransactionQuery.Count;
                cpQuery.CompletedOrders = cpTransactionQuery.Count(x => x.Status == SystemData.CPTransaction.Complete);
                cpQuery.IncompleteOrders = cpTransactionQuery.Count(x => x.Status != SystemData.CPTransaction.Complete);
            }

            displayModel.CPMonitoringDetails = cpQuery;

            TimeZoneInfo my = TimeZoneInfo.FindSystemTimeZoneById("Singapore");
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, my);
            var hours = Enumerable.Range(00, localTime.Hour + 1).Select(i => new DateTime(localTime.Year, localTime.Month, localTime.Day, i, 0, 0)).ToList();
            
            var utcStartToday = TimeZoneInfo.ConvertTimeToUtc(hours[0], my);            
            var todateTransactions = cpTransactionQuery.Where(x => x.EndTime >= utcStartToday && x.EndTime <= DateTime.UtcNow);

            var utcStartYesterday = TimeZoneInfo.ConvertTimeToUtc(hours[0].AddDays(-1), my);
            var utcEndYesterday = TimeZoneInfo.ConvertTimeToUtc(hours[0].AddHours(23).AddDays(-1), my);
            var yesterdayTransactions = cpTransactionQuery.Where(x => x.EndTime >= utcStartYesterday && x.EndTime <= utcEndYesterday);

            var totalConsumptionKwh = todateTransactions.Where(x => x.Status == SystemData.CPTransaction.Complete).Sum(x => x.MeterStopValue - x.MeterStartValue);
            var generatedRevenue = todateTransactions.Where(x => x.Status == SystemData.CPTransaction.Complete).Sum(x => x.TotalAmount);
            
            displayModel.PowerUtilizationDetails.TotalConsumptionKwh = totalConsumptionKwh;
            displayModel.PowerUtilizationDetails.GeneratedRevenue = generatedRevenue;
            displayModel.PowerUtilizationDetails.EstimatedSavings = 0; //TODO
          
            const decimal kiloWattPerFuel = 8.9M;
            const decimal co2KgPerFuel = 2.3M;

            var fuelReplacedLitre = totalConsumptionKwh / kiloWattPerFuel;

            var chargingSessionIncrementPercentage = yesterdayTransactions.Count() == 0 ? todateTransactions.Count() : ((todateTransactions.Count() - yesterdayTransactions.Count()) / yesterdayTransactions.Count());

            var successIncrementPercentage = yesterdayTransactions.Count(x => x.Status == SystemData.CPTransaction.Complete) == 0 ? todateTransactions.Count(x => x.Status == SystemData.CPTransaction.Complete) : ((todateTransactions.Count(x => x.Status == SystemData.CPTransaction.Complete) - yesterdayTransactions.Count(x => x.Status == SystemData.CPTransaction.Complete)) / yesterdayTransactions.Count(x => x.Status == SystemData.CPTransaction.Complete));

            var failedIncrementPercentage = yesterdayTransactions.Count(x => x.Status != SystemData.CPTransaction.Complete) == 0 ? todateTransactions.Count(x => x.Status != SystemData.CPTransaction.Complete) : ((todateTransactions.Count(x => x.Status != SystemData.CPTransaction.Complete) - yesterdayTransactions.Count(x => x.Status != SystemData.CPTransaction.Complete)) / yesterdayTransactions.Count(x => x.Status != SystemData.CPTransaction.Complete));

            displayModel.ToDateChargingDetails = new ToDateChargingDetails()
            {
                CO2ReductionKg = fuelReplacedLitre * co2KgPerFuel,
                FuelReplacedLitre = fuelReplacedLitre,
                ChargingSession = todateTransactions.Count(),
                ChargingSessionPercentage = chargingSessionIncrementPercentage,
                SuccessTransactionCount = todateTransactions.Count(x => x.Status == SystemData.CPTransaction.Complete),
                SuccessTransactionPercentage = successIncrementPercentage,
                FailedTransactionCount = todateTransactions.Count(x => x.Status != SystemData.CPTransaction.Complete),
                FailedTransactionPercentage = failedIncrementPercentage,
            };

            decimal availablePower = 0;
            var cpDetailsQuery = _cpDetailsRepository.ToQueryable().Where(x => x.CPSiteDetailsId == id).ToList();
            foreach (var cp in cpDetailsQuery)
            {
                //switch (cpc.ProductType)
                //{
                //    case SystemData.ProductType.Ac:
                //        availablePower += 22;
                //        break;
                //    case SystemData.ProductType.Dc:
                //        availablePower += 50;
                //        break;
                //    case SystemData.ProductType.AcDc:
                //        availablePower += 22;
                //        break;
                //}

                switch (cp.Status)
                {
                    case SystemData.CPStatus.Available:
                        cp.Status = SystemData.CPChartDisplayStatus.NotInUse;
                        break;
                    case SystemData.CPStatus.Preparing:
                        cp.Status = SystemData.CPChartDisplayStatus.Unavailable;
                        break;
                    case SystemData.CPStatus.Charging:
                        cp.Status = SystemData.CPChartDisplayStatus.InUse;
                        break;
                    case SystemData.CPStatus.SuspendedEVSE:
                        cp.Status = SystemData.CPChartDisplayStatus.Unavailable;
                        break;
                    case SystemData.CPStatus.SuspendedEV:
                        cp.Status = SystemData.CPChartDisplayStatus.Unavailable;
                        break;
                    case SystemData.CPStatus.Finishing:
                        cp.Status = SystemData.CPChartDisplayStatus.InUse;
                        break;
                    case SystemData.CPStatus.Reserved:
                        cp.Status = SystemData.CPChartDisplayStatus.Unavailable;
                        break;
                    case SystemData.CPStatus.Unavailable:
                        cp.Status = SystemData.CPChartDisplayStatus.Unavailable;
                        break;
                    case SystemData.CPStatus.Faulted:
                        cp.Status = SystemData.CPChartDisplayStatus.Maintenance;
                        break;
                    default:
                        break;
                }
            }

            foreach (var cpc in cpDetailsQuery.GroupBy(x => x.Status).ToList())
            {
                CPConnectorStatus _tempStatus = new CPConnectorStatus()
                {
                    Status = cpc.Key,
                    Count = cpDetailsQuery.Count(x => x.Status == cpc.Key),
                    Percentage = Decimal.Divide(cpDetailsQuery.Count(x => x.Status == cpc.Key), cpDetailsQuery.Count())
                };

                switch (cpc.Key)
                {
                    case SystemData.CPChartDisplayStatus.InUse:
                        _tempStatus.ColorCode = SystemData.CPStatusColorCode.InUse;
                        break;
                    case SystemData.CPChartDisplayStatus.NotInUse:
                        _tempStatus.ColorCode = SystemData.CPStatusColorCode.NotInUse;
                        break;
                    case SystemData.CPChartDisplayStatus.Maintenance:
                        _tempStatus.ColorCode = SystemData.CPStatusColorCode.Maintenance;
                        break;
                    case SystemData.CPChartDisplayStatus.Unavailable:
                        _tempStatus.ColorCode = SystemData.CPStatusColorCode.Unavailable;
                        break;
                    default:
                        break;
                }
                
                displayModel.ToDateChargingDetails.CPConnectorStatus.Add(_tempStatus);
                continue;
            }

            var breakdownConnector = _cpBreakdownErrorService.GetBreakdownDurationByCPId(id);

            foreach (var h in hours)
            {
                decimal excessPower = availablePower;
                var utcTime = TimeZoneInfo.ConvertTimeToUtc(h, my);
                var currentTimeTransactions = cpTransactionQuery.Where(x => x.EndTime.Date == utcTime.Date && x.EndTime.Hour == utcTime.Hour);
                if (breakdownConnector != null)
                {
                    //foreach (var t in breakdownConnector)
                    //{
                    //    if (((t.Status == SystemData.BreakdownErrorStatus.InProgress && h >= t.StartTime) || 
                    //        (t.Status == SystemData.BreakdownErrorStatus.Fixed && h >= t.StartTime && h >= t.EndTime)))
                    //    {
                    //        switch (t.ProductType)
                    //        {
                    //            case SystemData.ProductType.Ac:
                    //                excessPower = availablePower - 22;
                    //                break;
                    //            case SystemData.ProductType.Dc:
                    //                excessPower = availablePower - 50;
                    //                break;
                    //            case SystemData.ProductType.AcDc:
                    //                excessPower = availablePower - 22;
                    //                break;
                    //        }
                    //    }
                    //}
                }

                PowerUtilizationChartDetails _tempChartDetails = new PowerUtilizationChartDetails()
                {
                    Time = h.ToString("hhtt"),
                    KiloWattPerHour = currentTimeTransactions.Sum(x => x.MeterStopValue - x.MeterStartValue),
                    ExcessPowerPerHour = excessPower - currentTimeTransactions.Sum(x => x.MeterStopValue - x.MeterStartValue)
                };
                displayModel.PowerUtilizationDetails.PowerUtilizationChartDetails.Add(_tempChartDetails);
            }

            displayModel.BreakdownErrorDetails = _cpBreakdownErrorService.GetErrorListByCPId(id);

            displayModel.Success = true;
            displayModel.StatusCode = SystemData.StatusCode.Success;

            return displayModel;
        }

        public async Task<DashboardDisplayModel> GetDashboardDetailsAsync(int userId, int pastDays)
        {
            var siteListing = (from site in _cpSiteDetailsRepository.ToQueryable()
                               where site.UserAccountId == userId
                               let cpIds = _cpDetailsRepository.ToQueryable()!.Where(x => x.CPSiteDetailsId == site.Id).Select(x => x.Id).ToList()
                               select new SiteDetails
                               {
                                   SiteId = site.Id,
                                   SiteName = site.SiteName,
                                   SiteLocation = $"{site.City}, {site.State}",
                                   StationType = "Commercial",
                                   Status = site.OperationalStatus,
                                   TotalCharger = _cpDetailsRepository.ToQueryable().Where(x => x.CPSiteDetailsId == site.Id).Count(),
                                   Income = Math.Round(_cpTransactionRepository.ToQueryable().Where(x => cpIds.Contains(x.CPDetailsId)).Select(x => x.TotalAmount).Sum(), 2),
                               }).ToList();

            await UpdateCPStatusByHeartBeat();

            var chargerListing = (from cpc in _cpConnectorRepository.ToQueryable()
                                 join cp in _cpDetailsRepository.ToQueryable()!
                                 on cpc.CPDetailsId equals cp.Id
                                 join site in _cpSiteDetailsRepository.ToQueryable()!
                                 on cp.CPSiteDetailsId equals site.Id
                                 join pt in _productTypeRepository.ToQueryable()
                                 on cpc.ProductTypeId equals pt.Id
                                 where site.UserAccountId == userId
                                 select new ChargerDetails()
                                 {
                                    CPConnectorId = cpc.Id,
                                    CPConnectorName = cpc.Name,
                                    ProductType = pt.Name,
                                    ChargingPointSerialNumber = cp.SerialNo,
                                    Status = cpc.Status,
                                    CPDetailsId = cp.Id,
                                    ConnectorId = cpc.ConnectorId,
                                    SiteName = site.SiteName,
                                    PowerOutput = cpc.PowerOutput,
                                 }).ToList();

            foreach (var charger in chargerListing)
            {
                var chargerTransaction = _cpTransactionRepository.ToQueryable()!.Where(x => x.CPDetailsId == charger.CPDetailsId && x.ConnectorId == charger.ConnectorId && x.Status == SystemData.CPTransaction.Complete).ToList();
                if (chargerTransaction.Count > 0)
                {
                    double timeSpan = 0.00;
                    foreach (var trans in chargerTransaction.Where(x => x.Status == SystemData.CPTransaction.Complete).ToList())
                    {
                        timeSpan += (trans.EndTime - trans.StartTime).TotalSeconds;
                    }
                    charger.ChargingTime = TimeSpan.FromSeconds(timeSpan);
                    charger.AverageChargeSpeed = chargerTransaction.Sum(x => x.TotalMeterValue) / Convert.ToDecimal(charger.ChargingTime.TotalHours);
                    charger.AveragePowerPercentage = charger.AverageChargeSpeed == 0 || charger.PowerOutput == 0? 0 : Math.Round((charger.AverageChargeSpeed / charger.PowerOutput) * 100);
                    charger.TotalCharging = chargerTransaction.Count; //all status is counted
                }
            }

            TimeZoneInfo my = TimeZoneInfo.FindSystemTimeZoneById("Singapore");

            var transactionListing = (from cptrans in _cpTransactionRepository.ToQueryable()                                      
                                      join cp in _cpDetailsRepository.ToQueryable()!
                                      on cptrans.CPDetailsId equals cp.Id
                                      join site in _cpSiteDetailsRepository.ToQueryable()!
                                      on cp.CPSiteDetailsId equals site.Id
                                      join cpc in _cpConnectorRepository.ToQueryable()!
                                      on cp.Id equals cpc.CPDetailsId
                                      join pt in _productTypeRepository.ToQueryable()
                                      on cpc.ProductTypeId equals pt.Id
                                      where site.UserAccountId == userId &&
                                      pastDays <= 1 ? cptrans.CreatedOn.Date == DateTime.UtcNow.AddDays(-1 * pastDays).Date : cptrans.CreatedOn.Date >= DateTime.UtcNow.AddDays(-1 * pastDays).Date
                                      select new HistoryDetails()
                                      {
                                          DateTime = TimeZoneInfo.ConvertTimeFromUtc(cptrans.CreatedOn, my),
                                          Event = "Charge the car on station",
                                          CPConnectorId = cpc.Id,
                                          CPConnectorName = cpc.Name,
                                          ProductType = pt.Name,
                                          ChargingPointSerialNumber = cp.SerialNo,
                                          SiteName = site.SiteName,
                                          Status = cptrans.Status == SystemData.CPTransaction.Started ? "In Progress" : cptrans.Status
                                      }).ToList();

            var CPDetailsId = chargerListing.Select(x => x.CPDetailsId).ToList();
            var previousMonthTransaction = Convert.ToInt32(Math.Floor(_cpTransactionRepository.ToQueryable().Where(x => CPDetailsId.Contains(x.CPDetailsId) && x.CreatedOn.Month == DateTime.UtcNow.AddMonths(-1).Month && x.CreatedOn.Year == DateTime.UtcNow.Year).Select(x => x.TotalAmount).Sum()));
            var previousMonthTotalNumberOfTransction = _cpTransactionRepository.ToQueryable().Where(x => CPDetailsId.Contains(x.CPDetailsId) && x.CreatedOn.Month == DateTime.UtcNow.AddMonths(-1).Month && x.CreatedOn.Year == DateTime.UtcNow.Year).Select(x => x.TransactionId).Count();

            CurrentMonthSummary currentMonthSummary = new CurrentMonthSummary();
            currentMonthSummary.TotalEnergy = Math.Round(_cpTransactionRepository.ToQueryable().Where(x => CPDetailsId.Contains(x.CPDetailsId) && x.CreatedOn.Month == DateTime.UtcNow.Month && x.CreatedOn.Year == DateTime.UtcNow.Year).Select(x => x.TotalMeterValue).Sum(), 2);
            currentMonthSummary.HourUtilisation =Math.Round(_cpTransactionRepository.ToQueryable().Where(x => CPDetailsId.Contains(x.CPDetailsId) && x.CreatedOn.Month == DateTime.UtcNow.Month && x.CreatedOn.Year == DateTime.UtcNow.Year).Select(x => x.TotalHoursTaken).Sum(), 2);
            currentMonthSummary.ActiveCharging = chargerListing.Where(x => x.Status == SystemData.CPStatus.Charging).Count();
            currentMonthSummary.Transaction = Math.Round(_cpTransactionRepository.ToQueryable().Where(x => CPDetailsId.Contains(x.CPDetailsId) && x.CreatedOn.Month == DateTime.UtcNow.Month && x.CreatedOn.Year == DateTime.UtcNow.Year).Select(x => x.TotalAmount).Sum(),2);
            currentMonthSummary.TotalNumberOfTransaction = _cpTransactionRepository.ToQueryable().Where(x => CPDetailsId.Contains(x.CPDetailsId) && x.CreatedOn.Month == DateTime.UtcNow.Month && x.CreatedOn.Year == DateTime.UtcNow.Year).Select(x => x.TransactionId).Count();
            currentMonthSummary.TransactionPercentage = currentMonthSummary.Transaction == 0 || previousMonthTransaction == 0 ? 0 :(currentMonthSummary.Transaction - previousMonthTransaction) / previousMonthTransaction;
            currentMonthSummary.TotalNumberOfTransactionPercentage = currentMonthSummary.TotalNumberOfTransaction == 0 || previousMonthTotalNumberOfTransction == 0 ? 0 :(currentMonthSummary.TotalNumberOfTransaction - previousMonthTotalNumberOfTransction) / previousMonthTotalNumberOfTransction;

            var response = new DashboardDisplayModel
            {
                Success = true,
                StatusCode = 200,
                SiteList = siteListing,
                ChargerList = chargerListing,
                HistoryList = transactionListing,
                CurrentMonthSummaryList = currentMonthSummary
             };

            return response;
        }

        public async Task<CPSiteDetails> GetCPSiteDetailsByIdAsync(int id)
        {
            var cpSiteDetails = await _cpSiteDetailsRepository.FindByIdAsync(id);

            return cpSiteDetails;

        }

        public async Task<CPSiteDetails> UpdateCPSiteDetailsAsync(CPSiteDetailsRequestModel model)
        {
            var updateCpSiteDetails = await _cpSiteDetailsRepository.FindByIdAsync(model.Id);
            if (updateCpSiteDetails == null) throw (new CustomValidationException(nameof(updateCpSiteDetails), SystemData.CustomValidation.Invalid));

            updateCpSiteDetails.PersonInCharge = model.PersonInCharge;
            updateCpSiteDetails.Email = model.Email;
            updateCpSiteDetails.OfficeNo = model.OfficeNo;
            updateCpSiteDetails.MobileNo = model.MobileNo;
            updateCpSiteDetails.OperationalStatus = model.OperationalStatus;
            updateCpSiteDetails.MaintenanceProgram = model.MaintenanceProgram;
            updateCpSiteDetails.Description = model.Description;
            updateCpSiteDetails.UpdatedOn = DateTime.UtcNow;

            updateCpSiteDetails = await _cpSiteDetailsRepository.UpdateAndSaveChangesAsync(updateCpSiteDetails);

            return updateCpSiteDetails;
        }

        public async Task<CPSiteDetails> DeleteCPSiteDetailsByIdAsync(int id)
        {
            var deleteCpSiteDetails = await _cpSiteDetailsRepository.FindByIdAsync(id);
            if (deleteCpSiteDetails == null) throw (new CustomValidationException(nameof(deleteCpSiteDetails), SystemData.CustomValidation.Invalid));

            var updateCpDetails = _cpDetailsRepository.ToQueryable().Where(x => x.CPSiteDetailsId == id).ToList();

            if (updateCpDetails.Count > 0)
            {
                updateCpDetails.ForEach(x => { x.Status = SystemData.CPStatus.OutOfService; });
               
                await _cpDetailsRepository.UpdateRangeAndSaveChangesAsync(updateCpDetails);

            }

            deleteCpSiteDetails.OperationalStatus = SystemData.CPOpertaionalStatus.Removed;
            deleteCpSiteDetails = await _cpSiteDetailsRepository.UpdateAndSaveChangesAsync(deleteCpSiteDetails);

            return deleteCpSiteDetails;
        }

        public CPDetailsReturnModel GetCPDetailsBySiteId(int id)
        {
            CPDetailsReturnModel cPDetailsReturnModel = new CPDetailsReturnModel();

            var cpDetails = _cpDetailsRepository.ToQueryable().Where(x => x.CPSiteDetailsId == id).ToList();

            List<CPDetailsViewModel> cpDetailsViewModelList = new List <CPDetailsViewModel>();

            foreach (var cp in cpDetails)
            {
                CPDetailsViewModel cPDetailsViewModel = new CPDetailsViewModel();
                cPDetailsViewModel.Id = cp.Id;
                cPDetailsViewModel.Charger = cp.Name;
                cPDetailsViewModel.SerialNo = cp.SerialNo;
                cPDetailsViewModel.TotalCharge = _cpTransactionRepository.ToQueryable().Where(x => x.CPDetailsId == cp.Id && x.Status == SystemData.CPTransaction.Complete).Count();
                cPDetailsViewModel.UnitUsage = _cpTransactionRepository.ToQueryable().Where(x => x.CPDetailsId == cp.Id && x.Status == SystemData.CPTransaction.Complete).Sum(x => x.TotalMeterValue);
                cPDetailsViewModel.Revenue = _cpTransactionRepository.ToQueryable().Where(x => x.CPDetailsId == cp.Id && x.Status == SystemData.CPTransaction.Complete).Sum(x => x.TotalAmount);
                cPDetailsViewModel.OperationalStatus = cp.Status;
                cpDetailsViewModelList.Add(cPDetailsViewModel);
            }

            cPDetailsReturnModel.CPDetails = cpDetailsViewModelList;
            cPDetailsReturnModel.TotalCharge = cPDetailsReturnModel.CPDetails.Sum( x => x.TotalCharge);
            cPDetailsReturnModel.TotalEVUsage = cPDetailsReturnModel.CPDetails.Sum(x => x.UnitUsage);
            cPDetailsReturnModel.Revenue = cPDetailsReturnModel.CPDetails.Sum(x => x.Revenue);
            return cPDetailsReturnModel;

        }


        #region CPHeatlhCheck

        public async Task<ResponseModelBase> BootNotification(BootNotificationModel model)
        {
            ResponseModelBase responseModelBase = new ResponseModelBase()
            {
                Success = true,
                StatusCode = SystemData.StatusCode.Success,
            };

            var updateCPDetails = _cpDetailsRepository.ToQueryable().Where(x => x.SerialNo == model.chargePointSerialNumber).FirstOrDefault();
            var updateCPConnectors = _cpConnectorRepository.ToQueryable().Where(x => x.CPDetailsId == updateCPDetails!.Id).ToList();

            if (updateCPDetails == null)
            {
                return responseModelBase = new ResponseModelBase()
                {
                    Success = false,
                    StatusCode = SystemData.StatusCode.NotFound
                };
            };

            if (updateCPDetails != null)
            {
                updateCPDetails.WebSocketId = model.webSocketId;
                updateCPDetails.Status = SystemData.CPStatus.Available;
                updateCPDetails.IsOnline = true;
                updateCPDetails = await _cpDetailsRepository.UpdateAndSaveChangesAsync(updateCPDetails);

                updateCPConnectors.ForEach(x => x.Status = SystemData.CPStatus.Available);
                await _cpConnectorRepository.UpdateRangeAndSaveChangesAsync(updateCPConnectors);
            };

            return responseModelBase;
        }

        public async Task<HeartBeatConf> UpdateCPHeartBeatAsync(string webSocketId)
        {
            HeartBeatConf responseModel = new HeartBeatConf()
            {
                Success = true,
                StatusCode = SystemData.StatusCode.Success
            };

            var updateCP = _cpDetailsRepository.ToQueryable().Where(x => x.WebSocketId == webSocketId).FirstOrDefault();
            if (updateCP == null)
            {
                //await _logger.LogInformation($"Web Socket ID {webSocketId} not found");
                //throw new Exception("CPDetails not found");

                return responseModel = new HeartBeatConf()
                {
                    Success = false,
                    StatusCode = SystemData.StatusCode.NotFound
                };
            }

            if (updateCP != null)
            {
                updateCP.HeartbeatDateTime = DateTime.UtcNow;
                await _cpDetailsRepository.UpdateAndSaveChangesAsync(updateCP);
                responseModel.currentTime = updateCP.HeartbeatDateTime;
            }

            return responseModel;   
        }

        private async Task UpdateCPStatusByHeartBeat()
        {
            var inactiveCP = _cpDetailsRepository.ToQueryable().Where(x => x.HeartbeatDateTime.AddMinutes(5) < DateTime.UtcNow).ToList();
            inactiveCP.ForEach(x => { x.Status = SystemData.CPStatus.Unavailable; x.IsOnline = false; });
            await _cpDetailsRepository.UpdateRangeAndSaveChangesAsync(inactiveCP);

            var inactiveConnector = _cpConnectorRepository.ToQueryable().Where(x => inactiveCP.Select(x => x.Id).Contains(x.CPDetailsId)).ToList();
            inactiveConnector.ForEach(x => x.Status = SystemData.CPStatus.Unavailable);
            await _cpConnectorRepository.UpdateRangeAndSaveChangesAsync(inactiveConnector);
        }

        private static double Distance(LocationCoordinates location1, LocationCoordinates location2)
        {
            const double earthRadius = 6371; // in km

            var lat1 = location1.Latitude;
            var lon1 = location1.Longitude;
            var lat2 = location2.Latitude;
            var lon2 = location2.Longitude;

            var dLat = ToRadians(lat2 - lat1);
            var dLon = ToRadians(lon2 - lon1);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var distance = earthRadius * c;

            return distance;
        }

        private static double ToRadians(decimal angle)
        {
            return Math.PI * Decimal.ToDouble(angle) / 180.0;
        }
        #endregion

        //View All Site Listing(Pop Up) 
        public ViewAllSiteListingResponseModel ViewAllSiteListing(ViewAllSiteListingRequestModel model)
        {
            var currentDate = DateTime.UtcNow;
            PagedCPSiteDetailsList returnPagedModel = new();

            var query = (from site in _cpSiteDetailsRepository.ToQueryable()
                        join cp in _cpDetailsRepository.ToQueryable()
                        on site.Id equals cp.CPSiteDetailsId
                        where site.UserAccountId == model.SearchSiteRequest.UserAccontId
                        select new CPSiteDetailsDisplayModel
                        {
                            CPId = cp.Id,
                            SiteName = site.SiteName,
                            CPName = cp.Name,
                            CPArea = site.City,
                            Status = cp.Status,
                            TotalConnector = _cpConnectorRepository.ToQueryable().Where(x => x.CPDetailsId == cp.Id).Count(),
                            IncomeOfTheDay = _cpTransactionRepository.ToQueryable().Where(x => x.EndTime.Date >= currentDate.Date && x.CPDetailsId == cp.Id).Select(x => x.TotalAmount).Sum(),
                            LoadLimit = site.LoadLimit
                        });

            if (!string.IsNullOrEmpty(model.SearchSiteRequest.SearchString))
            {
                query = query.Where(x => x.SiteName.ToLower().Contains(model.SearchSiteRequest.SearchString.ToLower()));
                if (query.Count() == 0)
                {
                    query = query.Where(x => x.CPArea.ToLower().Contains(model.SearchSiteRequest.SearchString.ToLower()));
                }
            }

            query = query.OrderByDescending(t => t.IncomeOfTheDay);

            if (!string.IsNullOrEmpty(model.SearchSiteRequest.OrderColumn))
            {
                if ((!string.IsNullOrEmpty(model.SearchSiteRequest.OrderBy) && model.SearchSiteRequest.OrderBy == "asc"))
                {
                    if (model.SearchSiteRequest.OrderColumn == "siteName")
                        query = query.OrderBy(t => t.SiteName);

                    if (model.SearchSiteRequest.OrderColumn == "cpName")
                        query = query.OrderBy(t => t.CPName);

                    if (model.SearchSiteRequest.OrderColumn == "cpArea")
                        query = query.OrderBy(t => t.CPArea);

                    if (model.SearchSiteRequest.OrderColumn == "status")
                        query = query.OrderBy(t => t.Status);

                    if (model.SearchSiteRequest.OrderColumn == "totalConnector")
                        query = query.OrderBy(t => t.TotalConnector);

                    if (model.SearchSiteRequest.OrderColumn == "incomeOfTheDay")
                        query = query.OrderBy(t => t.IncomeOfTheDay);
                }

                if ((!string.IsNullOrEmpty(model.SearchSiteRequest.OrderBy) && model.SearchSiteRequest.OrderBy == "desc"))
                {
                    if (model.SearchSiteRequest.OrderColumn == "siteName")
                        query = query.OrderByDescending(t => t.SiteName);

                    if (model.SearchSiteRequest.OrderColumn == "cpName")
                        query = query.OrderByDescending(t => t.CPName);

                    if (model.SearchSiteRequest.OrderColumn == "cpArea")
                        query = query.OrderByDescending(t => t.CPArea);

                    if (model.SearchSiteRequest.OrderColumn == "status")
                        query = query.OrderByDescending(t => t.Status);

                    if (model.SearchSiteRequest.OrderColumn == "totalConnector")
                        query = query.OrderByDescending(t => t.TotalConnector);

                    if (model.SearchSiteRequest.OrderColumn == "incomeOfTheDay")
                        query = query.OrderByDescending(t => t.IncomeOfTheDay);
                }
            }

            var pagedList = PagedList<CPSiteDetailsDisplayModel>.ToPagedList(query, model.SearchSiteRequest.PageNumber, model.SearchSiteRequest.PageSize);

            returnPagedModel.CPSiteDetailsList = pagedList.ToList();
            returnPagedModel.CurrentPage = pagedList.CurrentPage;
            returnPagedModel.TotalPages = pagedList.TotalPages;
            returnPagedModel.PageSize = pagedList.PageSize;
            returnPagedModel.TotalCount = pagedList.TotalCount;
            returnPagedModel.HasPrevious = pagedList.HasPrevious;
            returnPagedModel.HasNext = pagedList.HasNext;

            var response = new ViewAllSiteListingResponseModel
            {
                Success = true,
                StatusCode = 200,
                CPSiteDetailsList = returnPagedModel
            };

            return response;
        }

        public CPDashboardResponseModel DisplayChargePointDashboard(int cpId)
        {
            TimeZoneInfo my = TimeZoneInfo.FindSystemTimeZoneById("Singapore");
            var currentTimeUtc = DateTime.UtcNow;

            var cpDetails = (from cp in _cpDetailsRepository.ToQueryable()
                            join site in _cpSiteDetailsRepository.ToQueryable()
                            on cp.CPSiteDetailsId equals site.Id
                            where cp.Id == cpId
                            select new CPDetailsViewModel
                            {
                                Id = cp.Id,
                                Charger = cp.Name,
                                SiteName = site.SiteName,
                                SerialNo = cp.SerialNo,
                                OperationalStatus = cp.Status,
                                CreatedOn = cp.CreatedOn.ToString("MM/dd/yyyy hh:MM"),
                                //UpdatedOn = cp.UpdatedOn != null ? site.UpdatedOn.Value.ToString("MM/dd/yyyy hh:MM") : null,
                                //PlanAssigned = _pricingPlanRepository.ToQueryable().Where(x => x.Id == cp.PricingPlanId),
                                Longitude = site.Longitude,
                                Latitude = site.Latitude,
                            }).FirstOrDefault();

            if (cpDetails == null)
            {
                var response = new CPDashboardResponseModel
                {
                    Success = false,
                    StatusCode = SystemData.StatusCode.NotFound
                };
                return response;
            }

            var start = new TimeSpan(0, 0, 0);
            var end = new TimeSpan(24, 0, 0);
            var current = start;
            List<DateTime> datetimeList = new List<DateTime>();
            var startTime = DateTime.UtcNow.AddDays(-1);
            while(current < end)
            {
                datetimeList.Add(startTime + current);
                current = current.Add(new TimeSpan(1, 0, 0));
            }

            var kwhConsumedByTime = new List<decimal>();
            var localTimeList = new List<string>();

            foreach(var time in datetimeList)
            {
                var kwhConsumedSum = _cpTransactionRepository.ToQueryable().Where(x => x.CPDetailsId == cpId && x.EndTime.Hour == time.Hour && x.EndTime.Date == time.Date).Sum(x => x.TotalMeterValue);
                kwhConsumedByTime.Add(kwhConsumedSum);

                //change time from utc to malaysia time
                var localTime = TimeZoneInfo.ConvertTimeFromUtc(time, my);
                localTimeList.Add(localTime.ToString("hh:00 tt"));
            }

            KwhConsumptionGraphDetails kwhConsumptionGraphDetails = new KwhConsumptionGraphDetails
            {
                Time = localTimeList,
                KwhConsumed = kwhConsumedByTime
            };

            var transactionListing = (from trans in _cpTransactionRepository.ToQueryable()
                                     where trans.CPDetailsId == cpId 
                                     && trans.Status != SystemData.CPTransaction.Started
                                     && trans.EndTime < currentTimeUtc
                                     select new CPTransactionHistoryDetails
                                     {
                                         TransactionId = trans.Id,
                                         TransactionDate = TimeZoneInfo.ConvertTimeFromUtc(trans.CreatedOn, my),
                                         TimeIn = TimeZoneInfo.ConvertTimeFromUtc(trans.StartTime, my).ToString("hh:MM tt"),
                                         TimeOut = TimeZoneInfo.ConvertTimeFromUtc(trans.EndTime, my).ToString("hh:MM tt"),
                                         KwhConsumed = trans.TotalMeterValue,
                                         Amount = trans.TotalAmount,
                                         PaymentStatus = trans.Status == SystemData.CPTransaction.Complete ? "Success": "Fail"
                                     }).ToList();

            var breakdownErrorDetails = _cpBreakdownErrorService.GetErrorListByCPId(cpId);

            CPDashboardResponseModel responseModel = new CPDashboardResponseModel
            {
                Success = true,
                StatusCode = 200,
                GraphDetails = kwhConsumptionGraphDetails,
                TotalKwhConsumed = transactionListing.Select(x => x.KwhConsumed).Sum(),
                TotalRevenue = transactionListing.Select(x=>x.Amount).Sum(),
                TransactionHistoryDetails = transactionListing,
                BreakdownErrorDetails = breakdownErrorDetails,
                CPDetails = cpDetails
            };

            return responseModel;
        }

        public ConnectorListResponseModel GetConnectorDetails(int id)
        {
            var connectors = (from ct in _cpConnectorRepository.ToQueryable()
                             join pt in _productTypeRepository.ToQueryable()
                             on ct.ProductTypeId equals pt.Id
                             where ct.CPDetailsId == id && ct.IsActive == true
                             select new ConnectorDetails
                             {
                                 Id = ct.Id,
                                 ConnectorName = ct.Name,
                                 ProductType = _productTypeRepository.ToQueryable().Where(x=> x.Id == ct.ProductTypeId).Select(x => x.Name).FirstOrDefault(),
                                 PowerOutput = ct.PowerOutput,
                                 Status = ct.Status,
                                 //PricingPlanId = ct.PricingPlanId != null ? ct.PricingPlanId.Value : 0,
                                 IsLock = false
                             }).ToList();

            if(connectors == null)
            {
                var response = new ConnectorListResponseModel
                {
                    Success = false,
                    StatusCode = SystemData.StatusCode.NotFound
                };
                return response;
            }

            var responseModel = new ConnectorListResponseModel
            {
                Success = true,
                StatusCode = 200,
                ConnectorDetails = connectors
            };
            return responseModel;
        }

        public UpdateCPConnectorResponseModel UpdateCPConnector(UpdateCPConnectorRequestModel request)
        {
            var connector = _cpConnectorRepository.ToQueryable().Where(x => x.Id == request.Id).FirstOrDefault();
            if(connector == null)
            {
                var response = new UpdateCPConnectorResponseModel
                {
                    Success = false,
                    StatusCode = SystemData.StatusCode.NotFound
                };
                return response;
            }

            if(connector.Status == SystemData.CPConnectorStatus.InUse || connector.IsLock)
            {
                var response = new UpdateCPConnectorResponseModel
                {
                    Success = false,
                    StatusCode = SystemData.StatusCode.Invalid
                };
                return response;
            }

            connector.Name = request.Name;
            connector.ProductTypeId = request.ProductTypeId;
            connector.PowerOutput = request.PowerOutput;
            connector.ModifiedOn = DateTime.UtcNow;
            _cpConnectorRepository.UpdateAndSaveChangesAsync(connector);

            var responseModel = new UpdateCPConnectorResponseModel
            {
                Success = true,
                StatusCode = 200,
                ConnectorDetails = connector
            };
            return responseModel;
        }

        public async Task<ResponseModelBase> DeleteCPConnector(int id)
        {
            var connector = await _cpConnectorRepository.FindByIdAsync(id);
            if (connector == null)
            {
                var response = new ResponseModelBase
                {
                    Success = false,
                    StatusCode = SystemData.StatusCode.NotFound
                };
                return response;
            }

            if (connector.Status == SystemData.CPConnectorStatus.InUse || connector.IsLock)
            {
                var response = new UpdateCPConnectorResponseModel
                {
                    Success = false,
                    StatusCode = SystemData.StatusCode.Invalid
                };
                return response;
            }

            connector.IsActive = false;
            connector.ModifiedOn = DateTime.UtcNow;
            await _cpConnectorRepository.UpdateAndSaveChangesAsync(connector);

            var responseModel = new ResponseModelBase
            {
                Success = true,
                StatusCode = 200
            };
            return responseModel;
        }

        public async Task<UpdateCPDetailsResponseModel> UpdateCPDetailsAsync(UpdateCPDetailsRequestModel request)
        {
            var cp = await _cpDetailsRepository.FindByIdAsync(request.CPId);
            if (cp == null)
            {
                UpdateCPDetailsResponseModel response = new UpdateCPDetailsResponseModel
                {
                    Success = false,
                    StatusCode = SystemData.StatusCode.NotFound
                };
                return response;
            }

            var site = await _cpSiteDetailsRepository.FindByIdAsync(cp.CPSiteDetailsId);
            if (site.OperationalStatus != SystemData.CPOpertaionalStatus.Removed)
            {
                UpdateCPDetailsResponseModel response = new UpdateCPDetailsResponseModel
                {
                    Success = false,
                    StatusCode = SystemData.StatusCode.Invalid
                };
                return response;
            }

            cp.Name = request.Name;
            //cp.SerialNo = request.SerialNo;
            await _cpDetailsRepository.UpdateAndSaveChangesAsync(cp);

            UpdateCPDetailsResponseModel responseModel = new UpdateCPDetailsResponseModel
            {
                Success = true,
                StatusCode = 200,
                CPDetails = cp
            };
            return responseModel;
        }

        public async Task<ResponseModelBase> DeleteCPDetailsAsync(int id)
        {
            var cp = await _cpDetailsRepository.FindByIdAsync(id);
            if(cp == null)
            {
                ResponseModelBase response = new ResponseModelBase
                {
                    Success = false,
                    StatusCode = SystemData.StatusCode.NotFound
                };
                return response;
            }

            var site = await _cpSiteDetailsRepository.FindByIdAsync(cp.CPSiteDetailsId);
            if (site.OperationalStatus != SystemData.CPOpertaionalStatus.Removed)
            {
                ResponseModelBase response = new ResponseModelBase
                {
                    Success = false,
                    StatusCode = SystemData.StatusCode.Invalid
                };
                return response;
            }

            cp.Status = SystemData.CPStatus.Unavailable;
            cp.IsActive = false;
            await _cpDetailsRepository.UpdateAndSaveChangesAsync(cp);

            ResponseModelBase responseModel = new ResponseModelBase
            {
                Success = true,
                StatusCode = 200
            };
            return responseModel;
        }

        public async Task<ResponseModelBase> LockUnlockCPConnectorAsync(LockUnlockRequestModel request)
        {
            var connectors = _cpConnectorRepository.ToQueryable().Where(x => request.Ids.Contains(x.Id)).ToList();
            if(request.Lock)
            {
                connectors.ForEach(x => x.IsLock = true);
            }
            else
            {
                connectors.ForEach(x => x.IsLock = false);
            }

            await _cpConnectorRepository.UpdateRangeAndSaveChangesAsync(connectors);
            ResponseModelBase responseModel = new ResponseModelBase
            {
                Success = true,
                StatusCode = 200
            };
            return responseModel;
        }

        public async Task<CPDetails> UpdateLocalListVersion(GetLocalListVersionConf conf, string webSocketId)
        {
            var cp = _cpDetailsRepository.ToQueryable().Where(x => x.WebSocketId == webSocketId).FirstOrDefault();
            //TODO
            //cp.LocalListVersion = conf.listVersion;
            await _cpDetailsRepository.UpdateAndSaveChangesAsync(cp);
            return cp;
        }


        public async Task<ResponseModelBase> FirmwareStatusNotification (FirmwareStatusNotificationModel model)
        {
            ResponseModelBase responseModel = new ResponseModelBase
            {
                Success = true,
                StatusCode = 200
            };
            var updateCPDetails = _cpDetailsRepository.ToQueryable().Where(x => x.WebSocketId == model.websocketId).FirstOrDefault();
            var updateCPConnectors = _cpConnectorRepository.ToQueryable().Where(x => x.CPDetailsId == updateCPDetails!.Id).ToList();
           
            if(updateCPDetails!= null && updateCPConnectors != null)
            {
                if (model.status == SystemData.FirmwareStatus.Installed)
                {
                    updateCPDetails.Status = SystemData.CPStatus.Available;
                    updateCPConnectors.ForEach(x => x.Status = SystemData.CPStatus.Available);
                    await _cpDetailsRepository.UpdateAndSaveChangesAsync(updateCPDetails);
                    await _cpConnectorRepository.UpdateRangeAndSaveChangesAsync(updateCPConnectors);
                }
                else
                {
                    updateCPDetails.Status = SystemData.CPStatus.Unavailable;
                    updateCPConnectors.ForEach(x => x.Status = SystemData.CPStatus.Unavailable);
                    await _cpDetailsRepository.UpdateAndSaveChangesAsync(updateCPDetails);
                    await _cpConnectorRepository.UpdateRangeAndSaveChangesAsync(updateCPConnectors);
                }
            }

            return responseModel;
        }
    }
}
