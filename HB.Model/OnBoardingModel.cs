using Strateq.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Model
{
    public class CPSiteDisplayListModel : ResponseModelBase
    {
        public List<CPSiteDetailsModel> CPSiteList { get; set; }
    }

    public class NewSiteOnBoardingResponseModel : ResponseModelBase
    {
        public CPSiteDetailsModel SiteDetails { get; set; }
    }

    public class ValidateCPNameRequestModel : RequestModelBase
    {
        public int CPSiteDetailsId { get; set; }
        public string Name { get; set; }
        public string SerialNo { get; set; }
    }

    public class ValidateCPNameResponseModel : ResponseModelBase
    {
        public bool IsName { get; set; }
        public bool IsSerialNo { get; set; }
    }

    public class OnBoardingNewSiteRequestModel : RequestModelBase
    {
        public int UserAccountId { get; set; }
        public CPSiteDetailsModel SiteDetails { get; set; }
    }

    public class CPSiteDetailsModel
    {
        public int Id { get; set; } = 0;
        public string PersonInCharge { get; set; }
        public string SiteName { get; set; }
        public string Email { get; set; }
        public string OfficeNo { get; set; }
        public string MobileNo { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string MaintenanceProgram { get; set; }
        public string Description { get; set; }
        public int? LoadLimit { get; set; }
    }

    public class NewCPOnBoardingResponseModel : ResponseModelBase
    {
        public CPDetailsModel CPDetails { get; set; }
        public List<CPConnectorDetailsModel> CPConnectorList { get; set; } = new();
    }

    public class OnBoardingNewCPRequestModel : RequestModelBase
    {
        public CPDetailsModel CPDetails { get; set; }
        public List<CPConnectorDetailsModel> CPConnectorList { get; set; } = new();
    }

    public class CPDetailsModel
    {
        public int CPSiteDetailsId { get; set; }
        public string Name { get; set; }
        public string SerialNo { get; set; }
    }

    public class CPConnectorDetailsModel
    {
        public string Name { get; set; }
        public int ProductTypeId { get; set; }
        public int PowerOutput { get; set; }
    }
}
