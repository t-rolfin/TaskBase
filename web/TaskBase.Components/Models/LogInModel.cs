using System.ComponentModel.DataAnnotations;

namespace TaskBase.Components.Models
{
    public class LogInModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
