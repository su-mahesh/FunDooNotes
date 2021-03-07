using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonLayer.Model;
using CommonLayer.UserAccountException;
using RepositoryLayer.Interfaces;
using RepositoryLayer.UserModelContext;


namespace RepositoryLayer.Services
{
    public class UserRegistrationRL : IUserRegistrationRL<UserModel>
    {
        readonly UserModelDbContext userModelDbContext;


        public UserRegistrationRL(UserModelDbContext userModelDbContext)
        {
            this.userModelDbContext = userModelDbContext;
        }

        public bool IsPresent(string Email)
        {
            return userModelDbContext.Users.Any(u => u.Email == Email);
        }
        public bool Register(UserModel user)
        {
            if (!IsPresent(user.Email))
            {
                user.Password = new PasswordEncryption().EncryptPassword(user.Password);
                userModelDbContext.Users.Add(user);
                userModelDbContext.SaveChanges();
                return true;
            }
            else
            {
                throw new UserAccountException(UserAccountException.ExceptionType.EMAIL_ALREADY_EXIST, "email id already registered");
            }

        }

        public void Delete(UserModel entity)
        {
            throw new NotImplementedException();
        }

        public UserModel Get(long id)
        {
            return userModelDbContext.Users.FirstOrDefault(user => user.UserID == id);
        }

        public IEnumerable<UserModel> GetAll()
        {
            return userModelDbContext.Users.ToList();
        }

        public void Update(UserModel dbEntity, UserModel entity)
        {
            userModelDbContext.Users.Add(entity);
            userModelDbContext.SaveChanges();
        }

        public IEnumerable<UserModel> GetAllUsers()
        {
            return userModelDbContext.Users.ToList();
        }
    }
}
