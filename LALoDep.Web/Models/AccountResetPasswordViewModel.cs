 
using System;
using System.ComponentModel.DataAnnotations;

namespace LALoDep.Models
{
    public class AccountResetPasswordViewModel
    {
        [Required]
      
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
         [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public Guid Code { get; set; }
    }
} 