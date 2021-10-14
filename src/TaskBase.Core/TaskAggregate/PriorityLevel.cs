using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBase.Core.Shared;

namespace TaskBase.Core.TaskAggregate
{
    public class PriorityLevel : Enumeration
    {
        public static readonly PriorityLevel Low = new PriorityLevel(1, "Low");
        public static readonly PriorityLevel High = new PriorityLevel(2, "High");
        public static readonly PriorityLevel VeryHigh = new PriorityLevel(3, "Very High");

        public PriorityLevel(int value, string displayName) : base(value, displayName) { }
        public PriorityLevel() : base() { }
    }
}
