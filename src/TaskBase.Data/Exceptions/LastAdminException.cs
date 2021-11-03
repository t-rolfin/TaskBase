using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBase.Data.Exceptions
{

    [Serializable]
    public class LastAdminException : Exception
    {
        public LastAdminException() { }
        public LastAdminException(string message) : base(message) { }
        public LastAdminException(string message, Exception inner) : base(message, inner) { }
        protected LastAdminException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
