using System;
using System.Collections.Generic;
using System.Text;
using BusinessLayer.Interfaces;
using CommonLayer.Model;
using CommonLayer.RequestModel;
using RepositoryLayer.Interfaces;

namespace BusinessLayer.Services
{
    public class UserAccountBL : IUserAccountBL
    {
        IUserAccountRL<UserModel> userAccountRL;

        UserDetailValidation userDetailValidation;
        public UserAccountBL(IUserAccountRL<UserModel> userRegistrationsRL)
        {
            this.userAccountRL = userRegistrationsRL;
            userDetailValidation = new UserDetailValidation();
        }
        public UserModel AuthenticateUser(UserModel user)
        {            
            try
            {
                if (userDetailValidation.ValidateEmailAddress(user.Email) &&
                userDetailValidation.ValidatePassword(user.Password))
                {
                    return userAccountRL.AuthenticateUser(user);
                }
                else
                {
                    throw new UserDetailException(UserDetailException.ExceptionType.ENTERED_INVALID_USER_DETAILS, "user details are details");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public UserModel GetAuthorizedUser(string Email)
        {
            try
            {
                if (userDetailValidation.ValidateEmailAddress(Email))
                {
                    return userAccountRL.GetAuthorizedUser(Email);
                }
                else
                {
                    throw new UserDetailException(UserDetailException.ExceptionType.ENTERED_INVALID_USER_DETAILS, "user details are invalid");
                }
            }
            catch (Exception)
            {
                throw;
            } 
        }

        public UserModel RegisterUser(UserModel user)
        {
            try
            {
                if (userDetailValidation.ValidateFirstName(user.FirstName) &&
                userDetailValidation.ValidateLastName(user.LastName) &&
                userDetailValidation.ValidateEmailAddress(user.Email) &&
                userDetailValidation.ValidatePassword(user.Password))
                {
                    return userAccountRL.Register(user);
                }
                else
                {
                    throw new UserDetailException(UserDetailException.ExceptionType.ENTERED_INVALID_USER_DETAILS, "user details are invalid");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool ResetPassword(ResetPasswordModel user)
        {
            try
            {
                if (user.NewPassword.Equals(user.ConfirmNewPassword) &&
                    userDetailValidation.ValidatePassword(user.NewPassword))
                {
                    return userAccountRL.ResetPassword(user);
                }
                else
                {
                    throw new UserDetailException(UserDetailException.ExceptionType.CONFIRM_PASSWORD_DO_NO_MATCH, "New and comfirm password do not match");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
