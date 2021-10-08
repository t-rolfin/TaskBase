﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskBase.Core.Interfaces;

namespace TaskBase.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TaskDbContext _context;

        public UnitOfWork(TaskDbContext context)
        {
            _context = context;
        }

        public ITaskAsyncRepository Tasks => new TaskRepository(_context);

        public INotificationRepository Notifications => new NotificationRepository(_context);

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}