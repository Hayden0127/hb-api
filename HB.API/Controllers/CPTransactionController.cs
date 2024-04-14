using HB.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Strateq.Core.API.Controllers;
using Strateq.Core.Model;
using Strateq.Core.Service;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Text;
using System.Threading.Tasks;
using HB.Utilities;
using Strateq.Core.API.Exceptions;
using Microsoft.AspNetCore.Http;
using HB.Service;
using HB.Database.DbModels;
using System.Collections.Generic;
using HB.Database.Repositories;
using System.Security.Claims;

namespace HB.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]

    public class CPTransactionController : STQControllerBase
    {
        #region Fields
        private readonly ISystemLogService _logger;
        private readonly ICPTransactionService _cpTransactionService;
        private readonly ICPTransactionRepository _cpTransactionRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region Ctor
        public CPTransactionController(
            ISystemLogService logger,
            ICPTransactionService cPTransactionService,
            ICPTransactionRepository cpTransactionRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _cpTransactionService = cPTransactionService;
            _cpTransactionRepository = cpTransactionRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region Methods

        [HttpGet]
        [Route("GetAllCPTransactionList")]
        [SwaggerResponse(200, "Success.", typeof(ResponseModelBase))]
        [SwaggerResponse(400, "Invalid parameter(s).", typeof(ErrorResponseModel))]
        [SwaggerResponse(500, "Internal server error.", typeof(ErrorResponseModel))]
        public async Task<ActionResult> GetAllCPTransactionList()
        {
            try
            {
                var dbCPTransactionList = await _cpTransactionRepository.GetAllAsync();
                return Ok(dbCPTransactionList);
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
        [Route("GetPaymentDashboardDetails")]
        [SwaggerResponse(200, "Success.", typeof(ResponseModelBase))]
        [SwaggerResponse(400, "Invalid parameter(s).", typeof(ErrorResponseModel))]
        [SwaggerResponse(500, "Internal server error.", typeof(ErrorResponseModel))]
        public async Task<ActionResult> GetPaymentDashboardDetails()
        {
            try
            {
                if (_httpContextAccessor.HttpContext == null) throw new Exception("Unfound HttpContext");
                var currentUserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if (currentUserId == null) throw new CustomValidationException(SystemData.ErrorCode.Validation, $"User {SystemData.CustomValidation.NotFound}");

                var dbPaymentDashboardDisplayModel = _cpTransactionService.GetPaymentDashboardDetails(int.Parse(currentUserId.Value));
                return Ok(dbPaymentDashboardDisplayModel);
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
