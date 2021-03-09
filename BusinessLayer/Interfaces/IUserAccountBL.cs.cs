using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Model;

namespace BusinessLayer.Interfaces
{
    public interface IUserAccountBL
    {
        public UserModel RegisterUser(UserModel userModel);
        public UserModel AuthenticateUser(UserModel user);
        UserModel GetAuthorizedUser(string email);
        bool ResetPassword(string userID, ResetPasswordModel resetPasswordModel);
    }
}
