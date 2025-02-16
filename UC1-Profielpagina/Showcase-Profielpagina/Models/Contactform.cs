using System.ComponentModel.DataAnnotations;

namespace Showcase_Profielpagina.Models
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
        [RegularExpression(@"^([0-9a-zA-Z]" +
                               @"([\+\-_\.][0-9a-zA-Z]+)*" +
                               @")+" +
                               @"@(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]*\.)+[a-zA-Z0-9]{2,17})$", ErrorMessage = "email is invalid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The phone number is required.")]
        [Phone(ErrorMessage = "Please enter a valid phone number.")]
        [RegularExpression(@"^+?[0-9]{10,15}$", ErrorMessage = "phone number can only contain numbers and needs to be 8-15 numbers long")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "The subject is required.")]
        [StringLength(30, ErrorMessage = "The subject can only be 30 characters long.")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "The message is required.")]
        [StringLength(600, MinimumLength = 20, ErrorMessage = "Your message needs to be between 20 and 600 characters long.")]
        public string Message { get; set; }
    }
}
