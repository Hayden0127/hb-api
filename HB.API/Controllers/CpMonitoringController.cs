using HB.Model;
using HB.Service;
using HB.SmartSD.Integrator;
using HB.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Strateq.Core.API.Controllers;
using Strateq.Core.API.Exceptions;
using Strateq.Core.Model;
using Strateq.Core.Service;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Websocket.Client;

namespace HB.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]

    public class CPMonitoringController : STQControllerBase
    {

        #region Fields
        private readonly ISystemLogService _logger;
        private readonly ICPMonitoringService _cpMonitoringService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region Ctor
        public CPMonitoringController(
            ISystemLogService logger,
            ICPMonitoringService cpMonitoringService,
            IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _cpMonitoringService = cpMonitoringService;
            _httpContextAccessor = httpContextAccessor; 
        }
        #endregion

        #region Methods
        [HttpGet]
        [Route("GetAllChargePointMarkerByUserAccountId")]
        [SwaggerResponse(200, "Success.", typeof(ResponseModelBase))]
        [SwaggerResponse(400, "Invalid parameter(s).", typeof(ErrorResponseModel))]
        [SwaggerResponse(500, "Internal server error.", typeof(ErrorResponseModel))]
        public async Task<ActionResult> GetAllChargePointMarker()
        {
            StringBuilder logger = new StringBuilder();
            try
            {
                var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssffff");
                logger.Append($"GetAllChargePointMarker Request Timestamp {timestamp}");

                if (_httpContextAccessor.HttpContext == null) throw new Exception("Unfound HttpContext");
                var currentUserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if (currentUserId == null) throw new CustomValidationException(SystemData.ErrorCode.Validation, $"User {SystemData.CustomValidation.NotFound}");

                var dbChargePointMarkerList = _cpMonitoringService.GetAllChargePointMarkerByUserAccountId(int.Parse(currentUserId.Value));
                await _logger.LogInformation(logger.ToString());
                return Ok(dbChargePointMarkerList);
            }
            catch (Exception ex)
            {
                await _logger.LogError("Error - ", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
            finally
            {

            }
        }

        [HttpGet]
        [Route("GetCPSiteSummaryDisplayByUserAccountId/{duration}")]
        [SwaggerResponse(200, "Success.", typeof(ResponseModelBase))]
        [SwaggerResponse(400, "Invalid parameter(s).", typeof(ErrorResponseModel))]
        [SwaggerResponse(500, "Internal server error.", typeof(ErrorResponseModel))]
        public async Task<ActionResult> GetCPSiteSummaryDisplay([FromRoute] int duration)
        {
            StringBuilder logger = new StringBuilder();
            try
            {
                var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssffff");
                logger.Append($"GetCPSiteSummaryDisplayByUserAccountId Request Timestamp {timestamp}");

                if (_httpContextAccessor.HttpContext == null) throw new Exception("Unfound HttpContext");
                var currentUserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if (currentUserId == null) throw new CustomValidationException(SystemData.ErrorCode.Validation, $"User {SystemData.CustomValidation.NotFound}");

                var dbChargePointMarkerList = _cpMonitoringService.GetCPSiteSummaryDisplayByUserAccountId(int.Parse(currentUserId.Value), duration);
                await _logger.LogInformation(logger.ToString());
                return Ok(dbChargePointMarkerList);
            }
            catch (Exception ex)
            {
                await _logger.LogError("Error - ", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
            finally
            {

            }
        }

        [HttpPost]
        [Route("SearchCPSiteDisplayByUserAccountId")]
        [SwaggerResponse(200, "Success.", typeof(ResponseModelBase))]
        [SwaggerResponse(400, "Invalid parameter(s).", typeof(ErrorResponseModel))]
        [SwaggerResponse(500, "Internal server error.", typeof(ErrorResponseModel))]
        public async Task<ActionResult> SearchCPSiteDisplay(SearchCPSiteDisplayRequestModel model)
        {
            StringBuilder logger = new StringBuilder();
            try
            {
                var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssffff");
                logger.Append($"SearchCPSiteDisplayByUserAccountId Request Timestamp {model.Timestamp}");

                if (_httpContextAccessor.HttpContext == null) throw new Exception("Unfound HttpContext");
                var currentUserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if (currentUserId == null) throw new CustomValidationException(SystemData.ErrorCode.Validation, $"User {SystemData.CustomValidation.NotFound}");

                model.UserAccountId = int.Parse(currentUserId.Value);
                var dbChargePointMarkerList = _cpMonitoringService.SearchCPSiteDisplayByUserAccountId(model);
                await _logger.LogInformation(logger.ToString());
                return Ok(dbChargePointMarkerList);
            }
            catch (Exception ex)
            {
                await _logger.LogError("Error - ", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
            finally
            {

            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("UpdateCPStatus")]
        [SwaggerResponse(200, "Success.", typeof(ResponseModelBase))]
        [SwaggerResponse(400, "Invalid parameter(s).", typeof(ErrorResponseModel))]
        [SwaggerResponse(500, "Internal server error.", typeof(ErrorResponseModel))]
        public async Task<ActionResult> UpdateCPStatus([FromBody, SwaggerRequestBody("Request body", Required = true)] UpdateCPStatusRequestModel request)
        {
            StringBuilder logger = new StringBuilder();
            try
            {
                logger.Append($"UpdateCPStatusAsync Request Timestamp {request.Timestamp}");
                logger.Append($"UpdateCPStatusAsync SerialNumber - {request.StatusNotification.VendorId}");
                await _cpMonitoringService.UpdateCPStatusAsync(request.StatusNotification);
                await _logger.LogInformation(logger.ToString());
                return Ok();
            }
            catch (SmartSDException ex)
            {
                await _logger.LogError("Error - ", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
            catch (Exception ex)
            {
                await _logger.LogError("Error - ", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
            finally
            {

            }
        }

        [HttpGet]
        [Route("GetCPMonitoringChartDetails/{id}")]
        [SwaggerResponse(200, "Success.", typeof(ResponseModelBase))]
        [SwaggerResponse(400, "Invalid parameter(s).", typeof(ErrorResponseModel))]
        [SwaggerResponse(500, "Internal server error.", typeof(ErrorResponseModel))]
        public async Task<ActionResult> GetCPMonitoringChartDetails([FromRoute] int id)
        {
            StringBuilder logger = new StringBuilder();

            try
            {
                var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssffff");
                logger.Append($"GetCPMonitoringChartDetailsAsync Request Timestamp {timestamp}");
                var response = await _cpMonitoringService.GetCPMonitoringChartDetailsAsync(id);
                await _logger.LogInformation(logger.ToString());
                return Ok(response);
            }

            catch (CustomValidationException ex)
            {
                logger.Append($"[Validation Exception] {ex.Key} {ex.Message}");
                await _logger.LogInformation(logger.ToString());
                ErrorResponseModel response = new() { Code = SystemData.ErrorCode.Validation, Message = $"{ex.Key} {ex.Message}" };
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                await _logger.LogError(logger.ToString(), ex);
                ErrorResponseModel response = new() { Code = SystemData.ErrorCode.InternalServer, Message = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            finally
            {

            }

        }

        [HttpGet]
        [Route("GetDashboardDetails/{days}")]
        [SwaggerResponse(200, "Success.", typeof(ResponseModelBase))]
        [SwaggerResponse(400, "Invalid parameter(s).", typeof(ErrorResponseModel))]
        [SwaggerResponse(500, "Internal server error.", typeof(ErrorResponseModel))]
        public async Task<ActionResult> GetDashboardDetails([FromRoute] int days)
        {
            StringBuilder logger = new StringBuilder();

            try
            {
                var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssffff");
                logger.Append($"GetDashboardDetailsAsync Request Timestamp {timestamp}");

                if (_httpContextAccessor.HttpContext == null) throw new Exception("Unfound HttpContext");
                var currentUserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if (currentUserId == null) throw new CustomValidationException(SystemData.ErrorCode.Validation, $"User {SystemData.CustomValidation.NotFound}");

                var response = await _cpMonitoringService.GetDashboardDetailsAsync(int.Parse(currentUserId.Value), days);
                await _logger.LogInformation(logger.ToString());
                return Ok(response);
            }

            catch (CustomValidationException ex)
            {
                logger.Append($"[Validation Exception] {ex.Key} {ex.Message}");
                await _logger.LogInformation(logger.ToString());
                ErrorResponseModel response = new() { Code = SystemData.ErrorCode.Validation, Message = $"{ex.Key} {ex.Message}"  };
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                await _logger.LogError(logger.ToString(), ex);
                ErrorResponseModel response = new() { Code = SystemData.ErrorCode.InternalServer, Message = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            finally
            {

            }

        }


        [HttpGet]
        [Route("GetCPSiteDetailsById/{id}")]
        [SwaggerResponse(200, "Success.", typeof(ResponseModelBase))]
        [SwaggerResponse(400, "Invalid parameter(s).", typeof(ErrorResponseModel))]
        [SwaggerResponse(500, "Internal server error.", typeof(ErrorResponseModel))]
        public async Task<ActionResult> GetCPSiteDetailsById([FromRoute] int id)
        {
            StringBuilder logger = new StringBuilder();

            try
            {
                var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssffff");
                logger.Append($"GetCPSiteDetailsByIdAsync Request Timestamp {timestamp}");
                var response = await _cpMonitoringService.GetCPSiteDetailsByIdAsync(id);
                await _logger.LogInformation(logger.ToString());
                return Ok(response);
            }

            catch (CustomValidationException ex)
            {
                logger.Append($"[Validation Exception] {ex.Key} {ex.Message}");
                await _logger.LogInformation(logger.ToString());
                ErrorResponseModel response = new() { Code = SystemData.ErrorCode.Validation, Message = $"{ex.Key} {ex.Message}" };
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                await _logger.LogError(logger.ToString(), ex);
                ErrorResponseModel response = new() { Code = SystemData.ErrorCode.InternalServer, Message = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            finally
            {

            }

        }

        [HttpPost]
        [Route("UpdateCPSiteDetails")]
        [SwaggerResponse(200, "Success.", typeof(ResponseModelBase))]
        [SwaggerResponse(400, "Invalid parameter(s).", typeof(ErrorResponseModel))]
        [SwaggerResponse(500, "Internal server error.", typeof(ErrorResponseModel))]
        public async Task<ActionResult>UpdateCPSiteDetails(CPSiteDetailsRequestModel model)
        {
            StringBuilder logger = new StringBuilder();

            try
            {
                var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssffff");
                logger.Append($"UpdateCPSiteDetailsAsync Request Timestamp {timestamp}");
                var response = await _cpMonitoringService.UpdateCPSiteDetailsAsync(model);
                await _logger.LogInformation(logger.ToString());
                return Ok(response);
            }

            catch (CustomValidationException ex)
            {
                logger.Append($"[Validation Exception] {ex.Key} {ex.Message}");
                await _logger.LogInformation(logger.ToString());
                ErrorResponseModel response = new() { Code = SystemData.ErrorCode.Validation, Message = $"{ex.Key} {ex.Message}" };
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                await _logger.LogError(logger.ToString(), ex);
                ErrorResponseModel response = new() { Code = SystemData.ErrorCode.InternalServer, Message = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            finally
            {

            }

        }


        [HttpDelete]
        [Route("DeleteCPSiteDetails/{id}")]
        [SwaggerResponse(200, "Success.", typeof(ResponseModelBase))]
        [SwaggerResponse(400, "Invalid parameter(s).", typeof(ErrorResponseModel))]
        [SwaggerResponse(500, "Internal server error.", typeof(ErrorResponseModel))]
        public async Task<ActionResult> DeleteCPSiteDetails([FromRoute] int id)
        {
            StringBuilder logger = new StringBuilder();

            try
            {
                var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssffff");
                logger.Append($"DeleteCPSiteDetailsAsync Request Timestamp {timestamp}");
                var response = await _cpMonitoringService.DeleteCPSiteDetailsByIdAsync(id);
                await _logger.LogInformation(logger.ToString());
                return Ok(response);
            }

            catch (CustomValidationException ex)
            {
                logger.Append($"[Validation Exception] {ex.Key} {ex.Message}");
                await _logger.LogInformation(logger.ToString());
                ErrorResponseModel response = new() { Code = SystemData.ErrorCode.Validation, Message = $"{ex.Key} {ex.Message}" };
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                await _logger.LogError(logger.ToString(), ex);
                ErrorResponseModel response = new() { Code = SystemData.ErrorCode.InternalServer, Message = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            finally
            {

            }

        }

        [HttpGet]
        [Route("GetCPDetailsBySiteId/{id}")]
        [SwaggerResponse(200, "Success.", typeof(ResponseModelBase))]
        [SwaggerResponse(400, "Invalid parameter(s).", typeof(ErrorResponseModel))]
        [SwaggerResponse(500, "Internal server error.", typeof(ErrorResponseModel))]
        public async Task<ActionResult> GetCPDetailsBySiteId([FromRoute] int id)
        {
            StringBuilder logger = new StringBuilder();

            try
            {
                var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssffff");
                logger.Append($"GetCPDetailsBySiteId Request Timestamp {timestamp}");
                var response = _cpMonitoringService.GetCPDetailsBySiteId(id);
                await _logger.LogInformation(logger.ToString());
                return Ok(response);
            }

            catch (CustomValidationException ex)
            {
                logger.Append($"[Validation Exception] {ex.Key} {ex.Message}");
                await _logger.LogInformation(logger.ToString());
                ErrorResponseModel response = new() { Code = SystemData.ErrorCode.Validation, Message = $"{ex.Key} {ex.Message}" };
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                await _logger.LogError(logger.ToString(), ex);
                ErrorResponseModel response = new() { Code = SystemData.ErrorCode.InternalServer, Message = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            finally
            {

            }

        }

        [HttpPost]
        [Route("ViewAllSiteListing")]
        [SwaggerResponse(200, "Success.", typeof(ResponseModelBase))]
        [SwaggerResponse(400, "Invalid parameter(s).", typeof(ErrorResponseModel))]
        [SwaggerResponse(500, "Internal server error.", typeof(ErrorResponseModel))]
        public async Task<ActionResult> ViewAllSiteListing(ViewAllSiteListingRequestModel request)
        {
            StringBuilder logger = new StringBuilder();

            try
            {
                var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssffff");
                logger.Append($"ViewAllSiteListing Timestamp {timestamp}");
                await _logger.LogInformation(logger.ToString());
                await _logger.LogInformation($"ViewAllSiteListingRequest: {JsonSerializer.Serialize(request)}");

                if (_httpContextAccessor.HttpContext == null) throw new Exception("Unfound HttpContext");
                var currentUserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if (currentUserId == null) throw new CustomValidationException(SystemData.ErrorCode.Validation, $"User {SystemData.CustomValidation.NotFound}");

                request.SearchSiteRequest.UserAccontId = int.Parse(currentUserId.Value);
                ViewAllSiteListingResponseModel response = _cpMonitoringService.ViewAllSiteListing(request);

                return Ok(response);
            }
            catch (CustomValidationException ex)
            {
                logger.Append($"[Validation Exception] {ex.Key} {ex.Message}");
                await _logger.LogInformation(logger.ToString());
                ResponseModelBase response = new() { Timestamp = DateTime.Now.ToString("yyyyMMddHHmmssffff"), Success = false, StatusCode = SystemData.StatusCode.NotFound };
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                await _logger.LogError(logger.ToString(), ex);
                ResponseModelBase response = new() { Timestamp = DateTime.Now.ToString("yyyyMMddHHmmssffff"), Success = false, StatusCode = SystemData.StatusCode.Invalid };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            finally
            {

            }

        }

        [HttpPost]
        [Route("GetChargePointDashboard")]
        [SwaggerResponse(200, "Success.", typeof(ResponseModelBase))]
        [SwaggerResponse(400, "Invalid parameter(s).", typeof(ErrorResponseModel))]
        [SwaggerResponse(500, "Internal server error.", typeof(ErrorResponseModel))]
        public async Task<ActionResult> GetChargePointDashboard(CPDashboardRequestModel request)
        {
            StringBuilder logger = new StringBuilder();

            try
            {
                var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssffff");
                logger.Append($"GetChargePointDashboard Timestamp {timestamp}");
                await _logger.LogInformation(logger.ToString());
                await _logger.LogInformation($"GetChargePointDashboard: {JsonSerializer.Serialize(request)}");
                CPDashboardResponseModel response = _cpMonitoringService.DisplayChargePointDashboard(request.Id);

                return Ok(response);
            }
            catch (CustomValidationException ex)
            {
                logger.Append($"[Validation Exception] {ex.Key} {ex.Message}");
                await _logger.LogInformation(logger.ToString());
                ResponseModelBase response = new() { Timestamp = DateTime.Now.ToString("yyyyMMddHHmmssffff"), Success = false, StatusCode = SystemData.StatusCode.NotFound };
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                await _logger.LogError(logger.ToString(), ex);
                ResponseModelBase response = new() { Timestamp = DateTime.Now.ToString("yyyyMMddHHmmssffff"), Success = false, StatusCode = SystemData.StatusCode.Invalid };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            finally
            {

            }

        }

        [HttpPost]
        [Route("GetConnectorByCPId")]
        [SwaggerResponse(200, "Success.", typeof(ResponseModelBase))]
        [SwaggerResponse(400, "Invalid parameter(s).", typeof(ErrorResponseModel))]
        [SwaggerResponse(500, "Internal server error.", typeof(ErrorResponseModel))]
        public async Task<ActionResult> GetConnectorByCPId(CPDashboardRequestModel request)
        {
            StringBuilder logger = new StringBuilder();

            try
            {
                var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssffff");
                logger.Append($"GetConnectorByCPId Timestamp {timestamp}");
                await _logger.LogInformation(logger.ToString());
                await _logger.LogInformation($"GetConnectorByCPId: {JsonSerializer.Serialize(request)}");
                ConnectorListResponseModel response = _cpMonitoringService.GetConnectorDetails(request.Id);

                return Ok(response);
            }
            catch (CustomValidationException ex)
            {
                logger.Append($"[Validation Exception] {ex.Key} {ex.Message}");
                await _logger.LogInformation(logger.ToString());
                ResponseModelBase response = new() { Timestamp = DateTime.Now.ToString("yyyyMMddHHmmssffff"), Success = false, StatusCode = SystemData.StatusCode.NotFound };
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                await _logger.LogError(logger.ToString(), ex);
                ResponseModelBase response = new() { Timestamp = DateTime.Now.ToString("yyyyMMddHHmmssffff"), Success = false, StatusCode = SystemData.StatusCode.Invalid };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            finally
            {

            }
        }

        [HttpPost]
        [Route("UpdateCPConnector")]
        [SwaggerResponse(200, "Success.", typeof(ResponseModelBase))]
        [SwaggerResponse(400, "Invalid parameter(s).", typeof(ErrorResponseModel))]
        [SwaggerResponse(500, "Internal server error.", typeof(ErrorResponseModel))]
        public async Task<ActionResult> UpdateCPConnector(UpdateCPConnectorRequestModel request)
        {
            StringBuilder logger = new StringBuilder();

            try
            {
                var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssffff");
                logger.Append($"UpdateCPConnector Timestamp {timestamp}");
                await _logger.LogInformation(logger.ToString());
                await _logger.LogInformation($"UpdateCPConnector: {JsonSerializer.Serialize(request)}");
                UpdateCPConnectorResponseModel response = _cpMonitoringService.UpdateCPConnector(request);

                return Ok(response);
            }
            catch (CustomValidationException ex)
            {
                logger.Append($"[Validation Exception] {ex.Key} {ex.Message}");
                await _logger.LogInformation(logger.ToString());
                ResponseModelBase response = new() { Timestamp = DateTime.Now.ToString("yyyyMMddHHmmssffff"), Success = false, StatusCode = SystemData.StatusCode.NotFound };
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                await _logger.LogError(logger.ToString(), ex);
                ResponseModelBase response = new() { Timestamp = DateTime.Now.ToString("yyyyMMddHHmmssffff"), Success = false, StatusCode = SystemData.StatusCode.Invalid };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            finally
            {

            }
        }

        [HttpDelete]
        [Route("DeleteCPConnector/{id}")]
        [SwaggerResponse(200, "Success.", typeof(ResponseModelBase))]
        [SwaggerResponse(400, "Invalid parameter(s).", typeof(ErrorResponseModel))]
        [SwaggerResponse(500, "Internal server error.", typeof(ErrorResponseModel))]
        public async Task<ActionResult> DeleteCPConnector([FromRoute]int id)
        {
            StringBuilder logger = new StringBuilder();

            try
            {
                var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssffff");
                logger.Append($"DeleteCPConnector Timestamp {timestamp}");
                await _logger.LogInformation(logger.ToString());
                await _logger.LogInformation($"DeleteCPConnector: {JsonSerializer.Serialize(id)}");
                ResponseModelBase response = await _cpMonitoringService.DeleteCPConnector(id);

                return Ok(response);
            }
            catch (CustomValidationException ex)
            {
                logger.Append($"[Validation Exception] {ex.Key} {ex.Message}");
                await _logger.LogInformation(logger.ToString());
                ResponseModelBase response = new() { Timestamp = DateTime.Now.ToString("yyyyMMddHHmmssffff"), Success = false, StatusCode = SystemData.StatusCode.NotFound };
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                await _logger.LogError(logger.ToString(), ex);
                ResponseModelBase response = new() { Timestamp = DateTime.Now.ToString("yyyyMMddHHmmssffff"), Success = false, StatusCode = SystemData.StatusCode.Invalid };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            finally
            {

            }
        }

        [HttpPost]
        [Route("UpdateCPDetails")]
        [SwaggerResponse(200, "Success.", typeof(ResponseModelBase))]
        [SwaggerResponse(400, "Invalid parameter(s).", typeof(ErrorResponseModel))]
        [SwaggerResponse(500, "Internal server error.", typeof(ErrorResponseModel))]
        public async Task<ActionResult> UpdateCPDetails(UpdateCPDetailsRequestModel request)
        {
            StringBuilder logger = new StringBuilder();

            try
            {
                var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssffff");
                logger.Append($"UpdateCPDetailsAsync Timestamp {timestamp}");
                await _logger.LogInformation(logger.ToString());
                await _logger.LogInformation($"UpdateCPDetailsAsync: {JsonSerializer.Serialize(request)}");
                UpdateCPDetailsResponseModel response = await _cpMonitoringService.UpdateCPDetailsAsync(request);

                return Ok(response);
            }
            catch (CustomValidationException ex)
            {
                logger.Append($"[Validation Exception] {ex.Key} {ex.Message}");
                await _logger.LogInformation(logger.ToString());
                ResponseModelBase response = new() { Timestamp = DateTime.Now.ToString("yyyyMMddHHmmssffff"), Success = false, StatusCode = SystemData.StatusCode.NotFound };
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                await _logger.LogError(logger.ToString(), ex);
                ResponseModelBase response = new() { Timestamp = DateTime.Now.ToString("yyyyMMddHHmmssffff"), Success = false, StatusCode = SystemData.StatusCode.Invalid };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            finally
            {

            }
        }

        [HttpDelete]
        [Route("DeleteCPDetails/{id}")]
        [SwaggerResponse(200, "Success.", typeof(ResponseModelBase))]
        [SwaggerResponse(400, "Invalid parameter(s).", typeof(ErrorResponseModel))]
        [SwaggerResponse(500, "Internal server error.", typeof(ErrorResponseModel))]
        public async Task<ActionResult> DeleteCPDetails([FromRoute]int id)
        {
            StringBuilder logger = new StringBuilder();

            try
            {
                var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssffff");
                logger.Append($"DeleteCPDetailsAsync Timestamp {timestamp}");
                await _logger.LogInformation(logger.ToString());
                await _logger.LogInformation($"DeleteCPDetailsAsync: {JsonSerializer.Serialize(id)}");
                ResponseModelBase response = await _cpMonitoringService.DeleteCPDetailsAsync(id);

                return Ok(response);
            }
            catch (CustomValidationException ex)
            {
                logger.Append($"[Validation Exception] {ex.Key} {ex.Message}");
                await _logger.LogInformation(logger.ToString());
                ResponseModelBase response = new() { Timestamp = DateTime.Now.ToString("yyyyMMddHHmmssffff"), Success = false, StatusCode = SystemData.StatusCode.NotFound };
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                await _logger.LogError(logger.ToString(), ex);
                ResponseModelBase response = new() { Timestamp = DateTime.Now.ToString("yyyyMMddHHmmssffff"), Success = false, StatusCode = SystemData.StatusCode.Invalid };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            finally
            {

            }
        }

        [HttpPost]
        [Route("LockUnlockCPConnector")]
        [SwaggerResponse(200, "Success.", typeof(ResponseModelBase))]
        [SwaggerResponse(400, "Invalid parameter(s).", typeof(ErrorResponseModel))]
        [SwaggerResponse(500, "Internal server error.", typeof(ErrorResponseModel))]
        public async Task<ActionResult> LockUnlockCPConnector(LockUnlockRequestModel request)
        {
            StringBuilder logger = new StringBuilder();

            try
            {
                var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssffff");
                logger.Append($"LockUnlockCPConnectorAsync Timestamp {timestamp}");
                await _logger.LogInformation(logger.ToString());
                await _logger.LogInformation($"LockUnlockCPConnectorAsync: {JsonSerializer.Serialize(request)}");
                ResponseModelBase response = await _cpMonitoringService.LockUnlockCPConnectorAsync(request);

                return Ok(response);
            }
            catch (CustomValidationException ex)
            {
                logger.Append($"[Validation Exception] {ex.Key} {ex.Message}");
                await _logger.LogInformation(logger.ToString());
                ResponseModelBase response = new() { Timestamp = DateTime.Now.ToString("yyyyMMddHHmmssffff"), Success = false, StatusCode = SystemData.StatusCode.NotFound };
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                await _logger.LogError(logger.ToString(), ex);
                ResponseModelBase response = new() { Timestamp = DateTime.Now.ToString("yyyyMMddHHmmssffff"), Success = false, StatusCode = SystemData.StatusCode.Invalid };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            finally
            {

            }
        }
        #endregion
    }
}
