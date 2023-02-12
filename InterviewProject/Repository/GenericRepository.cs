﻿using InterviewProject.DbContexts;
using InterviewProject.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace InterviewProject.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DataContext _dataContext;
        private readonly DbSet<T> _db;
        public GenericRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
            _db = _dataContext.Set<T>();
        }
        public async Task Delete(int id)
        {
            var entity=await _db.FindAsync(id);
            _db.Remove(entity);
        }

        public async Task<T> Get(Expression<Func<T, bool>> expression, List<string> includes = null)
        {
            IQueryable<T> query = _db;
            if (includes!=null)
            {
                foreach (var item in includes)
                {
                    query = query.Include(item);
                }
            }
         
            return await query.AsNoTracking().FirstOrDefaultAsync(expression);
        }

        public async Task<IList<T>> GetAll(Expression<Func<T, bool>> expression = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, List<string> includes = null)
        {
            IQueryable<T> query = _db;
            if (expression != null)
            {
                query = query.Where(expression);
            }
            if (includes != null)
            {
                foreach (var item in includes)
                {
                    query = query.Include(item);
                }
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return await query.AsNoTracking().ToListAsync();
        }

        public async Task Insert(T entity)
        {
            await _db.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _db.Attach(entity);
            _dataContext.Entry(entity).State = EntityState.Modified;
        }
    }
}
