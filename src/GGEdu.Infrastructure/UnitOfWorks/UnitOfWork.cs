using GGEdu.Core.UnitOfWorks;
using GGEdu.Infrastructure.Context;

namespace GGEdu.Infrastructure.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GGEduContext _context;

        public UnitOfWork(GGEduContext context)
        {
            _context = context;
        }

        public int Commit()
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var saveCount = _context.SaveChanges();
                    transaction.Commit();
                    return saveCount;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var saveCount = await _context.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync();
                    return saveCount;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
    }
}
