using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using Showcase_Profielpagina.Models;
using System.Numerics;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Net.NetworkInformation;

namespace Showcase_Profielpagina.Controllers
{
    public class ContactController : Controller
    {
        private readonly HttpClient _httpClient;
        public ContactController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7278");
        }

        // GET: ContactController
        public ActionResult Index()
        {
            return View();
        }

        // POST: ContactController
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Contactform form, string Recaptcha)
        {
            Console.WriteLine("");
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "De ingevulde velden voldoen niet aan de gestelde voorwaarden";
                return View();
            }

            var isRechaptchaValid = await ValidateRecaptcha(Recaptcha);

            if (!isRechaptchaValid) {
                ViewBag.Message = "reCAPTCHA validatie is mislukt. Probeer het opnieuw.";
                return View();
            }

            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            var json = JsonConvert.SerializeObject(form, settings);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync("/api/mail", content); 

            if(!response.IsSuccessStatusCode)
            {
                ViewBag.Message = "Er is iets misgegaan";
                return View();
            }

            ViewBag.Message = "Het contactformulier is verstuurd";
            return View();
        }

        private async Task<bool> ValidateRecaptcha(string recaptchaResponse) {
            if (string.IsNullOrEmpty(recaptchaResponse)) {
                return false;
            }

            using (var client = new HttpClient()) {
                var requestData = new FormUrlEncodedContent(new[]
                {
             new KeyValuePair<string, string>("secret", "6Le-OdkqAAAAAMWAiu3wTTj8YqD_xG4_3nYVaiOq"), // in secrets zetten
             new KeyValuePair<string, string>("response", recaptchaResponse)
         });

                var recaptchaResponseMessage = await client.PostAsync("https://www.google.com/recaptcha/api/siteverify", requestData);

                if (!recaptchaResponseMessage.IsSuccessStatusCode) {
                    return false;
                }

                var jsonResponse = await recaptchaResponseMessage.Content.ReadAsStringAsync();
                dynamic JsonRecaptcharespone = JsonConvert.DeserializeObject(jsonResponse);
                return JsonRecaptcharespone.success == true;
            }
        }
    }
}
