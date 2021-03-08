using System;
using System.Collections.Generic;
using System.Text;
using BusinessLayer.Interfaces;
using CommonLayer.Model;
using RepositoryLayer.Interfaces;

namespace BusinessLayer.Services
{
    public class UserRegistrationBL : IUserRegistrationBL
    {
        IUserRegistrationRL<UserModel> userRegistrationsRL;

        public UserRegistrationBL(IUserRegistrationRL<UserModel> userRegistrationsRL)
        {
            this.userRegistrationsRL = userRegistrationsRL;
        }
        public bool AuthenticateUser(UserModel user)
        {
            return userRegistrationsRL.AuthenticateUser(user);
        }

        public bool RegisterUser(UserModel userModel)
        {
            return userRegistrationsRL.Register(userModel);
        }
    }
}
