using EShop.Entities;

namespace EShop.Services.Contracts;

public interface IGenericService<TEntity> where TEntity : BaseEntity
{
    void Add(TEntity entity);

    Task AddAsync(TEntity entity);

    void Update(TEntity entity);

    void Remove(TEntity entity);

    void Remove(int id);

    TEntity FindById(int id);

    Task<TEntity> FindByIdAsync(int id);

    List<TEntity> GetAll();

    Task<List<TEntity>> GetAllAsync();
}
