using AutoMapper;
using HB.Database.DbModels;
using HB.Database.Repositories;
using HB.Model;
using HB.SmartSD.Integrator;
using HB.Utilities;
using Strateq.Core.Model;
using Strateq.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Service
{
    public class OnBoardingService : IOnBoardingService
    {
        #region Fields

        private readonly ICPSiteDetailsRepository _cpSiteDetailsRepository;
        private readonly ICPDetailsRepository _cpDetailsRepository;
        private readonly ICPConnectorRepository _cpConnectorRepository;
        private readonly IMapper _mapper;
        private readonly ISystemLogService _logger;
        private readonly ServiceHelper _smartSDServiceHelper;

        #endregion

        #region Ctor
        public OnBoardingService(ICPSiteDetailsRepository cpSiteDetailsRepository,
            ICPDetailsRepository cpDetailsRepository,
            ICPConnectorRepository cpConnectorRepository,
            IMapper mapper,
            ISystemLogService logger)
        {
            _cpSiteDetailsRepository = cpSiteDetailsRepository;
            _cpDetailsRepository = cpDetailsRepository;
            _cpConnectorRepository = cpConnectorRepository;
            _mapper = mapper;
            _logger = logger;
            _smartSDServiceHelper = new ServiceHelper(logger);
        }

        #endregion

        #region Methods

        public CPSiteDisplayListModel GetAllCPSiteListingByUserAccountId(int id)
        {
            CPSiteDisplayListModel returnModel = new CPSiteDisplayListModel()
            {
                Success = true, 
                StatusCode = SystemData.StatusCode.Success
            };

            var cpSiteQuery = from cps in _cpSiteDetailsRepository.ToQueryable()
                              where cps.UserAccountId == id
                              select new CPSiteDetailsModel()
                              {
                                  Id = cps.Id,
                                  PersonInCharge = cps.PersonInCharge,
                                  SiteName = cps.SiteName,
                                  Email = cps.Email,
                                  OfficeNo = cps.OfficeNo,
                                  MobileNo = cps.MobileNo,
                                  Longitude = cps.Longitude,
                                  Latitude = cps.Latitude,
                                  Address = cps.Address,
                                  City = cps.City,
                                  State = cps.State,
                                  Country = cps.Country,
                                  MaintenanceProgram = cps.MaintenanceProgram,
                                  Description = cps.Description                                  
                              };

            if (cpSiteQuery.Count() == 0)
            {
                returnModel.Success = false;
                returnModel.StatusCode = SystemData.StatusCode.NotFound;
            }
            returnModel.CPSiteList = cpSiteQuery.ToList();
            return returnModel;
        }

        public async Task<NewSiteOnBoardingResponseModel> CreateNewSiteOnBoardingAsync(OnBoardingNewSiteRequestModel model)
        {
            CPSiteDetails newCPSiteDetails = new CPSiteDetails();
            _mapper.Map(model.SiteDetails, newCPSiteDetails);
            newCPSiteDetails.UserAccountId = model.UserAccountId;
            newCPSiteDetails.Status = SystemData.CPRegistrationStatus.Accepted;
            newCPSiteDetails.OperationalStatus = SystemData.CPOpertaionalStatus.Open;

            CreateSiteRequestModel newSite = new CreateSiteRequestModel()
            {
                SiteName = newCPSiteDetails.SiteName,
                TradingName = "Strateq Group Sdn Bhd",
                Address = newCPSiteDetails.Address,
                ContactName = newCPSiteDetails.PersonInCharge,
                ContactPhone = newCPSiteDetails.MobileNo,
                Latitude = newCPSiteDetails.Latitude.ToString(),
                Longitude = newCPSiteDetails.Longitude.ToString()
            };
            var response = await _smartSDServiceHelper.CreateSiteRecord(newSite);

            newCPSiteDetails.SmartSDSiteId = response.SiteId;
            await _cpSiteDetailsRepository.AddAndSaveChangesAsync(newCPSiteDetails);
            NewSiteOnBoardingResponseModel returnModel = new NewSiteOnBoardingResponseModel()
            {
                SiteDetails = model.SiteDetails,
                Success = true,
                StatusCode = SystemData.StatusCode.Success
            };
            return returnModel;
        }

        public ValidateCPNameResponseModel ValidateCPNameAndSerialNo(ValidateCPNameRequestModel request)
        {
            ValidateCPNameResponseModel returnModel = new ValidateCPNameResponseModel()
            {
                Success = true,
                StatusCode = SystemData.StatusCode.Success
            };

            var cpDetails = _cpDetailsRepository.ToQueryable().Where(x => x.CPSiteDetailsId == request.CPSiteDetailsId);
            var existName = cpDetails.Any(x => x.Name == request.Name);
            var existSerialNo = cpDetails.Any(x => x.SerialNo == request.SerialNo);

            if (existName || existSerialNo)
            {
                returnModel = new ValidateCPNameResponseModel()
                {
                    Success = false,
                    StatusCode = SystemData.StatusCode.Exist,
                    IsName = existName,
                    IsSerialNo = existSerialNo
                };
            }

            return returnModel;
        }

        public async Task<NewCPOnBoardingResponseModel> CreateNewCPOnBoardingAsync(OnBoardingNewCPRequestModel model)
        {
            NewCPOnBoardingResponseModel returnModel = new NewCPOnBoardingResponseModel();

            var cpDetails = _cpDetailsRepository.ToQueryable().Where(x => x.CPSiteDetailsId == model.CPDetails.CPSiteDetailsId);
            var exist = cpDetails.Any(x => x.Name == model.CPDetails.Name);

            if (exist)
            {
                returnModel = new NewCPOnBoardingResponseModel()
                {
                    CPDetails = model.CPDetails,
                    CPConnectorList = model.CPConnectorList,
                    Success = false,
                    StatusCode = SystemData.StatusCode.Exist
                };

                return returnModel;
            }

            CPDetails newCPDetails = new CPDetails()
            {
                CPSiteDetailsId = model.CPDetails.CPSiteDetailsId,
                Name = model.CPDetails.Name,
                SerialNo = model.CPDetails.SerialNo,
                Status = SystemData.CPStatus.Unavailable
            };
            newCPDetails = await _cpDetailsRepository.AddAndSaveChangesAsync(newCPDetails);


            for (var i = 0; i < model.CPConnectorList.Count; i++)
            {
                CPConnector newCPConnector = new CPConnector()
                {
                    CPDetailsId = newCPDetails.Id,
                    Name = model.CPConnectorList[i].Name,
                    ProductTypeId = model.CPConnectorList[i].ProductTypeId,
                    PowerOutput = model.CPConnectorList[i].PowerOutput,
                    ConnectorId = i + 1
                };
                await _cpConnectorRepository.AddAndSaveChangesAsync(newCPConnector);
            }

            returnModel = new NewCPOnBoardingResponseModel()
            {
                CPDetails = model.CPDetails,
                CPConnectorList = model.CPConnectorList,
                Success = true,
                StatusCode = SystemData.StatusCode.Success
            };

            return returnModel;
        }

        #endregion
    }
}
