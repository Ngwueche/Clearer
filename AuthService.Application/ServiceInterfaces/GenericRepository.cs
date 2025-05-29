using System.Linq.Expressions;
using AuthService.Domain.Entities;

namespace AuthService.Application.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntities
    {
        void Create(T entity);
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression,
        bool trackChanges);
        void Update(T entity);
        void Delete(T entity);

    }
}
