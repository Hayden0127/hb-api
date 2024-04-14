
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
using System.Text;
using System.Threading.Tasks;

namespace HB.API.Controllers
{
    [Route("[controller]")]
    [ApiController]

    public class UserProfileController : STQControllerBase
    {
        #region Fields
        private readonly ISystemLogService _logger;
        private readonly IUserAccountService _userAccountService;
        #endregion

        #region Ctor
        public UserProfileController(
            ISystemLogService logger,
            IUserAccountService userAccountService)
        {
            _logger = logger;
            _userAccountService = userAccountService;
        }
        #endregion

        #region Methods

        [HttpPost]
        [AllowAnonymous]
        [Route("UserSignUp")]
        [SwaggerResponse(200, "Success.", typeof(ResponseModelBase))]
        [SwaggerResponse(400, "Invalid parameter(s).", typeof(ErrorResponseModel))]
        [SwaggerResponse(500, "Internal server error.", typeof(ErrorResponseModel))]
        public async Task<ActionResult> UserSignUp([FromBody, SwaggerRequestBody("Request body", Required = true)] NewUserSignUpRequestModel request)
        {
            StringBuilder logger = new StringBuilder();
            try
            {
                logger.Append($"UserSignUp Request Timestamp {request.Timestamp}");
                await _logger.LogInformation(logger.ToString());
                var response = await _userAccountService.CreateNewUserAccountAsync(request);
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
        [AllowAnonymous]
        [Route("UserLogin")]
        [SwaggerResponse(200, "Success.", typeof(ResponseModelBase))]
        [SwaggerResponse(400, "Invalid parameter(s).", typeof(ErrorResponseModel))]
        [SwaggerResponse(500, "Internal server error.", typeof(ErrorResponseModel))]
        public async Task<ActionResult> UserLogin([FromBody, SwaggerRequestBody("Request body", Required = true)] UserLoginRequestModel request)
        {
            StringBuilder logger = new StringBuilder();
            try
            {
                logger.Append($"UserLogin Request Timestamp {request.Timestamp}");
                await _logger.LogInformation(logger.ToString());
                var response = await _userAccountService.LoginUserAccount(request);
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
