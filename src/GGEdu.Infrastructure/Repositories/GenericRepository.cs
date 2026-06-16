using GGEdu.Core.Entities;
using GGEdu.Core.Repositories;
using GGEdu.Infrastructure.Context;
using GGEdu.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GGEdu.Infrastructure.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly GGEduContext _ggEduContext;

        public GenericRepository(GGEduContext ggEduContext)
        {
            _ggEduContext = ggEduContext;
        }

        public void Create(TEntity entity)
        {
            _ggEduContext.Set<TEntity>().Add(entity);
        }

        public async Task CreateAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            await _ggEduContext.Set<TEntity>().AddAsync(entity, cancellationToken);

            if (autoSave)
            {
                await _ggEduContext.SaveChangesAsync(cancellationToken);
            }
        }

        public void CreateRange(List<TEntity> entities)
        {
            _ggEduContext.Set<TEntity>().AddRange(entities);
        }

        public async Task CreateRangeAsync(List<TEntity> entities, CancellationToken cancellationToken = default)
        {
            await _ggEduContext.Set<TEntity>().AddRangeAsync(entities, cancellationToken);
        }

        public void Delete(TEntity entity)
        {
            _ggEduContext.Remove(entity);
        }

        public void DeleteRange(List<TEntity> entities)
        {
            foreach (TEntity entity in entities)
            {
                _ggEduContext.Remove(entity);
            }
        }

        public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            // EF Core tracking updates (Remove) are synchronous; we only need async when hitting the DB (SaveChanges).
            // Wrapping Remove() in Task.Run is an anti-pattern (no IO benefit, and can cause threading issues).
            _ggEduContext.Remove(entity);
            await Task.CompletedTask;
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _ggEduContext.Set<TEntity>().ToList();
        }

        public IQueryable<TEntity> GetAllAsQueryable()
        {
            return _ggEduContext.Set<TEntity>().AsNoTracking();
        }

        public async Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _ggEduContext.Set<TEntity>().ToListAsync(cancellationToken);
        }

        public TEntity? GetBy(Expression<Func<TEntity, bool>> predicate)
        {
            return _ggEduContext.Set<TEntity>().FirstOrDefault(predicate);
        }

        public async Task<TEntity?> GetByAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _ggEduContext.Set<TEntity>().FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public TEntity? GetById(Guid id)
        {
            return _ggEduContext.Set<TEntity>().FirstOrDefault(c => c.Id.Equals(id));
        }

        public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _ggEduContext.Set<TEntity>().FirstOrDefaultAsync(c => c.Id.Equals(id), cancellationToken);
        }

        public IEnumerable<TEntity> GetListBy(Expression<Func<TEntity, bool>> predicate)
        {
            return _ggEduContext.Set<TEntity>().Where(predicate).ToList();
        }

        public async Task<IEnumerable<TEntity>> GetListByAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _ggEduContext.Set<TEntity>().Where(predicate).ToListAsync(cancellationToken);
        }

        public int Save()
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveAsync()
        {
            throw new NotImplementedException();
        }

        public void Update(TEntity entity)
        {
            _ggEduContext.Set<TEntity>().Update(entity);
        }

        public async Task UpdateAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            // EF Core tracking updates (Update) are synchronous; we only need async when hitting the DB (SaveChanges).
            // Wrapping Update() in Task.Run is an anti-pattern (no IO benefit, and can cause threading issues).
            _ggEduContext.Set<TEntity>().Update(entity);

            if (autoSave)
            {
                await _ggEduContext.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task LoadNavigationPropertyAsync<TProperty>(TEntity entity,
            Expression<Func<TEntity, TProperty?>> navigationProperty, CancellationToken cancellationToken = default)
            where TProperty : class
        {
            var navigationPropertyName = navigationProperty.GetPropertyName();

            var navigation = _ggEduContext.Entry(entity).Metadata
            .FindNavigation(navigationPropertyName);

            if (navigation?.IsCollection == true)
            {
                await _ggEduContext.Entry(entity)
                .Collection(navigation)
                .LoadAsync(cancellationToken);
            }
            else
            {
                await _ggEduContext.Entry(entity)
                .Reference(navigationProperty)
                .LoadAsync(cancellationToken);
            }

        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _ggEduContext.Set<TEntity>().AnyAsync(predicate, cancellationToken);
        }
    }
}