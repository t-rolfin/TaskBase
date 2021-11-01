using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBase.Data.Exceptions
{

    [Serializable]
    public class InvalidUserNameException : Exception
    {
        public InvalidUserNameException() { }
        public InvalidUserNameException(string message) : base(message) { }
        public InvalidUserNameException(string message, Exception inner) : base(message, inner) { }
        protected InvalidUserNameException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
