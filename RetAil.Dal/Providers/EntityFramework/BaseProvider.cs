using Microsoft.EntityFrameworkCore;
using RetAil.Dal.Entities;
using RetAil.Dal.Providers.Abstract;

namespace RetAil.Dal.Providers.EntityFramework
{
    public abstract class BaseProvider<TEntity> : ICrudProvider<TEntity> where TEntity :BaseEntity
    {
        private readonly ApplicationContext _applicationContext;
        private readonly DbSet<TEntity> _dbSet;

        public BaseProvider(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
            _dbSet=_applicationContext.Set<TEntity>();
        }
        

        public async Task Create(TEntity entity)
        {
            _dbSet.Add(entity);
            await _applicationContext.SaveChangesAsync();        }

        public async Task<TEntity> GetById(Guid id)
        {
            return await _dbSet.FirstAsync(x=>x.Id==id);
        }

        public async Task<List<TEntity>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }
        

        public async Task Update(TEntity entity)
        { 
            _applicationContext.Entry(entity).State=EntityState.Modified;
             await _applicationContext.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
             _dbSet.Remove(_dbSet.First(x => x.Id == id));
             await _applicationContext.SaveChangesAsync();

        }
    }
}