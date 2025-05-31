using System.Linq.Expressions;
using AuthService.Domain.Entities;

namespace AuthService.Application.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntities
    {
        Task Create(T entity);
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression,
        bool trackChanges);
        Task Update(T entity);
        Task Delete(T entity);

    }
}
