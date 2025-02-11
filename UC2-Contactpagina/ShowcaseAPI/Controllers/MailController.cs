using Microsoft.AspNetCore.Mvc;
using ShowcaseAPI.Models;
using System.Net.Mail;
using System.Net;
using System;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShowcaseAPI.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase {
        // POST api/<MailController>
        [HttpPost]
        public ActionResult Post([Bind("FirstName, LastName, Email, Phone, Subject, Message")] Contactform form) {
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
