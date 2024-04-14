using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using HB.Model;
using HB.Service;
using Strateq.Core.API.Filter;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Strateq.Core.Service;
using Microsoft.AspNetCore.Http;

namespace HB.API.Controllers
{
    [ApiController]
    [ModelStateValidationFilter]
    [Route("[controller]")]
    public class TokenController : Controller
    {
        private readonly ITokenService _tokenService;
        private ISystemLogService _logger;

        public TokenController(ITokenService tokenService,
            ISystemLogService logger)
        {
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _logger = logger;
        }

        [Route("refresh")]
        [HttpPost]
        public ActionResult Refresh(RefreshTokenModel model)
        {
            var principal = _tokenService.CheckPrincipalFromToken(model.AccessToken);
            if (principal == null)
                return BadRequest("Invalid Token.");
            var userId = int.Parse(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "");

            var userToken = _tokenService.ValidateRefreshToken(userId, model.RefreshToken);
            if (userToken == null)
                return BadRequest("Invalid Token.");

            var newJwtToken = _tokenService.CreateAccessToken(principal.Claims);
            var newRefreshToken = _tokenService.CreateRefreshToken(userToken).Result;

            var tokenResult = new RefreshResponseModel
            {
                AccessToken = newJwtToken,
                RefreshToken = newRefreshToken
            };

            return Ok(tokenResult);
        }

        [Route("revoke")]
        [HttpPost]
        public ActionResult Revoke(string token)
        {
            var principal = _tokenService.CheckPrincipalFromToken(token);
            if (principal == null)
                return BadRequest("Invalid Token.");
            var userId = int.Parse(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value??"");

            var userToken = _tokenService.GetTokenFromId(userId);
            if (userToken == null)
                return BadRequest("Invalid Token.");

            userToken.RefreshToken = string.Empty;
            _tokenService.UpdateAccountToken(userToken);

            return NoContent();
        }
    }
}
