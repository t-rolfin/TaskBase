using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBase.Data.Exceptions
{

    [Serializable]
    public class TaskNotFoundException : Exception
    {
        public TaskNotFoundException() { }
        public TaskNotFoundException(string message) : base(message) { }
        public TaskNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected TaskNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
