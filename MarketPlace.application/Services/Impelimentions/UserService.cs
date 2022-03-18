using MarketPlace.application.Extensions;
using MarketPlace.application.Services.Interfaces;
using MarketPlace.application.Utils;
using MarketPlace.dataLayer.DTOs.Account;
using MarketPlace.dataLayer.Entities.Account;
using MarketPlace.dataLayer.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.application.Services.Impelimentions
{
    public class UserService : IUserService
    {
        #region Contructor
        private readonly IGenericRepository<User> _userRepository;
        private readonly IPasswordHelper _passwordHelper;
        private readonly IEmailSender _emailSender;
        public UserService(IEmailSender emailSender, IGenericRepository<User> userRepository, IPasswordHelper passwordHelper)
        {
            _emailSender = emailSender;
            _passwordHelper = passwordHelper;
            _userRepository = userRepository;
        }
        #endregion
        #region Dispose
        public async ValueTask DisposeAsync()
        {
            await _userRepository.DisposeAsync();
        }
        #endregion
        #region Account
        public async Task<RegisterUserResult> RegisterUser(RegisterUserDto register)
        {
            try
            {
                if (!await IsUserExistByEmail(register.Email))
                {
                    var user = new User
                    {
                        FirstName = register.FirstName,
                        LastName = register.LastName,
                        Email = register.Email,
                        Password = _passwordHelper.EncodePasswordMd5(register.Password),
                        MobileActiveCode = new Random().Next(10000, 99999).ToString(),
                        EmailActiveCode = Guid.NewGuid().ToString("N")
                    };
                    await _userRepository.AddEntity(user);
                    await _userRepository.SaveChangesAsync();
                    return RegisterUserResult.Success;
                }
                else return RegisterUserResult.EmailExist;
            }
            catch (Exception)
            {
                return RegisterUserResult.Error;
            }
            return RegisterUserResult.Error;


        }
        public async Task<bool> IsUserExistByEmail(string email)
        {
            return await _userRepository.GetQuery().AnyAsync(p => p.Email == email);
        }

        public async Task<LoginUserResult> LoginUser(LoginUserDto login)
        {
            var user = await GetUserByEmail(login.Email);
            if (user == null) return LoginUserResult.NotFound;
            if (!user.IsEmailActive) return LoginUserResult.NotActivated;
            if (user.Password != _passwordHelper.EncodePasswordMd5(login.Password)) return LoginUserResult.NotFound;
            return LoginUserResult.Success;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _userRepository.GetQuery().SingleOrDefaultAsync(p => p.Email == email);
        }

        public async Task<ForgotPasswordResult> RecoverUserPassword(ForgotPasswordDto forgot)
        {
            var user = await GetUserByEmail(forgot.Email);
            if (user == null) return ForgotPasswordResult.NotFound;
            var newPassword = new Random().Next(100000, 999999).ToString();
            user.Password = _passwordHelper.EncodePasswordMd5(newPassword);
            _userRepository.EditEntity(user);
            await _userRepository.SaveChangesAsync();

            await _emailSender.SendEmailAsync(forgot.Email, "بازگردانی رمز عبور", $"رمز عبور جدید شما :{newPassword}");
            return ForgotPasswordResult.Success;
        }

        public async Task<string> GetEmailActiveCodeByEmail(string email)
        {
            return await _userRepository.GetQuery().Where(p => p.Email == email).Select(p => p.EmailActiveCode).FirstOrDefaultAsync();
        }

        public async Task<bool> ConfirmUserEmail(string email, string activeCode)
        {
            var user = await GetUserByEmail(email);
            if (user == null) return false;
            if (user.IsEmailActive) return false;
            if (activeCode != user.EmailActiveCode) return false;
            user.IsEmailActive = true;
            user.EmailActiveCode = Guid.NewGuid().ToString("N");
            _userRepository.EditEntity(user);
            await _userRepository.SaveChangesAsync();
            return true;
        }

        public async Task<ChangePasswordResult> ChangePassword(ChangePasswordDto change, long userId)
        {
            User user = await _userRepository.GetEntityById(userId);
            if (user != null)
            {
                if (user.Password != _passwordHelper.EncodePasswordMd5(change.CurrentPassword)) return ChangePasswordResult.WrongCurrentPassword;
                user.Password = _passwordHelper.EncodePasswordMd5(change.NewPassword);
                _userRepository.EditEntity(user);
                await _userRepository.SaveChangesAsync();
                return ChangePasswordResult.Success;
            }
            return ChangePasswordResult.Error;
        }

        public async Task<EditUserProfileDto> GetUserProfileForEdit(long userId)
        {
            var user = await _userRepository.GetEntityById(userId);
            return new EditUserProfileDto { 
            FirstName=user.FirstName,
            LastName=user.LastName,
            Avatar=user.Avatar
            };
        }

        public async Task<EditUserProfileResult> EditUserProfile(EditUserProfileDto profilem,long userId,IFormFile avatarImage)
        {
            var user =await _userRepository.GetEntityById(userId);
            if (user == null||user.IsBlocked||!user.IsEmailActive) return EditUserProfileResult.NotFound;

            user.FirstName = profilem.FirstName;
            user.LastName = profilem.LastName;
            if (avatarImage != null && avatarImage.IsImage())
            {
                var imageName = Guid.NewGuid().ToString("N")+Path.GetExtension(avatarImage.FileName);
                avatarImage.AddImageToServer(imageName,PathExtension.UserAvatarOriginSever,100,100,PathExtension.UserAvatarThumbSever,user.Avatar);
                user.Avatar = imageName;
            }
            _userRepository.EditEntity(user);
            await _userRepository.SaveChangesAsync();
            return EditUserProfileResult.Success;

        }

        public async Task<string> GetUserFullName(long userId)
        {
            var user =await _userRepository.GetEntityById(userId);
            return user.FirstName + " " + user.LastName;
        }


        #endregion
    }
}
