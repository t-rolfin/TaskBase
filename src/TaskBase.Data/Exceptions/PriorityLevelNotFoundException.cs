using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBase.Data.Exceptions
{

    [Serializable]
    public class PriorityLevelNotFoundException : Exception
    {
        public PriorityLevelNotFoundException() { }
        public PriorityLevelNotFoundException(string message) : base(message) { }
        public PriorityLevelNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected PriorityLevelNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
