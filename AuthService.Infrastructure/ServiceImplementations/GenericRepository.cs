using System.Linq.Expressions;
using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.ServiceImplementations
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntities
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public void Create(T entity) => _dbSet.Add(entity);

        public void Update(T entity) => _dbSet.Update(entity);

        public void Delete(T entity) => _dbSet.Remove(entity);

        public IQueryable<T> FindAll(bool trackChanges = false) =>
            trackChanges ? _dbSet : _dbSet.AsNoTracking();

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> predicate, bool trackChanges = false) =>
            (trackChanges ? _dbSet : _dbSet.AsNoTracking()).Where(predicate);


    }
}
