using Microsoft.EntityFrameworkCore.ChangeTracking;
using TodoApp.Models.Base;

namespace TodoApp.Repositories.Base;

public interface IBaseRepository<TModel> where TModel : BaseModel
{
    public Task<EntityEntry<TModel>> Create(TModel modelData);
    public void Delete(string Id);
    public  Task<List<TModel>> GetList();
    public Task<TModel> Get(string Id);

}
