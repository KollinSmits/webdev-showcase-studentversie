using System.ComponentModel.DataAnnotations;

namespace ShowcaseAPI.Models
{
    public class Contactform
    {
        [Required(ErrorMessage = "The firstname is required.")]
        [StringLength(20, ErrorMessage = "Your firstname can only be 20 characters long.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "The firstname can only contain letters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "The lastname is required.")]
        [StringLength(20, ErrorMessage = "Your Lastname can only be 20 characters long.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "The last name can only contain letters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "The e-mailadres is required.")]
        [EmailAddress]
        [MaxLength(30, ErrorMessage = "Your e-mailadres can only be 30 characters long.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The phone number is required.")]
        [Phone(ErrorMessage = "Please enter a valid phone number.")]
        [StringLength(15, MinimumLength = 8, ErrorMessage = "The phone number must be between 8 and 15 digits long.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "The subject is required.")]
        [StringLength(30, ErrorMessage = "The subject can only be 30 characters long.")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "The message is required.")]
        [StringLength(200, MinimumLength = 20, ErrorMessage = "Your message needs to be between 20 and 200 characters long.")]
        public string Message { get; set; }
    }
}
