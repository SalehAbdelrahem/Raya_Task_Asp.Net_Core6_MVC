namespace DAL.Contacts
{
    public interface IRepository<TEntity, TId> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity?> GetByIdAsync(TId id);
        Task<TEntity> CreateAsync(TEntity item);
        Task<bool> UpdateAsync(TEntity item);
        Task<bool> DeleteAsync(TId id);
        Task<long> GetCountAsync();
    }
}
