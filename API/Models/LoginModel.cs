using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public record LoginModel(
        [Required(ErrorMessage = "UserName is required.")]
        string UserName, 
        
        [Required(ErrorMessage = "Password is required.")]
        string Password
    );
}
