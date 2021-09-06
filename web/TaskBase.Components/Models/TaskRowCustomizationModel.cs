using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBase.Components.Models
{
    public class TaskRowCustomizationModel
    {
        public TaskRowCustomizationModel(bool hasTaskCreationButton, string rowName, string bgColor)
        {
            HasTaskCreationButton = hasTaskCreationButton;
            RowName = rowName;
            BgColor = bgColor;
        }

        public bool HasTaskCreationButton { get; set; }
        public string RowName { get; set; } = "New Row";
        public string BgColor { get; set; } = "bg-primary";
    }
}
