using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public record RegisterModel(
            [Required(ErrorMessage = "UserName is required.")]
            string UserName,

            [EmailAddress]
            [Required(ErrorMessage = "Email is required.")]
            string Email,

            [Required(ErrorMessage = "Password is required.")]
            string Password
        );
}
