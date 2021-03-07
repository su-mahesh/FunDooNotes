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
        public bool LoggingUser(UserModel user)
        {
            return userRegistrationsRL.LoggingUser(user);
        }
        public IEnumerable<UserModel> GetAllUsers()
        {
            return userRegistrationsRL.GetAllUsers();
        }

        public bool RegisterUser(UserModel userModel)
        {
            return userRegistrationsRL.Register(userModel);
        }
    }
}
