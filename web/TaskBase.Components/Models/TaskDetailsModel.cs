﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBase.Components.Models
{
    public record TaskDetailsModel(string Id, string Title, string Description, PriorityLevelModel PriorityLevel, string DueDate);
}
