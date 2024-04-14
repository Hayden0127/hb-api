using HB.Database.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HB.Utilities;
using HB.Model;
using HB.Database.DbModels;
using Strateq.Core.Utilities;

namespace HB.Service
{
    public class UserAccountService : IUserAccountService
    {
        #region Fields
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly IUserAccountAuthorizationTokenRepository _userAccountAuthorizationTokenRepository;
        private readonly ITokenService _tokenService;
        #endregion

        #region Ctor
        public UserAccountService(IUserAccountRepository userAccountRepository,
            IUserAccountAuthorizationTokenRepository userAccountAuthorizationTokenRepository,
            ITokenService tokenService)
        {
            _userAccountRepository = userAccountRepository ?? throw new Exception(nameof(userAccountRepository));
            _userAccountAuthorizationTokenRepository = userAccountAuthorizationTokenRepository ?? throw new Exception(nameof(userAccountAuthorizationTokenRepository));
            _tokenService = tokenService ?? throw new Exception(nameof(tokenService));
        }
        #endregion

        #region Methods

        public async Task<UserAccountResponseModel> CreateNewUserAccountAsync(NewUserSignUpRequestModel model)
        {
            UserAccountResponseModel returnModel = new UserAccountResponseModel();

            var emailExist = _userAccountRepository.ToQueryable()!.Any(x => x.Email == model.Email);
            if (emailExist)
            {
                returnModel.Success = false;
                returnModel.StatusCode = SystemData.StatusCode.Exist;
                return returnModel;
            }

            //generate salt & combine with password
            var salt = SaltEncryption.generateSalt();
            var saltHashPassword = SaltEncryption.hash(model.Password, salt);

            UserAccount newUser = new UserAccount()
            {
                Email = model.Email,
                FullName = model.FullName,
                Salt = salt,
                SaltHashPassword = saltHashPassword
            };
            newUser = await _userAccountRepository.AddAndSaveChangesAsync(newUser);

            UserAccountAuthorizationToken newToken = new UserAccountAuthorizationToken()
            {
                UserAccountId = newUser.Id,
                RefreshToken = string.Empty,
            };
            await _userAccountAuthorizationTokenRepository.AddAndSaveChangesAsync(newToken);

            returnModel.Success = true;
            returnModel.StatusCode = SystemData.StatusCode.Success;
            returnModel.Email = newUser.Email;
            returnModel.FullName = newUser.FullName;
            returnModel.AccessToken = _tokenService.CreateAccessToken(newUser.Id);

            return returnModel;
        }


        public async Task<UserAccountResponseModel> LoginUserAccount(UserLoginRequestModel model)
        {
            UserAccountResponseModel returnModel = new UserAccountResponseModel();

            var existUser = _userAccountRepository.ToQueryable()!.Where(x => x.Email == model.Email).FirstOrDefault();
            if (existUser == null)
            {
                returnModel.Success = false;
                returnModel.StatusCode = SystemData.StatusCode.LoginFail;
                return returnModel;
            }

            //to compare credentials
            var hashedPassword = SaltEncryption.hash(model.Password, existUser.Salt);
            var matchPassword = SaltEncryption.Equals(hashedPassword, existUser.SaltHashPassword);

            if (!matchPassword)
            {
                returnModel.Success = false;
                returnModel.StatusCode = SystemData.StatusCode.LoginFail;
                return returnModel;
            }

            var updateToken = _userAccountAuthorizationTokenRepository.ToQueryable().Where(x => x.UserAccountId == existUser.Id).FirstOrDefault();
            await _tokenService.CreateRefreshToken(updateToken!);

            returnModel.Success = true;
            returnModel.StatusCode = SystemData.StatusCode.Success;
            returnModel.Email = existUser.Email;
            returnModel.FullName = existUser.FullName;
            returnModel.AccessToken = _tokenService.CreateAccessToken(existUser.Id);

            return returnModel;
        }


        #endregion
    }
}
