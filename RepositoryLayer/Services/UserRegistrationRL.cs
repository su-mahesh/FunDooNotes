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
        PasswordEncryption passwordEncryption = new PasswordEncryption();

        public UserRegistrationRL(UserModelDbContext userModelDbContext)
        {
            this.userModelDbContext = userModelDbContext;
        }
        public bool AuthenticateUser(UserModel user)
        {            
            if (IsEmailPresent(user.Email))
            {
                string Password = passwordEncryption.EncryptPassword(user.Password);
                return userModelDbContext.Users.FirstOrDefault(u => u.Email == user.Email).Password == Password;
            }
            else
            {
                throw new UserAccountException(UserAccountException.ExceptionType.EMAIL_DONT_EXIST, "email address is not registered");
            }
        }

        public bool IsEmailPresent(string Email)
        {
            return userModelDbContext.Users.Any(u => u.Email == Email);
        }
        public bool Register(UserModel user)
        {
            if (!IsEmailPresent(user.Email))
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
    }
}
