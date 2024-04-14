using AutoMapper;
using HB.Database.DbModels;
using HB.Model;

namespace HB.Utilities
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<CPSiteDetailsModel, CPSiteDetails>();
            CreateMap<CreateUpdatePricingPlanRequestModel, PricingPlan>();
            CreateMap<PricingPlanTypeDetails, PricingPlanType>();
            CreateMap<SampledValue, MeterValue>();

        }
    }
}
