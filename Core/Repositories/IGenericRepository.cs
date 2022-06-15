using System.Linq.Expressions;

namespace Core.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        public Task<T> GetAsync(Expression<Func<T, bool>> filter = null);
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null);
        Task AddAsync(T entity);
        Task AddAllAsync(List<T> entities);
        Task UpdateAll(List<T> entities);
        void Update(T entity);
        void Delete(T entity);
    }
}