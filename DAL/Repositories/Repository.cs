using DAL.Contacts;
using DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class Repository<TEntity, TId> : IRepository<TEntity, TId> where TEntity : class
    {

        protected readonly AppDBContext _context;
        protected readonly DbSet<TEntity> _dbset;

        public Repository(AppDBContext context)
        {
            _context = context;
            _dbset = _context.Set<TEntity>();
        }
        public virtual async Task<TEntity?> GetByIdAsync(TId id)
        {
            var re = await _dbset.FindAsync(id);
            return re;
        }

        public async Task<TEntity> CreateAsync(TEntity item)
        {
            var Item = (await _dbset.AddAsync(item)).Entity;
            _context.SaveChanges();
            return Item;
        }
        public Task<bool> UpdateAsync(TEntity item)
        {
            var entity = _dbset.Update(item);
            _context.SaveChanges();
            if (entity != null)
            {
                return Task.FromResult(true);
            }
            else
            {
                return Task.FromResult(false);
            }
        }
        public async Task<bool> DeleteAsync(TId id)
        {
            var item = await GetByIdAsync(id);
            if (item != null)
            {
                _dbset.Remove(item);
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }

        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {

            return await Task.FromResult(_dbset);

        }

        public async Task<long> GetCountAsync()
        {
            return await Task.FromResult(_dbset.Count());
        }
    }
}
