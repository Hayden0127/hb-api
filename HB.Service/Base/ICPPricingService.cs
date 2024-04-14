using HB.Database.DbModels;
using HB.Model;
using Microsoft.AspNetCore.Mvc;
using Strateq.Core.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Service
{ 
    public interface ICPPricingService
    {
        Task<UpdateCPPricingResponseModel> UpdateCPPricingAsync(UpdateCPPricingRequestModel request);
        Task<UpdateCPPricingResponseModel> RemoveCPPricingAsync(int id);
        PagedPricingPlanDetailsList GetAllPricingPlanPaginationListView(SearchPricingRequestModel request);
        PagedCPPricingPlanList GetChargePointPricingPaginationListView(SearchCPPricingPlanRequestModel request);
        string ChargePointPricingCSVString(SearchCPPricingPlanRequestModel request);
        Task<PricingPlanResponseModel> CreateUpdatePricingPlanAsync(CreateUpdatePricingPlanRequestModel model);
        Task<ResponseModelBase> DeletePricingPlanAsync(int id);
        Task<List<PriceVaries>> GetAllPriceVariesAsync();
        Task<List<Unit>> GetAllUnitAsync();
        List<PricingPlanDisplayModel> GetAllPricingPlanByUserAccountId(int id);
        PricingPlanDisplayModel? GetAllPricingPlanBId(int id);
    }
}
