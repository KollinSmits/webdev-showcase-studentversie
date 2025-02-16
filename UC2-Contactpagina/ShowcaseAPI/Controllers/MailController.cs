using Microsoft.AspNetCore.Mvc;
using ShowcaseAPI.Models;
using System.Net.Mail;
using System.Net;
using System;
using System.Drawing.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Numerics;
using System.Text.RegularExpressions;
using Microsoft.IdentityModel.Tokens;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShowcaseAPI.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase {

        private bool ValidateFormInputs(Contactform contactform) {
            string mailregex = @"^([0-9a-zA-Z]" +
                               @"([\+\-_\.][0-9a-zA-Z]+)*" +
                               @")+" +
                               @"@(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]*\.)+[a-zA-Z0-9]{2,17})$";
            List<string> errors = new List<string>();

            if (string.IsNullOrWhiteSpace(contactform.FirstName) || !Regex.IsMatch(contactform.FirstName, @"^[a-zA-Z]+$"))
                errors.Add("Fistname is required and can only contain letters.");

            if (string.IsNullOrWhiteSpace(contactform.LastName) || !Regex.IsMatch(contactform.LastName, @"^[a-zA-Z]+$"))
                errors.Add("Lastname is required and can only contain letters.");

            if (string.IsNullOrWhiteSpace(contactform.Email) || !Regex.IsMatch(contactform.Email, mailregex))
                errors.Add("fistname is required and email in incorrect.");

            if (string.IsNullOrWhiteSpace(contactform.Phone) || !Regex.IsMatch(contactform.Phone, @"^+?[0-9]{10,15}$"))
                errors.Add("Ongeldig telefoonnummer. Gebruik alleen cijfers en eventueel een landcode.");

            if (string.IsNullOrWhiteSpace(contactform.Subject) || !Regex.IsMatch(contactform.Subject, @"^[a-zA-Z]+$"))
                errors.Add("Onderwerp mag niet leeg zijn.");

            if (string.IsNullOrWhiteSpace(contactform.Message) || contactform.Message.Length < 10 )
                errors.Add("Bericht moet minimaal 10 tekens bevatten.");

            if (errors.IsNullOrEmpty()) {
                return true;
            } else {
                return false;
            }

        }

        // POST api/<MailController>
        [HttpPost]
        public ActionResult Post([FromBody] Contactform form) {
            var client = new SmtpClient("sandbox.smtp.mailtrap.io", 2525) {
                Credentials = new NetworkCredential("17eb748d87ca77", "a058ddf888af5e"),
                EnableSsl = true
            };

            MailAddress from = new MailAddress(form.Email);
            MailAddress to = new MailAddress("9ce65ce9a2-f47694@inbox.mailtrap.io");
            MailMessage email = new MailMessage(from, to);
            email.Subject = form.Subject;
            email.Body = $"geachte, student\n" +
                       $"\n{form.Message}\n\n" +
                       $"met vriendelijke groet,\n\n\n" +
                       $"{form.FirstName} {form.LastName}\n\n" +
                       $"Telefoonnummer: {form.Phone}";

            try {
                if (!ValidateFormInputs(form)) {
                        return BadRequest(new { message = "Ongeldige invoer. Controleer uw gegevens en probeer opnieuw." });
                }

                client.Send(email);
                System.Console.WriteLine("Sent");
                return Ok(new { message = "Email succesvol verzonden" });
            }
            catch (SmtpException ex) {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, new { message = "Er is een fout opgetreden bij het verzenden van de e-mail.", error = ex.Message });
            }

        }
    }
}
