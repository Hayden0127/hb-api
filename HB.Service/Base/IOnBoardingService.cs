using HB.Model;
using Strateq.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Service
{
    public interface IOnBoardingService
    {
        CPSiteDisplayListModel GetAllCPSiteListingByUserAccountId(int id);
        Task<NewSiteOnBoardingResponseModel> CreateNewSiteOnBoardingAsync(OnBoardingNewSiteRequestModel model);
        ValidateCPNameResponseModel ValidateCPNameAndSerialNo(ValidateCPNameRequestModel request);
        Task<NewCPOnBoardingResponseModel> CreateNewCPOnBoardingAsync(OnBoardingNewCPRequestModel model);
    }
}
