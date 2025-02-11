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

namespace Showcase_Profielpagina.Controllers
{
    public class ContactController : Controller
    {
        private readonly HttpClient _httpClient;
        public ContactController(HttpClient httpClient)
        {
            _httpClient = httpClient;
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
        public async Task<ActionResult> Index(Contactform form)
        {
            Console.WriteLine("");
            if(!ModelState.IsValid)
            {
                ViewBag.Message = "De ingevulde velden voldoen niet aan de gestelde voorwaarden";
                return View();
            }

            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            var json = JsonConvert.SerializeObject(form, settings);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync("api/mail", content); // Vervang deze regel met het POST-request

            if(!response.IsSuccessStatusCode)
            {
                ViewBag.Message = "Er is iets misgegaan";
                return View();
            }

            ViewBag.Message = "Het contactformulier is verstuurd";
            return View();
        }
    }
}
