using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBase.Data.Exceptions
{

    [Serializable]
    public class DuplicateUserNameException : Exception
    {
        public DuplicateUserNameException() { }
        public DuplicateUserNameException(string message) : base(message) { }
        public DuplicateUserNameException(string message, Exception inner) : base(message, inner) { }
        protected DuplicateUserNameException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
