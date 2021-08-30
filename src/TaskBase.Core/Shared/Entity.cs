using System;
using System.Collections.Generic;
using System.Text;

namespace TaskBase.Core.Shared
{
    public class Entity<T>
    {
        private Entity() { }

        protected Entity(T id) 
            => (Id) = (id);

        public T Id { get; init; }
    }
}
