using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HB.SmartSD.Integrator
{
    public class CreateIncidentsRequestModel
    {
        [JsonPropertyName("summary")]
        public string Summary { get; set; } = string.Empty;
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
        [JsonPropertyName("auto_assign")]
        public bool? AutoAssign { get; set; } = null;
        [JsonPropertyName("site_id")]
        public int? SiteId { get; set; } = null;
    }

    public class CreateIncidentsResponseModel
    {
        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;
        [JsonPropertyName("incident_id")]
        public int IncidentId { get; set; } = 0;
        [JsonPropertyName("incident_no")]
        public string IncidentNo { get; set; } = string.Empty;
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;
        [JsonPropertyName("status_code")]
        public int StatusCode { get; set; } = 0;
    }

    public class GetIncidentsResponseModel
    {
        [JsonPropertyName("incident_id")]
        public int IncidentId { get; set; } = 0;
        [JsonPropertyName("incident_no")]
        public string IncidentNo { get; set; } = string.Empty;
        [JsonPropertyName("summary")]
        public string Summary { get; set; } = string.Empty;
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
        [JsonPropertyName("status")]
        public int Status { get; set; } = 0;
        [JsonPropertyName("status_code")]
        public int StatusCode { get; set; } = 0;
        [JsonPropertyName("severity")]
        public int Severity { get; set; } = 0;
        [JsonPropertyName("sla_deadline")]
        public string SlaDeadline { get; set; } = string.Empty;
        [JsonPropertyName("helpdesk_response")]
        public string HelpdeskResponse { get; set; } = string.Empty;
        [JsonPropertyName("helpdesk_resolution")]
        public string HelpdeskResolution { get; set; } = string.Empty;
        [JsonPropertyName("onsite_response")]
        public string OnsiteResponse { get; set; } = string.Empty;
        [JsonPropertyName("onsite_resolution")]
        public string OnsiteResolution { get; set; } = string.Empty;
        [JsonPropertyName("reporter_name")]
        public string ReporterName { get; set; } = string.Empty;
        [JsonPropertyName("reporter_contact")]
        public string ReporterContact { get; set; } = string.Empty;
        [JsonPropertyName("reporter_email")]
        public string ReporterEmail { get; set; } = string.Empty;
        [JsonPropertyName("reporting_channel")]
        public string ReportingChannel { get; set; } = string.Empty;
        [JsonPropertyName("customer_ref_no")]
        public string CustomerRefNo { get; set; } = string.Empty;
        [JsonPropertyName("user_group")]
        public string UserGroup { get; set; } = string.Empty;
        [JsonPropertyName("assignee")]
        public string Assignee { get; set; } = string.Empty;
        [JsonPropertyName("assignee_id")]
        public string AssigneeId { get; set; } = string.Empty;
        [JsonPropertyName("resolution_time")]
        public string ResolutionTime { get; set; } = string.Empty;
        [JsonPropertyName("resolution_description")]
        public string ResolutionDescription { get; set; } = string.Empty;
        [JsonPropertyName("resolved_by")]
        public string ResolvedBy { get; set; } = string.Empty;
        [JsonPropertyName("support_type")]
        public string SupportType { get; set; } = string.Empty;
        [JsonPropertyName("service_action")]
        public string ServiceAction { get; set; } = string.Empty;
        [JsonPropertyName("service_item")]
        public string ServiceItem { get; set; } = string.Empty;
        [JsonPropertyName("service_type")]
        public string ServiceType { get; set; } = string.Empty;
        [JsonPropertyName("forward_to")]
        public string ForwardTo { get; set; } = string.Empty;
        [JsonPropertyName("escalation_reason")]
        public string EscalationReason { get; set; } = string.Empty;
        [JsonPropertyName("first_call_resolution")]
        public string FirstCallResolution { get; set; } = string.Empty;
        [JsonPropertyName("organisation_id")]
        public int OrganisationId { get; set; } = 0;
        [JsonPropertyName("organisation")]
        public string Organisation { get; set; } = string.Empty;
        [JsonPropertyName("site_id")]
        public int SiteId { get; set; } = 0;
        [JsonPropertyName("site")]
        public string Site { get; set; } = string.Empty;
        [JsonPropertyName("site_roid")]
        public string SiteRoid { get; set; } = string.Empty;
        [JsonPropertyName("trading_name")]
        public string TradingName { get; set; } = string.Empty;
        [JsonPropertyName("created_by")]
        public string CreatedBy { get; set; } = string.Empty;
        [JsonPropertyName("created_at")]
        public string CreatedAt { get; set; } = string.Empty;

    }

    public class CreateSiteRequestModel
    {
        [JsonPropertyName("site_name")]
        public string SiteName { get; set; } = string.Empty;
        [JsonPropertyName("trading_name")]
        public string TradingName { get; set; } = string.Empty;
        [JsonPropertyName("address")]
        public string Address { get; set; } = string.Empty;
        [JsonPropertyName("contact_name")]
        public string ContactName { get; set; } = string.Empty;
        [JsonPropertyName("contact_phone")]
        public string ContactPhone { get; set; } = string.Empty;
        [JsonPropertyName("latitude")]
        public string Latitude { get; set; } = string.Empty;
        [JsonPropertyName("longitude")]
        public string Longitude { get; set; } = string.Empty;
    }

    public class CreateSiteResponseModel
    {
        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;
        [JsonPropertyName("site_id")]
        public int SiteId { get; set; } = 0;
        [JsonPropertyName("site_name")]
        public string SiteName { get; set; } = string.Empty;
        [JsonPropertyName("contact_name")]
        public string ContactName { get; set; } = string.Empty;
        [JsonPropertyName("contact_phone")]
        public string ContactPhone { get; set; } = string.Empty;
        [JsonPropertyName("latitude")]
        public string Latitude { get; set; } = string.Empty;
        [JsonPropertyName("longitude")]
        public string Longitude { get; set; } = string.Empty;
    }

    public class GetSitesResponseModel
    {
        [JsonPropertyName("site_id")]
        public int SiteId { get; set; } = 0;
        [JsonPropertyName("site_name")]
        public string SiteName { get; set; } = string.Empty;
        [JsonPropertyName("address")]
        public string Address { get; set; } = string.Empty;
        [JsonPropertyName("trading_name")]
        public string TradingName { get; set; } = string.Empty;
        [JsonPropertyName("organization")]
        public int Organization { get; set; } = 0;
        [JsonPropertyName("status")]
        public int Status { get; set; } = 0;
        [JsonPropertyName("latitude")]
        public string Latitude { get; set; } = string.Empty;
        [JsonPropertyName("longitude")]
        public string Longitude { get; set; } = string.Empty;
        [JsonPropertyName("contact_name")]
        public string ContactName { get; set; } = string.Empty;
        [JsonPropertyName("contact_phone")]
        public string ContactPhone { get; set; } = string.Empty;
    }
}
