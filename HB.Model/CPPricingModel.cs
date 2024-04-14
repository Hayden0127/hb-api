using Strateq.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HB.Database.DbModels;

namespace HB.Model
{
    public class SearchCPPricingPlanRequestModel : PaginationQueryStringParameters
    {
        public string SearchCPName { get; set; }
        public int? SearchProductTypeId { get; set; }
        public int? SearchCPSiteId { get; set; }
        public int? SearchPricingPlanId { get; set; }
        public string SearchCPStatus { get; set; }
        public string OrderColumn { get; set; }
        public string OrderBy { get; set; }
        public int UserAccountId { get; set; }
    }

    public class CPPricingPlanDisplayModel
    {
        public int Id { get; set; }
        public string CPName { get; set; }
        public List<int> ProductTypeIds { get; set; }
        public string ProductTypes { get; set; }
        public int CPSiteId { get; set; }
        public string CPSiteName { get; set; }
        public string CPSiteAddress { get; set; }
        public string CPStatus { get; set; }
        public bool IsOnline { get; set; }
        public int TotalConnector { get; set; }
        public int? PricingPlanId { get; set; }
        public string PricingPlanName { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }

    public class PagedCPPricingPlanList : PagerModel
    {
        public List<CPPricingPlanDisplayModel> CPPricingPlanList { get; set; }
    }

    public class UpdateCPPricingRequestModel : RequestModelBase
    {
        public int CPDetailsId { get; set; }
        public int PricingPlanId { get; set; }
    }

    public class UpdateCPPricingResponseModel : ResponseModelBase
    {
        public CPDetails ChargingPoint { get; set; }
    }

    public class SearchPricingRequestModel : PaginationQueryStringParameters
    {
        public int UserAccountId { get; set; }
        public string SearchPlanName { get; set; }
        public string SearchFixedFee { get; set; }
        public int SearchProductTypeId { get; set; }
        public int SearchUnitId { get; set; }
        public string OrderColumn { get; set; }
        public string OrderBy { get; set; }

    }

    public class PricingPlanDetailsDisplayModel
    {
        public int Id { get; set; }
        public string PlanName { get; set; }
        public int PriceVariesId { get; set; }
        public string PriceVaries { get; set; }
        public decimal FixedFee { get; set; }
        public List<int> ProductTypeIds { get; set; }
        public string ProductTypes { get; set; }
        public List<int> UnitIds { get; set; }
        public string Units { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }

    public class PagedPricingPlanDetailsList : PagerModel
    {
        public List<PricingPlanDetailsDisplayModel> PricingPlanList { get; set; }
    }

    public class PricingPlanListResponseModel: ResponseModelBase
    {
        public PagedPricingPlanDetailsList PricingPlanList { get; set; }
    }

    public class CreateUpdatePricingPlanRequestModel : RequestModelBase
    {
        public int? UserAccountId { get; set; }
        public int? Id { get; set; }
        public string PlanName { get; set; }
        public decimal FixedFee { get; set; }
        public int PriceVariesId { get; set; }
        public int? PerBlock { get; set; }
        public List<PricingPlanTypeDetails> PricingPlanTypeList { get; set; }
    }

    public class PricingPlanTypeDetails
    {
        public int? Id { get; set; }
        public int ProductTypeId { get; set; }
        public decimal PriceRate { get; set; }
        public int UnitId { get; set; }
        public string UnitName { get; set; } //create & update not needed to pass in
        public string ProductTypeName { get; set; } //create & update not needed to pass in
        public decimal FixedFee { get; set; }
        public int? PerBlock { get; set; }
    }

    public class PricingPlanResponseModel : ResponseModelBase
    {
        public PricingPlan PricingPlan { get; set; }
    }

    public class PricingPlanDisplayModel
    {
        public int? Id { get; set; }
        public string PlanName { get; set; }
        public decimal FixedFee { get; set; }
        public int PriceVariesId { get; set; }
        public string PriceVaries { get; set; }
        public int? PerBlock { get; set; }
        public List<PricingPlanTypeDetails> PricingPlanTypeList { get; set; }
    }
}
