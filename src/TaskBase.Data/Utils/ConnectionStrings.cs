using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBase.Data.Utils
{
    public class ConnectionStrings
    {
        Dictionary<string, string> _connectionsStrings;

        public ConnectionStrings()
        {
            _connectionsStrings = new Dictionary<string, string>();
        }

        public ConnectionStrings Add(string name, string connectionString)
        {
            _connectionsStrings.Add(name, connectionString);
            return this;
        }

        public string GetConnectionString(string name)
        {
            return _connectionsStrings.Where(x => x.Key == name).First().Value;
        }
    }
}
