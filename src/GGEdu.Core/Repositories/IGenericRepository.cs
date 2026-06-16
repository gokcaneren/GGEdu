using GGEdu.Core.Entities;
using System.Linq.Expressions;

namespace GGEdu.Core.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        IQueryable<TEntity> GetAllAsQueryable();
        IEnumerable<TEntity> GetAll();
        Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
        TEntity? GetById(Guid id);
        Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task CreateAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);
        Task CreateRangeAsync(List<TEntity> entities, CancellationToken cancellationToken = default);
        Task UpdateAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);
        void Delete(TEntity entity);
        void DeleteRange(List<TEntity> entities);
        void Create(TEntity entity);
        void CreateRange(List<TEntity> entities);
        void Update(TEntity entity);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
        TEntity? GetBy(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity?> GetByAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
        IEnumerable<TEntity> GetListBy(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> GetListByAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
        Task LoadNavigationPropertyAsync<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty?>> navigationProperty, CancellationToken cancellationToken = default) where TProperty : class;
    }
}
