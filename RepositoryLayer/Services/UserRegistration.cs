using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.ResponseModel;
using RepositoryLayer.Interfaces;
using RepositoryLayer.UserModelContext;

namespace RepositoryLayer.Services
{
    class UserRegistrationRL : IUserRegistrationRL<UserModel>
    {
        UserModelDbContext userModelDbContext;
        public void Add(UserModel entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(UserModel entity)
        {
            throw new NotImplementedException();
        }

        public UserModel Get(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(UserModel dbEntity, UserModel entity)
        {
            throw new NotImplementedException();
        }
    }
}
