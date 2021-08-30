using System;
using System.Collections.Generic;
using System.Text;

namespace TaskBase.Core.Exceptions
{

    [Serializable]
    public class EmployeeIsNotInOrganizationException : Exception
    {
        public EmployeeIsNotInOrganizationException() { }
        public EmployeeIsNotInOrganizationException(string message) : base(message) { }
        public EmployeeIsNotInOrganizationException(string message, Exception inner) : base(message, inner) { }
        protected EmployeeIsNotInOrganizationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
