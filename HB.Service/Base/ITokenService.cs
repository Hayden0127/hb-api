using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using HB.Database.DbModels;
using Strateq.Core.Service.Base;

namespace HB.Service
{
    public interface ITokenService : IBaseService
    {
        string CreateAccessToken(int UserId);
        string CreateAccessToken(IEnumerable<Claim> usersClaims);
        Task<string> CreateRefreshToken(UserAccountAuthorizationToken model);
        ClaimsPrincipal CheckPrincipalFromToken(string accessToken);
        UserAccountAuthorizationToken? ValidateRefreshToken(int userId, string refreshToken);
        UserAccountAuthorizationToken? GetTokenFromId(int userId);
        Task AddSaveAccountToken(UserAccountAuthorizationToken accToken);
        Task UpdateAccountToken(UserAccountAuthorizationToken accToken);
    }
}
