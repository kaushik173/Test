using System.ComponentModel.DataAnnotations;

namespace LALoDep.Models
{
    public class AccountLoginViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

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