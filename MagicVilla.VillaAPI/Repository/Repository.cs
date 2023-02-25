using MagicVilla.VillaAPI.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;
using MagicVilla.VillaAPI.Repository.IRepository;

namespace MagicVilla.VillaAPI.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> _dbSet;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            _dbSet = _db.Set<T>();
        }
        public async Task CreateAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await SaveAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, bool tracked = true, string? includeProperties = null)
        {
            IQueryable<T> query = _dbSet;
            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null, int pageSize = 0, int pageNumber = 1)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }


            if (pageSize > 0)
            {
                if(pageSize > 100)
                {
                    pageSize = 100;
                }
                //skip.take(5)
                //page number - 2 || page size - 5
                //skip(5*(1)) take(5)

                query = query.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
            }


            if (includeProperties != null)
            {
                foreach(var property in includeProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }

            return await query.ToListAsync();
        }

        public async Task RemoveAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
