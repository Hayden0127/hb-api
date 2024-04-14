
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

    public class BookingController : STQControllerBase
    {
        #region Fields
        private readonly ISystemLogService _logger;
        private readonly IBookingService _bookingService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region Ctor
        public BookingController(
            ISystemLogService logger,
            IBookingService bookingService,
            IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _bookingService = bookingService;
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region Methods

        [HttpPost]
        [AllowAnonymous]
        [Route("RoomBooking")]
        [SwaggerResponse(200, "Success.", typeof(ResponseModelBase))]
        [SwaggerResponse(400, "Invalid parameter(s).", typeof(ErrorResponseModel))]
        [SwaggerResponse(500, "Internal server error.", typeof(ErrorResponseModel))]
        public async Task<ActionResult> RoomBooking([FromBody, SwaggerRequestBody("Request body", Required = true)] BookingRequestModel request)
        {
            StringBuilder logger = new StringBuilder();
            try
            {
                logger.Append($"Room Booking Request Timestamp {request.Timestamp}");

                if (_httpContextAccessor.HttpContext == null) throw new Exception("Unfound HttpContext");
                var currentUserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if (currentUserId == null) throw new CustomValidationException(SystemData.ErrorCode.Validation, $"User {SystemData.CustomValidation.NotFound}");
                request.UserAccountId = int.Parse(currentUserId.Value);
                //request.UserAccountId = 1;

                await _logger.LogInformation(logger.ToString());
                var response = await _bookingService.CreateNewBookingAsync(request);

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
        [AllowAnonymous]
        [Route("GetAllBookingByUser")]
        [SwaggerResponse(200, "Success.", typeof(ResponseModelBase))]
        [SwaggerResponse(400, "Invalid parameter(s).", typeof(ErrorResponseModel))]
        [SwaggerResponse(500, "Internal server error.", typeof(ErrorResponseModel))]
        public async Task<ActionResult> GetAllBookingByUser()
        {
            try
            {
                if (_httpContextAccessor.HttpContext == null) throw new Exception("Unfound HttpContext");
                var currentUserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if (currentUserId == null) throw new CustomValidationException(SystemData.ErrorCode.Validation, $"User {SystemData.CustomValidation.NotFound}");

                BookingDisplayListResponseModel response = _bookingService.GetAllBookingByUser(int.Parse(currentUserId.Value));
                //BookingDisplayListResponseModel response = _bookingService.GetAllBookingByUser(1);
                return Ok(response);
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
