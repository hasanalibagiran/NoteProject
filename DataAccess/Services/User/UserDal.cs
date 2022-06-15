using System.Linq;
using System.Linq.Expressions;
using Core.Repositories;
using DataAccess.Repositories;
using Entities;

namespace DataAccess.Services
{
    public class UserDal : GenericRepository<User>, IUserDal
    {
        public UserDal(AppDbContext context) : base(context)
        {
        }
    }
}