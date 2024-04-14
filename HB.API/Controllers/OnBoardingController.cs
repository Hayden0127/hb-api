
using HB.Model;
using HB.Service;
using HB.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Strateq.Core.API.Controllers;
using Strateq.Core.API.Exceptions;
using Strateq.Core.Model;
using Strateq.Core.Service;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HB.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]

    public class OnBoardingController : STQControllerBase
    {
        #region Fields
        private readonly ISystemLogService _logger;
        private readonly IOnBoardingService _onBoardingService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region Ctor
        public OnBoardingController(
            ISystemLogService logger,
            IOnBoardingService onBoardingService,
            IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _onBoardingService = onBoardingService; 
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region Methods


        [HttpGet]
        [Route("GetAllCPSiteListingByUserAccountId")]
        [SwaggerResponse(200, "Success.", typeof(ResponseModelBase))]
        [SwaggerResponse(400, "Invalid parameter(s).", typeof(ErrorResponseModel))]
        [SwaggerResponse(500, "Internal server error.", typeof(ErrorResponseModel))]
        public async Task<ActionResult> GetAllCPSiteListing()
        {
            StringBuilder logger = new StringBuilder();

            try
            {
                var requestTimestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssffff");
                logger.Append($"CreateNewOnBoarding Request Timestamp {requestTimestamp}");
                if (_httpContextAccessor.HttpContext == null) throw new Exception("Unfound HttpContext");
                var currentUserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if (currentUserId == null) throw new CustomValidationException(SystemData.ErrorCode.Validation, $"User {SystemData.CustomValidation.NotFound}");

                CPSiteDisplayListModel response = _onBoardingService.GetAllCPSiteListingByUserAccountId(int.Parse(currentUserId.Value));
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

        [HttpPost]
        [Route("CreateNewSiteOnBoarding")]
        [SwaggerResponse(200, "Success.", typeof(ResponseModelBase))]
        [SwaggerResponse(400, "Invalid parameter(s).", typeof(ErrorResponseModel))]
        [SwaggerResponse(500, "Internal server error.", typeof(ErrorResponseModel))]
        public async Task<ActionResult> CreateNewSiteOnBoarding([FromBody, SwaggerRequestBody("Request body", Required = true)] OnBoardingNewSiteRequestModel request)
        {
            StringBuilder logger = new StringBuilder();

            try
            {
                logger.Append($"CreateNewOnBoarding Request Timestamp {request.Timestamp}");
                if (_httpContextAccessor.HttpContext == null) throw new Exception("Unfound HttpContext");
                var currentUserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if (currentUserId == null) throw new CustomValidationException(SystemData.ErrorCode.Validation, $"User {SystemData.CustomValidation.NotFound}");

                request.UserAccountId = int.Parse(currentUserId.Value);
                NewSiteOnBoardingResponseModel response = await _onBoardingService.CreateNewSiteOnBoardingAsync(request);
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

        [HttpPost]
        [Route("ValidateCPNameAndSerialNo")]
        [SwaggerResponse(200, "Success.", typeof(ResponseModelBase))]
        [SwaggerResponse(400, "Invalid parameter(s).", typeof(ErrorResponseModel))]
        [SwaggerResponse(500, "Internal server error.", typeof(ErrorResponseModel))]
        public async Task<ActionResult> ValidateCPNameAndSerialNo([FromBody, SwaggerRequestBody("Request body", Required = true)] ValidateCPNameRequestModel request)//
        {
            StringBuilder logger = new StringBuilder();

            try
            {
                logger.Append($"CreateNewOnBoarding Request Timestamp {request.Timestamp}");
                ValidateCPNameResponseModel response = _onBoardingService.ValidateCPNameAndSerialNo(request);
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


        [HttpPost]
        [Route("CreateNewCPOnBoarding")]
        [SwaggerResponse(200, "Success.", typeof(ResponseModelBase))]
        [SwaggerResponse(400, "Invalid parameter(s).", typeof(ErrorResponseModel))]
        [SwaggerResponse(500, "Internal server error.", typeof(ErrorResponseModel))]
        public async Task<ActionResult> CreateNewCPOnBoarding([FromBody, SwaggerRequestBody("Request body", Required = true)] OnBoardingNewCPRequestModel request)
        {
            StringBuilder logger = new StringBuilder();

            try
            {
                logger.Append($"CreateNewOnBoarding Request Timestamp {request.Timestamp}");
                NewCPOnBoardingResponseModel response = await _onBoardingService.CreateNewCPOnBoardingAsync(request);
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


        #endregion

    }
}
