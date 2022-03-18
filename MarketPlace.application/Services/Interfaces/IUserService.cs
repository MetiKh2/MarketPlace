using MarketPlace.dataLayer.DTOs.Account;
using MarketPlace.dataLayer.Entities.Account;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.application.Services.Interfaces
{
   public interface IUserService:IAsyncDisposable
    {
        #region Account
        Task<RegisterUserResult> RegisterUser(RegisterUserDto register);
        Task<bool> IsUserExistByEmail(string email);
        Task<LoginUserResult> LoginUser(LoginUserDto login);
        Task<User> GetUserByEmail(string email);
        Task<ForgotPasswordResult> RecoverUserPassword(ForgotPasswordDto forgot);
        Task<string> GetEmailActiveCodeByEmail(string email);
        Task<bool> ConfirmUserEmail(string email,string activeCode);
        Task<ChangePasswordResult> ChangePassword(ChangePasswordDto change,long userId);
        Task<EditUserProfileDto> GetUserProfileForEdit(long userId);
        Task<EditUserProfileResult> EditUserProfile(EditUserProfileDto profile,long userId,IFormFile avatarImage);
        Task<string> GetUserFullName(long userId);
        #endregion
    }
}
