using DataAccess.Repositories;
using Entities;

namespace DataAccess.Services
{
    public class CategoryDal : GenericRepository<Category>, ICategoryDal
    {
        public CategoryDal(AppDbContext context) : base(context)
        {
        }
    }
}