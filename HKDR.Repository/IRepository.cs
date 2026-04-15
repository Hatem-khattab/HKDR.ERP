using System.Linq.Expressions;

namespace HKDR.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        // Create
        Task AddAsync(TEntity entity);
        Task AddRangeAsync(IEnumerable<TEntity> entities);

        // Read
        Task<TEntity?> GetByIdAsync(int id);
        Task<List<TEntity>> GetAllAsync();
        Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);

        // Update
        void Update(TEntity entity);

        // Delete
        void Remove(TEntity entity);
        Task RemoveByIdAsync(int id);

        // Exists / Count
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);

        // Save
        Task SaveAsync();
    }
}
