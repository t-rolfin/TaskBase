using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBase.Components.Models
{
    public class TaskRowCustomization
    {
        public TaskRowCustomization(string id, string rowName, string bgColor)
        {
            RowName = rowName;
            BgColor = bgColor;
            Id = id;
        }

        public string Id { get; set; }
        public string RowName { get; set; } = "New Row";
        public string BgColor { get; set; } = "bg-primary";
    }
}
