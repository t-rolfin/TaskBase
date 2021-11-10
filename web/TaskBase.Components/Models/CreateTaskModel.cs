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
        [Required(ErrorMessage = "The Title field is required.")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "The Description field is required.")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }

        [Display(Name = "Assign To")]
        public string AssignTo { get; set; }

        [Display(Name = "PriorityLevel")]
        public int PriorityLevel { get; set; }

    }
}
