using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Models
{
    public record CreateTaskModel(
        [Required]
        string Title,

        [Required]
        string Description,

        DateTime DueDate,
        string UserId
    );
}
