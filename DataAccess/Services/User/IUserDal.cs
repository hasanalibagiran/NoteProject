using System.Linq.Expressions;
using Core.Repositories;
using Entities;

namespace DataAccess.Services
{
    public interface IUserDal: IGenericRepository<User>
    {

    }
}