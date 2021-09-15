using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBase.Data.Utils
{
    public class ConnectionString
    {
        public ConnectionString(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; init; }
        public string Value { get; init; }
    }
}
