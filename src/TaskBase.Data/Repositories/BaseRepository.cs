using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBase.Core.Interfaces;

namespace TaskBase.Data.Repositories
{
    public abstract class BaseRepository<T> where T : class, IRootAggregate
    {
        protected readonly TaskDbContext _context;
        internal DbSet<T> dbSet;

        protected BaseRepository(TaskDbContext context)
        {
            _context = context;
            dbSet = _context.Set<T>();
        }
    }
}
