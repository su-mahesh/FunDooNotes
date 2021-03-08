using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Model;

namespace BusinessLayer.Interfaces
{
    public interface IUserRegistrationBL
    {
        public bool RegisterUser(UserModel userModel);
        public bool AuthenticateUser(UserModel user);
    }
}
