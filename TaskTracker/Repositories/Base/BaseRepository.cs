using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TaskTracker.Models.Base;

namespace TaskTracker.Repositories.Base;


public class BaseRepository<TModel> : IBaseRepository<TModel> where TModel: BaseModel
{
    protected ApplicationDbContext _db;
    
    public BaseRepository(ApplicationDbContext db)
    {
       _db = db;
    }

    public DbSet<TModel> Entity
    {
        get => _db.Set<TModel>();
    }
    
    public async Task<List<TModel>> GetList()
    {
        return await Entity.ToListAsync();
    }
    
    public async Task<EntityEntry<TModel>> Create(TModel modelData)
    {
        modelData.Id = Guid.NewGuid().ToString();
        var instance = await Entity.AddAsync(modelData);
        SaveChanges();
        return instance;
    }
    
    public async Task<TModel?> Get(string Id)
    {
        return await Entity.FindAsync(Id);
    }

    public async void Delete(string Id)
    {
        TModel? instance = await Get(Id);
        if (instance is null) return;
        Entity.Remove(instance);
        SaveChanges();
    }

    public async  void SaveChanges()
    {
        await _db.SaveChangesAsync();
    }

}