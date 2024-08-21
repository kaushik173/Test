using System.ComponentModel.DataAnnotations;

namespace LALoDep.Models
{
    public class AccountForgotPasswordViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress(ErrorMessage ="Invalid Email")]
  
        public string EmailAddress { get; set; }

        [Required]
        public int Width { get; set; }

        [Required]
        public int Height { get; set; }

        [Required]
        public int AvailWidth { get; set; }

        [Required]
        public int AvailHeight { get; set; }
    }
}