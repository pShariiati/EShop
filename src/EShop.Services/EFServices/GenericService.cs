using EShop.DataLayer.Context;
using EShop.Entities;
using EShop.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace EShop.Services.EFServices;

public class GenericService<TEntity> : IGenericService<TEntity> where TEntity : BaseEntity, new()
{
    private readonly IUnitOfWork _uow;
    private readonly DbSet<TEntity> _entity;

    public GenericService(IUnitOfWork uow)
    {
        _uow = uow;
        _entity = uow.Set<TEntity>();
    }

    public void Add(TEntity entity)
        => _entity.Add(entity);

    public async Task AddAsync(TEntity entity)
        => await _entity.AddAsync(entity);

    public void Update(TEntity entity)
        => _entity.Update(entity);

    public void Remove(TEntity entity)
        => _entity.Remove(entity);

    public void Remove(int id)
    {
        //var tEntity = new TEntity() { Id = id };
        var tEntity = new TEntity();
        var idProperty = typeof(TEntity).GetProperty("Id");
        if (idProperty is null)
            throw new Exception("The entity doesn't have Id field!");
        idProperty.SetValue(tEntity, id, null);
        _uow.MarkAsDeleted(tEntity);
    }

    public TEntity FindById(int id)
        => _entity.Find(id);

    public async Task<TEntity> FindByIdAsync(int id)
        => await _entity.FindAsync(id);

    public List<TEntity> GetAll()
        => _entity.ToList();

    public async Task<List<TEntity>> GetAllAsync()
        => await _entity.ToListAsync();
}
