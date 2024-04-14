using HB.Model;
using HB.Service;
using HB.Utilities;
using HB.SmartSD.Integrator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Strateq.Core.API.Controllers;
using Strateq.Core.API.Exceptions;
using Strateq.Core.Model;
using Strateq.Core.Service;
using Swashbuckle.AspNetCore.Annotations;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System;
using Websocket.Client;
using HB.Database.DbModels;
using System.Data;
using System.Security.Claims;

namespace HB.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]

    public class CPPricingController : STQControllerBase
    {

        #region Fields
        private readonly ICPPricingService _cpPricingService;
        private readonly ISystemLogService _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region Ctor
        public CPPricingController(ICPPricingService cpPricingService,
            ISystemLogService logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _cpPricingService = cpPricingService;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region Methods
        [HttpPost]
        [Route("GetAllChargePointPricingPagination")]
        [SwaggerResponse(200, "Success.", typeof(ResponseModelBase))]
        [SwaggerResponse(400, "Invalid parameter(s).", typeof(ErrorResponseModel))]
        [SwaggerResponse(500, "Internal server error.", typeof(ErrorResponseModel))]
        public async Task<ActionResult> GetAllChargePointPricingPagination(SearchCPPricingPlanRequestModel request)
        {
            StringBuilder logger = new StringBuilder();
            try
            {
                var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssffff");
                logger.Append($"GetAllChargePointPricingPagination Request Timestamp {timestamp}");

                if (_httpContextAccessor.HttpContext == null) throw new Exception("Unfound HttpContext");
                var currentUserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if (currentUserId == null) throw new CustomValidationException(SystemData.ErrorCode.Validation, $"User {SystemData.CustomValidation.NotFound}");

                request.UserAccountId = int.Parse(currentUserId.Value);
                var pricingPlanList = _cpPricingService.GetChargePointPricingPaginationListView(request);
                return Ok(pricingPlanList);
            }
            catch (CustomValidationException ex)
            {
                logger.Append($"[Validation Exception] {ex.Key} {ex.Message}");
                await _logger.LogInformation(logger.ToString());
                ErrorResponseModel response = new() { Code = SystemData.ErrorCode.Validation, Message = ex.Key + ' ' + ex.Message };
                await _logger.LogInformation(logger.ToString());
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                await _logger.LogError("Error - ", ex);
                ErrorResponseModel response = new() { Code = Utilities.SystemData.ErrorCode.InternalServer, Message = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            finally
            {

            }
        }

        [HttpPost]
        [Route("UpdateCPPricing")]
        [SwaggerResponse(200, "Success.", typeof(ResponseModelBase))]
        [SwaggerResponse(400, "Invalid parameter(s).", typeof(ErrorResponseModel))]
        [SwaggerResponse(500, "Internal server error.", typeof(ErrorResponseModel))]
        public async Task<ActionResult> UpdateCPPricing(UpdateCPPricingRequestModel request)
        {
            StringBuilder logger = new StringBuilder();
            try
            {
                var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssffff");
                logger.Append($"UpdateCPPricingAsync Request Timestamp {timestamp}");
                var updatedConnector = await _cpPricingService.UpdateCPPricingAsync(request);
                await _logger.LogInformation(logger.ToString());
                return Ok(updatedConnector);
            }
            catch (CustomValidationException ex)
            {
                logger.Append($"[Validation Exception] {ex.Key} {ex.Message}");
                await _logger.LogInformation(logger.ToString());
                ErrorResponseModel response = new() { Code = SystemData.ErrorCode.Validation, Message = ex.Key + ' ' + ex.Message };
                return BadRequest(response);
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
        [Route("RemoveCPPricing/{id}")]
        [SwaggerResponse(200, "Success.", typeof(ResponseModelBase))]
        [SwaggerResponse(400, "Invalid parameter(s).", typeof(ErrorResponseModel))]
        [SwaggerResponse(500, "Internal server error.", typeof(ErrorResponseModel))]
        public async Task<ActionResult> RemoveCPPricing([FromRoute] int id)
        {
            StringBuilder logger = new StringBuilder();
            try
            {
                var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssffff");
                logger.Append($"RemoveCPPricingAsync Request Timestamp {timestamp}");
                var updatedConnector = await _cpPricingService.RemoveCPPricingAsync(id);
                await _logger.LogInformation(logger.ToString());
                return Ok(updatedConnector);
            }
            catch (CustomValidationException ex)
            {
                logger.Append($"[Validation Exception] {ex.Key} {ex.Message}");
                await _logger.LogInformation(logger.ToString());
                ErrorResponseModel response = new() { Code = SystemData.ErrorCode.Validation, Message = ex.Key + ' ' + ex.Message };
                return BadRequest(response);
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
        [Route("GetAllPricingPlanListingPagination")]
        [SwaggerResponse(200, "Success.", typeof(ResponseModelBase))]
        [SwaggerResponse(400, "Invalid parameter(s).", typeof(ErrorResponseModel))]
        [SwaggerResponse(500, "Internal server error.", typeof(ErrorResponseModel))]
        public async Task<ActionResult> GetAllPricingPlanListingPagination(SearchPricingRequestModel request)
        {
            StringBuilder logger = new StringBuilder();
            try
            {
                var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssffff");
                logger.Append($"GetAllPricingPlanListingPagination Request Timestamp {timestamp}");

                if (_httpContextAccessor.HttpContext == null) throw new Exception("Unfound HttpContext");
                var currentUserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if (currentUserId == null) throw new CustomValidationException(SystemData.ErrorCode.Validation, $"User {SystemData.CustomValidation.NotFound}");

                request.UserAccountId = int.Parse(currentUserId.Value);
                var pricingPlanList = _cpPricingService.GetAllPricingPlanPaginationListView(request);
                await _logger.LogInformation(logger.ToString());
                return Ok(pricingPlanList);
            }
            catch (CustomValidationException ex)
            {
                logger.Append($"[Validation Exception] {ex.Key} {ex.Message}");
                await _logger.LogInformation(logger.ToString());
                ErrorResponseModel response = new() { Code = SystemData.ErrorCode.Validation, Message = ex.Key + ' ' + ex.Message };
                return BadRequest(response);
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
        [Route("GenerateChargePointPricingCSV")]
        [SwaggerResponse(200, "Success.", typeof(ResponseModelBase))]
        [SwaggerResponse(400, "Invalid parameter(s).", typeof(ErrorResponseModel))]
        [SwaggerResponse(500, "Internal server error.", typeof(ErrorResponseModel))]
        public async Task<ActionResult> GenerateChargePointPricingCSV(SearchCPPricingPlanRequestModel request)
        {
            StringBuilder logger = new StringBuilder();
            try
            {
                var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssffff");
                logger.Append($"GenerateChargePointPricingCSV Request Timestamp {timestamp}");
                string fileName = "ChargePointPricing.csv";

                if (_httpContextAccessor.HttpContext == null) throw new Exception("Unfound HttpContext");
                var currentUserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if (currentUserId == null) throw new CustomValidationException(SystemData.ErrorCode.Validation, $"User {SystemData.CustomValidation.NotFound}");

                request.UserAccountId = int.Parse(currentUserId.Value);
                var bytes = Encoding.UTF8.GetBytes(_cpPricingService.ChargePointPricingCSVString(request));
                await _logger.LogInformation(logger.ToString());
                return File(bytes, "text/csv", fileName);

            }
            catch (CustomValidationException ex)
            {
                logger.Append($"[Validation Exception] {ex.Key} {ex.Message}");
                await _logger.LogInformation(logger.ToString());
                ErrorResponseModel response = new() { Code = SystemData.ErrorCode.Validation, Message = ex.Key + ' ' + ex.Message };
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                await _logger.LogError("Error - ", ex);
                ErrorResponseModel response = new() { Code = Utilities.SystemData.ErrorCode.InternalServer, Message = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            finally
            {

            }
        }

        [HttpPost]
        [Route("CreateUpdatePricingPlan")]
        public async Task<ActionResult> CreateUpdatePricingPlan(CreateUpdatePricingPlanRequestModel request)
        {
            StringBuilder logger = new StringBuilder();

            try
            {
                logger.Append($"CreateUpdatePricingPlan Request Timestamp {request.Timestamp}");

                if (_httpContextAccessor.HttpContext == null) throw new Exception("Unfound HttpContext");
                var currentUserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if (currentUserId == null) throw new CustomValidationException(SystemData.ErrorCode.Validation, $"User {SystemData.CustomValidation.NotFound}");

                request.UserAccountId = int.Parse(currentUserId.Value);
                var pricingPlan = await _cpPricingService.CreateUpdatePricingPlanAsync(request);
                await _logger.LogInformation(logger.ToString());
                return Ok(pricingPlan);

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
        [Route("DeletePricingPlan/{id}")]
        [SwaggerResponse(200, "Success.", typeof(ResponseModelBase))]
        [SwaggerResponse(400, "Invalid parameter(s).", typeof(ErrorResponseModel))]
        [SwaggerResponse(500, "Internal server error.", typeof(ErrorResponseModel))]
        public async Task<ActionResult> DeletePricingPlan([FromRoute] int id)
        {
            StringBuilder logger = new StringBuilder();
            try
            {
                var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssffff");
                logger.Append($"DeletePricingPlan Request Timestamp {timestamp}");
                var response = await _cpPricingService.DeletePricingPlanAsync(id);
                await _logger.LogInformation(logger.ToString());
                return Ok(response);
            }
            catch (CustomValidationException ex)
            {
                logger.Append($"[Validation Exception] {ex.Key} {ex.Message}");
                await _logger.LogInformation(logger.ToString());
                ErrorResponseModel response = new() { Code = SystemData.ErrorCode.Validation, Message = ex.Key + ' ' + ex.Message };
                return BadRequest(response);
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
        [Route("GetAllPriceVaries")]
        [SwaggerResponse(200, "Success.", typeof(ResponseModelBase))]
        [SwaggerResponse(400, "Invalid parameter(s).", typeof(ErrorResponseModel))]
        [SwaggerResponse(500, "Internal server error.", typeof(ErrorResponseModel))]
        public async Task<ActionResult> GetAllPriceVaries()
        {
            StringBuilder logger = new StringBuilder();
            try
            {
                var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssffff");
                logger.Append($"GetAllPriceVaries Request Timestamp {timestamp}");
                var response = await _cpPricingService.GetAllPriceVariesAsync();
                await _logger.LogInformation(logger.ToString());
                return Ok(response);
            }
            catch (CustomValidationException ex)
            {
                logger.Append($"[Validation Exception] {ex.Key} {ex.Message}");
                await _logger.LogInformation(logger.ToString());
                ErrorResponseModel response = new() { Code = SystemData.ErrorCode.Validation, Message = ex.Key + ' ' + ex.Message };
                return BadRequest(response);
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
        [Route("GetAllUnit")]
        [SwaggerResponse(200, "Success.", typeof(ResponseModelBase))]
        [SwaggerResponse(400, "Invalid parameter(s).", typeof(ErrorResponseModel))]
        [SwaggerResponse(500, "Internal server error.", typeof(ErrorResponseModel))]
        public async Task<ActionResult> GetAllUnit()
        {
            StringBuilder logger = new StringBuilder();
            try
            {
                var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssffff");
                logger.Append($"GetAllUnit Request Timestamp {timestamp}");
                var response = await _cpPricingService.GetAllUnitAsync();
                await _logger.LogInformation(logger.ToString());
                return Ok(response);
            }
            catch (CustomValidationException ex)
            {
                logger.Append($"[Validation Exception] {ex.Key} {ex.Message}");
                await _logger.LogInformation(logger.ToString());
                ErrorResponseModel response = new() { Code = SystemData.ErrorCode.Validation, Message = ex.Key + ' ' + ex.Message };
                return BadRequest(response);
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
        [Route("GetAllPricingPlan")]
        [SwaggerResponse(200, "Success.", typeof(ResponseModelBase))]
        [SwaggerResponse(400, "Invalid parameter(s).", typeof(ErrorResponseModel))]
        [SwaggerResponse(500, "Internal server error.", typeof(ErrorResponseModel))]
        public async Task<ActionResult> GetAllPricingPlan()
        {
            StringBuilder logger = new StringBuilder();
            try
            {
                var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssffff");
                logger.Append($"GetAllPricingPlan Request Timestamp {timestamp}");

                if (_httpContextAccessor.HttpContext == null) throw new Exception("Unfound HttpContext");
                var currentUserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if (currentUserId == null) throw new CustomValidationException(SystemData.ErrorCode.Validation, $"User {SystemData.CustomValidation.NotFound}");

                var pricingPlanList = _cpPricingService.GetAllPricingPlanByUserAccountId(int.Parse(currentUserId.Value));
                await _logger.LogInformation(logger.ToString());
                return Ok(pricingPlanList);
            }
            catch (CustomValidationException ex)
            {
                logger.Append($"[Validation Exception] {ex.Key} {ex.Message}");
                await _logger.LogInformation(logger.ToString());
                ErrorResponseModel response = new() { Code = SystemData.ErrorCode.Validation, Message = ex.Key + ' ' + ex.Message };
                return BadRequest(response);
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
        [Route("GetPricingPlanById/{id}")]
        [SwaggerResponse(200, "Success.", typeof(ResponseModelBase))]
        [SwaggerResponse(400, "Invalid parameter(s).", typeof(ErrorResponseModel))]
        [SwaggerResponse(500, "Internal server error.", typeof(ErrorResponseModel))]
        public async Task<ActionResult> GetPricingPlanById([FromRoute] int id)
        {
            StringBuilder logger = new StringBuilder();
            try
            {
                var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssffff");
                logger.Append($"GetPricingPlanById Request Timestamp {timestamp}");

                var pricingPlan = _cpPricingService.GetAllPricingPlanBId(id);
                await _logger.LogInformation(logger.ToString());
                return Ok(pricingPlan);
            }
            catch (CustomValidationException ex)
            {
                logger.Append($"[Validation Exception] {ex.Key} {ex.Message}");
                await _logger.LogInformation(logger.ToString());
                ErrorResponseModel response = new() { Code = SystemData.ErrorCode.Validation, Message = ex.Key + ' ' + ex.Message };
                return BadRequest(response);
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

        #endregion
    }
}
