using Domain;

namespace Application.Abstraction
{
    public interface IRepository<T> where T : CustomerModel
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task<T> Get(string id);
        Task<T> Delete(string id);
    }
}
