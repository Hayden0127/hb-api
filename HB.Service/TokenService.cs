using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Strateq.Core.API.Infrastructures.Jwt;
using HB.Database.DbModels;
using HB.Database.Repositories;

namespace HB.Service
{
    public class TokenService : ITokenService
    {
        private readonly IUserAccountAuthorizationTokenRepository _userAccountAuthorizationTokenRepository;
        private readonly IConfiguration _config;
        public TokenService(IUserAccountAuthorizationTokenRepository userAccountAuthorizationTokenRepository, IConfiguration config)
        {
            _userAccountAuthorizationTokenRepository = userAccountAuthorizationTokenRepository;
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public string CreateAccessToken(int UserId)
        {
            var usersClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, UserId.ToString())
            };

            JwtToken token = new JwtToken(_config);
            var accessToken = token.GenerateAccessToken(usersClaims);
            return accessToken;
        }

        public string CreateAccessToken(IEnumerable<Claim> usersClaims)
        {
            JwtToken token = new JwtToken(_config);
            var accessToken = token.GenerateAccessToken(usersClaims);
            return accessToken;
        }

        public async Task<string> CreateRefreshToken(UserAccountAuthorizationToken model)
        {
            JwtToken token = new JwtToken(_config);
            var refreshToken = token.GenerateRefreshToken();

            model.RefreshToken = refreshToken;
            await UpdateAccountToken(model);
            return refreshToken;
        }

        public ClaimsPrincipal CheckPrincipalFromToken(string accessToken)
        {
            JwtToken token = new JwtToken(_config);
            return token.GetPrincipalFromToken(accessToken);
        }

        public UserAccountAuthorizationToken? ValidateRefreshToken(int userId, string refreshToken)
        {
            var userToken = _userAccountAuthorizationTokenRepository.ToQueryable().Where(t => t.UserAccountId == userId && t.RefreshToken == refreshToken).FirstOrDefault();
            return userToken;
        }

        public UserAccountAuthorizationToken? GetTokenFromId(int userId)
        {
            var userToken = _userAccountAuthorizationTokenRepository.ToQueryable().Where(t => t.UserAccountId == userId).FirstOrDefault();
            return userToken;
        }

        public async Task AddSaveAccountToken(UserAccountAuthorizationToken accToken)
        {
            await _userAccountAuthorizationTokenRepository.AddAndSaveChangesAsync(accToken);
        }

        public async Task UpdateAccountToken(UserAccountAuthorizationToken accToken)
        {
            await _userAccountAuthorizationTokenRepository.UpdateAndSaveChangesAsync(accToken);
        }
    }
}
