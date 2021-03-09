using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Model;

namespace RepositoryLayer.Interfaces
{
    public interface IUserAccountRL<TEntity>
    {
        IEnumerable<TEntity> GetAll();
        TEntity Get(long id);
        UserModel Register(TEntity entity);
        void Update(TEntity dbEntity, TEntity entity);
        void Delete(TEntity entity);
        UserModel AuthenticateUser(UserModel user);
        UserModel GetAuthorizedUser(string email);
    }
}
