using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBase.Data.Exceptions
{

    [Serializable]
    public class NotificationNotFoundException : Exception
    {
        public NotificationNotFoundException() { }
        public NotificationNotFoundException(string message) : base(message) { }
        public NotificationNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected NotificationNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
