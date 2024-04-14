using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using HB.Model;
using HB.Service;
using HB.Utilities;
using Strateq.Core.API.Filter;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Strateq.Core.Service;
using Microsoft.AspNetCore.Http;
using System.Text;
using Strateq.Core.API.Exceptions;
using Strateq.Core.Model;

namespace HB.API.Controllers
{
    [ApiController]
    [ModelStateValidationFilter]
    [Route("[controller]")]
    public class ProductTypeController : Controller
    {
        private readonly IProductTypeService _productTypeService;
        private ISystemLogService _logger;

        public ProductTypeController(IProductTypeService productTypeService,
            ISystemLogService logger)
        {
            _productTypeService = productTypeService ?? throw new ArgumentNullException(nameof(productTypeService));
            _logger = logger;
        }

        [Route("GetAllProductType")]
        [HttpGet]
        public async Task<ActionResult> GetAllProductType()
        {
            StringBuilder logger = new StringBuilder();

            try
            {
                var requestTimestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssffff");
                logger.Append($"GetAllProductType Request Timestamp {requestTimestamp}");
                var response = _productTypeService.GetAllProductTypes();
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

    }
}
