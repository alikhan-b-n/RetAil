using RetAil.Dal.Entities;

namespace RetAil.Dal.Providers.Abstract;

public interface ICrudProvider<TEntity> where TEntity : BaseEntity
{
    
    Task Create(TEntity entity);
    Task<TEntity> GetById(Guid id);
    Task<List<TEntity>> GetAll();
    Task Update(TEntity Entity);
    Task Delete(Guid id);

}