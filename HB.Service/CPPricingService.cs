using AutoMapper;
using HB.Database.DbModels;
using HB.Database.Repositories;
using HB.Model;
using HB.SmartSD.Integrator;
using HB.Utilities;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Strateq.Core.API.Exceptions;
using Microsoft.AspNetCore.Http;
using Strateq.Core.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Text;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using static Strateq.Core.Utilities.SystemDataCore;
using Strateq.Core.Model;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace HB.Service
{
    public class CPPricingService : ICPPricingService
    {
        #region Fields
        private readonly IPriceVariesRepository _priceVariesRepository;
        private readonly IProductTypeRepository _productTypeRepository;
        private readonly IPricingPlanRepository _pricingPlanRepository;
        private readonly IPricingPlanTypeRepository _pricingPlanTypeRepository;
        private readonly ICPConnectorRepository _cpConnectorRepository;
        private readonly ICPDetailsRepository _cpDetailsRepository;
        private readonly ICPSiteDetailsRepository _cpSiteDetailsRepository;
        private readonly IUnitRepository _unitRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region Ctor
        public CPPricingService(IPriceVariesRepository priceVariesRepository,
            IProductTypeRepository productTypeRepository,
            IPricingPlanRepository pricingPlanRepository,
            IPricingPlanTypeRepository pricingPlanTypeRepository,
            ICPConnectorRepository cPConnectorRepository,
            ICPDetailsRepository cPDetailsRepository,
            ICPSiteDetailsRepository cpSiteDetailsRepository,
            IUnitRepository unitRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _priceVariesRepository = priceVariesRepository;
            _productTypeRepository = productTypeRepository;
            _pricingPlanRepository = pricingPlanRepository;
            _pricingPlanTypeRepository = pricingPlanTypeRepository;
            _cpConnectorRepository = cPConnectorRepository;
            _cpDetailsRepository = cPDetailsRepository;
            _cpSiteDetailsRepository = cpSiteDetailsRepository;
            _unitRepository = unitRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region Methods

        public PagedCPPricingPlanList GetChargePointPricingPaginationListView(SearchCPPricingPlanRequestModel request)
        {
            PagedCPPricingPlanList returnPagedModel = new();

            TimeZoneInfo my = TimeZoneInfo.FindSystemTimeZoneById("Singapore");
            var pricingPlanQuery = (from cd in _cpDetailsRepository.ToQueryable()
                                   join pp in _pricingPlanRepository.ToQueryable()
                                   on cd.PricingPlanId equals pp.Id into pp_grp
                                   from cdpp in pp_grp.DefaultIfEmpty()
                                   join cs in _cpSiteDetailsRepository.ToQueryable()
                                   on cd.CPSiteDetailsId equals cs.Id
                                   where cs.UserAccountId == request.UserAccountId
                                   orderby cd.CreatedOn descending
                                   let refProdTypeIds = _cpConnectorRepository.ToQueryable().Where(x => x.CPDetailsId == cd.Id).Select(x => x.ProductTypeId).Distinct().ToList()
                                   let prodTypeNames = _productTypeRepository.ToQueryable().Where(x => refProdTypeIds.Contains(x.Id)).Select(x => x.Name).ToList()
                                   select new CPPricingPlanDisplayModel()
                                   {
                                       Id = cd.Id,
                                       CPName = cd.Name,
                                       ProductTypeIds = refProdTypeIds,
                                       ProductTypes = string.Join(", ", prodTypeNames).ToString(),
                                       CPSiteId = cs.Id,
                                       CPSiteName = cs.SiteName,
                                       CPSiteAddress = cs.Address,
                                       CPStatus = cd.Status,
                                       IsOnline = cd.IsOnline,
                                       TotalConnector = _cpConnectorRepository.ToQueryable().Where(x => x.CPDetailsId == cd.Id).Count(),
                                       PricingPlanId = cdpp.Id,
                                       PricingPlanName = cdpp.PlanName,
                                       CreatedOn = cd.AssignedOn != null ? TimeZoneInfo.ConvertTimeFromUtc(cd.AssignedOn ?? new DateTime(), my) : null,
                                       ModifiedOn = cd.ModifiedOn != null ? TimeZoneInfo.ConvertTimeFromUtc(cd.ModifiedOn ?? new DateTime(), my) : null
                                   }).ToList();

            if (!string.IsNullOrEmpty(request.SearchCPStatus))
            {
                switch (request.SearchCPStatus)
                {
                    case "ALL":
                        pricingPlanQuery = pricingPlanQuery.ToList();
                        break;
                    case "ONLINE":
                        pricingPlanQuery = pricingPlanQuery.Where(x => x.IsOnline).ToList();
                        break;
                    case "OFFLINE":
                        pricingPlanQuery = pricingPlanQuery.Where(x => !x.IsOnline).ToList();
                        break;
                }
            }
            if (!string.IsNullOrEmpty(request.SearchCPName))
            {
                pricingPlanQuery = pricingPlanQuery.Where(x => x.CPName.ToLower().Contains(request.SearchCPName.ToLower())).ToList();
            }
            if (request.SearchCPSiteId != null && request.SearchCPSiteId != 0)
            {
                pricingPlanQuery = pricingPlanQuery.Where(x => x.CPSiteId == request.SearchCPSiteId).ToList();
            }
            if (request.SearchProductTypeId != null && request.SearchProductTypeId != 0)
            {
                pricingPlanQuery = pricingPlanQuery.Where(x => x.ProductTypeIds.Contains(request.SearchProductTypeId ?? 0)).ToList();
            }
            if (request.SearchPricingPlanId != null && request.SearchPricingPlanId != 0)
            {
                pricingPlanQuery = pricingPlanQuery.Where(x => x.PricingPlanId == request.SearchPricingPlanId).ToList();
            }

            if (!string.IsNullOrEmpty(request.OrderColumn))
            {
                if ((!string.IsNullOrEmpty(request.OrderBy) && request.OrderBy == "asc"))
                {
                    if (request.OrderColumn == "cpName")
                        pricingPlanQuery = pricingPlanQuery.OrderBy(t => t.CPName).ToList();

                    if (request.OrderColumn == "productTypes")
                        pricingPlanQuery = pricingPlanQuery.OrderBy(t => t.ProductTypes).ToList();

                    if (request.OrderColumn == "cpSiteName")
                        pricingPlanQuery = pricingPlanQuery.OrderBy(t => t.CPSiteName).ToList();

                    if (request.OrderColumn == "pricingPlanName")
                        pricingPlanQuery = pricingPlanQuery.OrderBy(t => t.PricingPlanName).ToList();

                    if (request.OrderColumn == "createdOn")
                        pricingPlanQuery = pricingPlanQuery.OrderBy(t => t.CreatedOn).ToList();

                    if (request.OrderColumn == "modifiedOn")
                        pricingPlanQuery = pricingPlanQuery.OrderBy(t => t.ModifiedOn).ToList();
                }

                if ((!string.IsNullOrEmpty(request.OrderBy) && request.OrderBy == "desc"))
                {
                    if (request.OrderColumn == "cpName")
                        pricingPlanQuery = pricingPlanQuery.OrderByDescending(t => t.CPName).ToList();

                    if (request.OrderColumn == "productTypes")
                        pricingPlanQuery = pricingPlanQuery.OrderByDescending(t => t.ProductTypes).ToList();

                    if (request.OrderColumn == "cpSiteName")
                        pricingPlanQuery = pricingPlanQuery.OrderByDescending(t => t.CPSiteName).ToList();

                    if (request.OrderColumn == "pricingPlanName")
                        pricingPlanQuery = pricingPlanQuery.OrderByDescending(t => t.PricingPlanName).ToList();

                    if (request.OrderColumn == "createdOn")
                        pricingPlanQuery = pricingPlanQuery.OrderByDescending(t => t.CreatedOn).ToList();

                    if (request.OrderColumn == "modifiedOn")
                        pricingPlanQuery = pricingPlanQuery.OrderByDescending(t => t.ModifiedOn).ToList();
                }

            }

            var pagedList = PagedList<CPPricingPlanDisplayModel>.ToPagedList(pricingPlanQuery, request.PageNumber, request.PageSize);

            returnPagedModel.CPPricingPlanList = pagedList;
            returnPagedModel.CurrentPage = pagedList.CurrentPage;
            returnPagedModel.TotalPages = pagedList.TotalPages;
            returnPagedModel.PageSize = pagedList.PageSize;
            returnPagedModel.TotalCount = pagedList.TotalCount;
            returnPagedModel.HasPrevious = pagedList.HasPrevious;
            returnPagedModel.HasNext = pagedList.HasNext;

            return returnPagedModel;
        }

        public DataTable GetChargePointPricingData(SearchCPPricingPlanRequestModel request)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("ChargePointName", typeof(string));
            dt.Columns.Add("ProductType", typeof(string));
            dt.Columns.Add("ChargePointSite", typeof(string));
            dt.Columns.Add("ChargePointStatus", typeof(string));
            dt.Columns.Add("TotalConnector", typeof(string));
            dt.Columns.Add("PricingPlanName", typeof(string));
            dt.Columns.Add("CreatedOn", typeof(string));
            dt.Columns.Add("ModifiedOn", typeof(string));

            var query = GetChargePointPricingPaginationListView(request);

            foreach (var pricingplan in query.CPPricingPlanList)
            {
                dt.Rows.Add(pricingplan.Id,
                    pricingplan.CPName,
                    pricingplan.ProductTypes,
                    pricingplan.CPSiteName,
                    pricingplan.CPStatus,
                    pricingplan.TotalConnector,
                    pricingplan.PricingPlanName,
                    pricingplan.CreatedOn,
                    pricingplan.ModifiedOn);
            }

            return dt;
        }

        public string ChargePointPricingCSVString(SearchCPPricingPlanRequestModel request)
        {
            DataTable dataTable = GetChargePointPricingData(request);
            var csvString = new StringBuilder();
            for (var i = 0; i < dataTable.Columns.Count; i++)
            {
                csvString.Append(dataTable.Columns[i]);
                if (i != dataTable.Columns.Count - 1)
                {
                    csvString.Append(",");
                }
            }
            csvString.Append("\n");
            foreach (DataRow row in dataTable.Rows)
            {
                for (var i = 0; i < dataTable.Columns.Count; i++)
                {
                    csvString.Append(row[i].ToString());
                    if (i != dataTable.Columns.Count - 1)
                    {
                        csvString.Append(",");
                    }
                }
                csvString.Append("\n");
            }

            return csvString.ToString();
        }


        public async Task<UpdateCPPricingResponseModel> UpdateCPPricingAsync(UpdateCPPricingRequestModel request)
        {
            var response = new UpdateCPPricingResponseModel();
            var chargingPoint = _cpDetailsRepository.ToQueryable().Where(x => x.Id == request.CPDetailsId).FirstOrDefault();
            if (chargingPoint == null)
            {
                response.Success = false;
                response.StatusCode = SystemData.StatusCode.NotFound;
                return response;
            }

            var pricingPlan = await _pricingPlanRepository.FindByIdAsync(request.PricingPlanId);
            if (pricingPlan == null)
            {
                response.Success = false;
                response.StatusCode = SystemData.StatusCode.NotFound;
                return response;
            }

            chargingPoint.AssignedOn = DateTime.UtcNow;
            chargingPoint.ModifiedOn = DateTime.UtcNow;
            chargingPoint.PricingPlanId = request.PricingPlanId;
            await _cpDetailsRepository.UpdateAndSaveChangesAsync(chargingPoint);

            response.ChargingPoint = chargingPoint;
            response.Success = true;
            response.StatusCode = SystemData.StatusCode.Success;

            return response;
        }

        public async Task<UpdateCPPricingResponseModel> RemoveCPPricingAsync(int id)
        {
            var response = new UpdateCPPricingResponseModel();
            var chargingPoint = _cpDetailsRepository.ToQueryable().Where(x => x.Id == id).FirstOrDefault();
            if (chargingPoint == null)
            {
                response.Success = false;
                response.StatusCode = SystemData.StatusCode.NotFound;
                return response;
            }

            chargingPoint.AssignedOn = null;
            chargingPoint.ModifiedOn = DateTime.UtcNow;
            chargingPoint.PricingPlanId = null;
            await _cpDetailsRepository.UpdateAndSaveChangesAsync(chargingPoint);

            response.ChargingPoint = chargingPoint;
            response.Success = true;
            response.StatusCode = SystemData.StatusCode.Success;

            return response;
        }

        public PagedPricingPlanDetailsList GetAllPricingPlanPaginationListView(SearchPricingRequestModel request)
        {
            PagedPricingPlanDetailsList returnPagedModel = new();

            TimeZoneInfo my = TimeZoneInfo.FindSystemTimeZoneById("Singapore");
            var query = (from pp in _pricingPlanRepository.ToQueryable()
                        join pv in _priceVariesRepository.ToQueryable()
                        on pp.PriceVariesId equals pv.Id
                        where pp.UserAccountId == request.UserAccountId
                        let refProdTypeIds = _pricingPlanTypeRepository.ToQueryable().Where(x => x.PricingPlanId == pp.Id).Select(x => x.ProductTypeId).Distinct().ToList()
                        let prodTypeNames = _productTypeRepository.ToQueryable().Where(x => refProdTypeIds.Contains(x.Id)).Select(x=>x.Name).ToList()
                        let refUnitIds = _pricingPlanTypeRepository.ToQueryable().Where(x => x.PricingPlanId == pp.Id).Select(x => x.UnitId).Distinct().ToList()
                        let unitNames = _unitRepository.ToQueryable().Where(x => refUnitIds.Contains(x.Id)).Select(x => x.Name).ToList()
                        orderby pp.CreatedOn descending
                        select new PricingPlanDetailsDisplayModel
                        {
                            Id = pp.Id,
                            PlanName = pp.PlanName,
                            PriceVariesId = pv.Id,
                            PriceVaries = pv.Name,
                            FixedFee = pp.FixedFee,
                            ProductTypeIds = refProdTypeIds,
                            ProductTypes = string.Join(", ", prodTypeNames),
                            UnitIds = refUnitIds,
                            Units = string.Join(", ", unitNames),
                            CreatedOn = TimeZoneInfo.ConvertTimeFromUtc(pp.CreatedOn, my),
                            ModifiedOn = pp.ModifiedOn != null ? TimeZoneInfo.ConvertTimeFromUtc(pp.ModifiedOn ?? new DateTime(), my) : null
                        }).ToList();

            if (!string.IsNullOrEmpty(request.SearchPlanName))
            {
                query = query.Where(x => x.PlanName.ToLower().Contains(request.SearchPlanName.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(request.SearchFixedFee))
            {
                query = query.Where(x => x.FixedFee == decimal.Parse(request.SearchFixedFee)).ToList();
            }

            if (request.SearchProductTypeId != 0)
            {
                query = query.Where(x => x.ProductTypeIds.Contains(request.SearchProductTypeId)).ToList();
            }

            if (request.SearchUnitId != 0)
            {
                query = query.Where(x => x.UnitIds.Contains(request.SearchUnitId)).ToList();
            }

            //var pricingPlanList = new List<PricingPlanDetailsDisplayModel>();

            if (!string.IsNullOrEmpty(request.OrderColumn))
            {
                if ((!string.IsNullOrEmpty(request.OrderBy) && request.OrderBy == "asc"))
                {
                    if (request.OrderColumn == "planName")
                        query = query.OrderBy(t => t.PlanName).ToList();

                    if (request.OrderColumn == "priceVaries")
                        query = query.OrderBy(t => t.PriceVaries).ToList();

                    if (request.OrderColumn == "unit")
                        query = query.OrderBy(t => t.Units).ToList();

                    if (request.OrderColumn == "fixedFee")
                        query = query.OrderBy(t => t.FixedFee).ToList();

                    if (request.OrderColumn == "createdOn")
                        query = query.OrderBy(t => t.CreatedOn).ToList();

                    if (request.OrderColumn == "modifiedOn")
                        query = query.OrderBy(t => t.ModifiedOn).ToList();
                }

                if ((!string.IsNullOrEmpty(request.OrderBy) && request.OrderBy == "desc"))
                {
                    if (request.OrderColumn == "planName")
                        query = query.OrderByDescending(t => t.PlanName).ToList();

                    if (request.OrderColumn == "priceVaries")
                        query = query.OrderByDescending(t => t.PriceVaries).ToList();

                    if (request.OrderColumn == "unit")
                        query = query.OrderByDescending(t => t.Units).ToList();

                    if (request.OrderColumn == "fixedFee")
                        query = query.OrderByDescending(t => t.FixedFee).ToList();

                    if (request.OrderColumn == "createdOn")
                        query = query.OrderByDescending(t => t.CreatedOn).ToList();

                    if (request.OrderColumn == "modifiedOn")
                        query = query.OrderByDescending(t => t.ModifiedOn).ToList();
                }
            }

            var pagedList = PagedList<PricingPlanDetailsDisplayModel>.ToPagedList(query, request.PageNumber, request.PageSize);
            returnPagedModel.PricingPlanList = pagedList;
            returnPagedModel.CurrentPage = pagedList.CurrentPage;
            returnPagedModel.TotalPages = pagedList.TotalPages;
            returnPagedModel.PageSize = pagedList.PageSize;
            returnPagedModel.TotalCount = pagedList.TotalCount;
            returnPagedModel.HasPrevious = pagedList.HasPrevious;
            returnPagedModel.HasNext = pagedList.HasNext;


            return returnPagedModel;
        }


        public async Task<PricingPlanResponseModel> CreateUpdatePricingPlanAsync(CreateUpdatePricingPlanRequestModel model)
        {
            PricingPlanResponseModel returnModel = new PricingPlanResponseModel()
            {
                Success = true,
                StatusCode = SystemData.StatusCode.Success
            };
            PricingPlan? pricingPlan = new PricingPlan();
            var exist = _pricingPlanRepository.ToQueryable().Any(x => x.PlanName.ToLower() == model.PlanName.ToLower() && x.Id != model.Id);
            if (exist)
            {
                returnModel.Success = false;
                returnModel.StatusCode = SystemData.StatusCode.Exist;
                return returnModel;
            }

            if (model.Id == null)
            {
                _mapper.Map(model, pricingPlan);
                pricingPlan.ModifiedOn = null;
                pricingPlan = await _pricingPlanRepository.AddAndSaveChangesAsync(pricingPlan);

                List<PricingPlanType> newPricingPlanTypes = _mapper.Map<List<PricingPlanTypeDetails>, List<PricingPlanType>>(model.PricingPlanTypeList);
                newPricingPlanTypes.ForEach(x => { x.PricingPlanId = pricingPlan.Id; x.ModifiedOn = null; }); 
                await _pricingPlanTypeRepository.AddRangeAndSaveChangesAsync(newPricingPlanTypes);
            }

            if (model.Id != null)
            {
                pricingPlan = _pricingPlanRepository.ToQueryable()!.Where(x => x.Id == model.Id && x.UserAccountId == model.UserAccountId).FirstOrDefault();
                if (pricingPlan == null)
                {
                    returnModel.Success = false;
                    returnModel.StatusCode = SystemData.StatusCode.NotFound;
                    return returnModel;
                }

                _mapper.Map(model, pricingPlan);
                pricingPlan.ModifiedOn = DateTime.UtcNow;
                pricingPlan = await _pricingPlanRepository.UpdateAndSaveChangesAsync(pricingPlan);

                var existingPricingPlanTypes = _pricingPlanTypeRepository.ToQueryable().Where(x => x.PricingPlanId == model.Id);
                if(existingPricingPlanTypes != null)
                {
                    if (existingPricingPlanTypes.Count() != model.PricingPlanTypeList.Count())
                    {
                        var pricingPlanTypesId = model.PricingPlanTypeList.Select(x => x.Id).ToList();
                        var pricingPlanTypesToBeDelete = existingPricingPlanTypes.Where(x => !pricingPlanTypesId.Contains(x.Id)).ToList();
                        await _pricingPlanTypeRepository.DeleteRangeAndSaveChangesAsync(pricingPlanTypesToBeDelete);
                    }
                }
                
                foreach (var ppt in model.PricingPlanTypeList)
                {
                    if (ppt.Id == null)
                    {
                        PricingPlanType newPricingPlanType = new PricingPlanType();
                        _mapper.Map(ppt, newPricingPlanType);
                        newPricingPlanType.PricingPlanId = pricingPlan.Id;
                        newPricingPlanType.ModifiedOn = null;
                        await _pricingPlanTypeRepository.AddAndSaveChangesAsync(newPricingPlanType);
                    }

                    if (ppt.Id != null)
                    {
                        var pricingPlanType = await _pricingPlanTypeRepository.FindByIdAsync(ppt.Id ?? 0);
                        _mapper.Map(ppt, pricingPlanType);
                        pricingPlanType.PricingPlanId = pricingPlan.Id;
                        pricingPlanType.ModifiedOn = DateTime.UtcNow;
                        await _pricingPlanTypeRepository.UpdateAndSaveChangesAsync(pricingPlanType);
                    }
                }
            }

            returnModel.PricingPlan = pricingPlan;
            return returnModel;
        }

        public async Task<ResponseModelBase> DeletePricingPlanAsync(int id)
        {
            var response = new ResponseModelBase();
            var pricingPlan = await _pricingPlanRepository.FindByIdAsync(id);
            if (pricingPlan == null)
            {
                response.Success = false;
                response.StatusCode = SystemData.StatusCode.NotFound;
                return response;
            }

            var pricingPlanTypes = _pricingPlanTypeRepository.ToQueryable().Where(x => x.PricingPlanId == id).ToList();
            if (pricingPlanTypes == null)
            {
                response.Success = false;
                response.StatusCode = SystemData.StatusCode.NotFound;
                return response;
            }

            var assignedChargePoint = _cpDetailsRepository.ToQueryable().Where(x => x.PricingPlanId == id).ToList();
            if (assignedChargePoint != null)
            {
                response.Success = false;
                response.StatusCode = SystemData.StatusCode.Invalid;
                return response;
            }

            await _pricingPlanTypeRepository.DeleteRangeAndSaveChangesAsync(pricingPlanTypes);
            await _pricingPlanRepository.DeleteAndSaveChangesAsync(pricingPlan);

            response.Success = true;
            response.StatusCode = SystemData.StatusCode.Success;
            return response;
        }

        public async Task<List<PriceVaries>> GetAllPriceVariesAsync()
        {
            return await _priceVariesRepository.GetAllAsync();
        }

        public async Task<List<Unit>> GetAllUnitAsync()
        {
            return await _unitRepository.GetAllAsync();
        }

        public List<PricingPlanDisplayModel> GetAllPricingPlanByUserAccountId(int id)
        {
            var ppQuery = (from pp in _pricingPlanRepository.ToQueryable().Where(x => x.UserAccountId == id)
                        join pv in _priceVariesRepository.ToQueryable()
                        on pp.PriceVariesId equals pv.Id
                        select new PricingPlanDisplayModel()
                        {
                            Id = pp.Id,
                            PlanName = pp.PlanName,
                            FixedFee = pp.FixedFee,
                            PriceVariesId = pv.Id,
                            PriceVaries = pv.Name,
                            PerBlock = pp.PerBlock,
                        }).ToList();

            foreach (var pp in ppQuery)
            {
                var pptQuery = from ppt in _pricingPlanTypeRepository.ToQueryable()
                               join u in _unitRepository.ToQueryable()
                               on ppt.UnitId equals u.Id
                               join pt in _productTypeRepository.ToQueryable()
                               on ppt.ProductTypeId equals pt.Id
                               where ppt.PricingPlanId == pp.Id
                               select new PricingPlanTypeDetails()
                               {
                                   Id = ppt.Id,
                                   ProductTypeId = ppt.ProductTypeId,
                                   PriceRate = ppt.PriceRate,
                                   UnitId = ppt.UnitId,
                                   UnitName = u.Name,
                                   ProductTypeName = pt.Name
                               };

                pp.PricingPlanTypeList = pptQuery.ToList();
            }

            return ppQuery;
        }

        public PricingPlanDisplayModel? GetAllPricingPlanBId(int id)
        {
            var ppQuery = (from pp in _pricingPlanRepository.ToQueryable()
                           join pv in _priceVariesRepository.ToQueryable()
                           on pp.PriceVariesId equals pv.Id
                           where pp.Id == id
                           select new PricingPlanDisplayModel()
                           {
                               Id = pp.Id,
                               PlanName = pp.PlanName,
                               FixedFee = pp.FixedFee,
                               PriceVariesId = pv.Id,
                               PriceVaries = pv.Name,
                               PerBlock = pp.PerBlock,
                           }).FirstOrDefault();

            if (ppQuery != null)
            {
                var pptQuery = from ppt in _pricingPlanTypeRepository.ToQueryable()
                               join u in _unitRepository.ToQueryable()
                               on ppt.UnitId equals u.Id
                               join pt in _productTypeRepository.ToQueryable()
                               on ppt.ProductTypeId equals pt.Id
                               where ppt.PricingPlanId == ppQuery.Id
                               select new PricingPlanTypeDetails()
                               {
                                   Id = ppt.Id,
                                   ProductTypeId = ppt.ProductTypeId,
                                   PriceRate = ppt.PriceRate,
                                   UnitId = ppt.UnitId,
                                   UnitName = u.Name,
                                   ProductTypeName = pt.Name
                               };

                ppQuery.PricingPlanTypeList = pptQuery.ToList();
            }           

            return ppQuery;
        }

        #endregion
    }
}
