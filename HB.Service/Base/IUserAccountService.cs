using HB.Model;

namespace HB.Service
{
    public interface IUserAccountService
    {
        Task<UserAccountResponseModel> CreateNewUserAccountAsync(NewUserSignUpRequestModel model);
        Task<UserAccountResponseModel> LoginUserAccount(UserLoginRequestModel model);
    }
}