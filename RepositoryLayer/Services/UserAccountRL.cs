using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonLayer.Model;
using CommonLayer.RequestModel;
using CommonLayer.UserAccountException;
using RepositoryLayer.Interfaces;
using RepositoryLayer.UserModelContext;

namespace RepositoryLayer.Services
{
    public class UserAccountRL : IUserAccountRL<UserModel>
    {
        readonly UserModelDbContext userModelDbContext;
        readonly PasswordEncryption passwordEncryption = new PasswordEncryption();

        public UserAccountRL(UserModelDbContext userModelDbContext)
        {
            this.userModelDbContext = userModelDbContext;
        }
        public UserModel AuthenticateUser(UserModel user)
        {
            string Password = passwordEncryption.EncryptPassword(user.Password);

            if (IsEmailPresent(user.Email))
            {
                if (userModelDbContext.Users.FirstOrDefault(u => u.Email == user.Email).Password == Password)
                {
                    UserModel u = userModelDbContext.Users.FirstOrDefault(u => u.Email == user.Email);
                    u.Password = null;
                    return u;
                }
                else
                    throw new UserAccountException(UserAccountException.ExceptionType.WRONG_PASSWORD, "password is wrong");
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
        public UserModel Register(UserModel user)
        {
            if (!IsEmailPresent(user.Email))
            {
                user.Password = new PasswordEncryption().EncryptPassword(user.Password);
                userModelDbContext.Users.Add(user);
                userModelDbContext.SaveChanges();
                return userModelDbContext.Users.Where(u => u.Email.Equals(user.Email)).Select(u => new UserModel
                {
                    UserID = u.UserID,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email
                }).ToList().First();
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

        public UserModel GetAuthorizedUser(string email)
        {
            return userModelDbContext.Users.FirstOrDefault(u => u.Email == email);
        }

        public bool ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            try
            {
                var NewPassword = passwordEncryption.EncryptPassword(resetPasswordModel.NewPassword);
                var user = userModelDbContext.Users.FirstOrDefault(u => u.Email.Equals(resetPasswordModel.Email));                
                if (user != null)
                {
                    user.Password = NewPassword;
                    userModelDbContext.Entry(user).Property(x => x.Password).IsModified = true;
                    userModelDbContext.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }
    }
}
