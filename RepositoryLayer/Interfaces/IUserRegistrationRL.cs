using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Model;

namespace RepositoryLayer.Interfaces
{
    public interface IUserRegistrationRL<TEntity>
    {
        IEnumerable<TEntity> GetAll();
        TEntity Get(long id);
        bool Register(TEntity entity);
        void Update(TEntity dbEntity, TEntity entity);
        void Delete(TEntity entity);
        bool AuthenticateUser(UserModel user);
    }
}
