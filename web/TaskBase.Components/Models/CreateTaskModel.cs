using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBase.Components.Models
{
    public class CreateTaskModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public DateTime DueDate { get; set; }
        public string AssignTo { get; set; }
        public int PriorityLevel { get; set; }

    }
}
