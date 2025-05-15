using System.Diagnostics;
using IntegracionGemini.Interfaces;
using IntegracionGemini.Models;
using IntegracionGemini.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IntegracionGemini.Controllers
{
    public class HomeController : Controller
    {
        private readonly GeminiRepository _geminiService;
        private readonly OpenAIRepository _openAIService;

        public HomeController(GeminiRepository geminiService, OpenAIRepository openAIService)
        {
            _geminiService = geminiService;
            _openAIService = openAIService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.GeminiMessages = TempData["GeminiMessages"] != null
                ? JsonConvert.DeserializeObject<List<ChatMessage>>(TempData["GeminiMessages"].ToString())
                : new List<ChatMessage>();

            ViewBag.OpenAIMessages = TempData["OpenAIMessages"] != null
                ? JsonConvert.DeserializeObject<List<ChatMessage>>(TempData["OpenAIMessages"].ToString())
                : new List<ChatMessage>();

            TempData.Keep(); // Mantiene TempData para el siguiente request
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string prompt, string modelo)
        {
            var now = DateTime.Now.ToString("HH:mm");
            List<ChatMessage> geminiMessages = TempData["GeminiMessages"] != null
                ? JsonConvert.DeserializeObject<List<ChatMessage>>(TempData["GeminiMessages"].ToString())
                : new List<ChatMessage>();

            List<ChatMessage> openAIMessages = TempData["OpenAIMessages"] != null
                ? JsonConvert.DeserializeObject<List<ChatMessage>>(TempData["OpenAIMessages"].ToString())
                : new List<ChatMessage>();

            if (modelo == "Gemini")
            {
                geminiMessages.Add(new ChatMessage { User = "Tú", Text = prompt, Time = now });
                var respuesta = await _geminiService.ObtenerRespuestaChatbot(prompt);
                geminiMessages.Add(new ChatMessage { User = "Gemini", Text = respuesta, Time = now });
            }
            else if (modelo == "OpenAI")
            {
                openAIMessages.Add(new ChatMessage { User = "Tú", Text = prompt, Time = now });
                var respuesta = await _openAIService.ObtenerRespuestaChatbot(prompt);
                openAIMessages.Add(new ChatMessage { User = "OpenAI", Text = respuesta, Time = now });
            }

            TempData["GeminiMessages"] = JsonConvert.SerializeObject(geminiMessages);
            TempData["OpenAIMessages"] = JsonConvert.SerializeObject(openAIMessages);

            return RedirectToAction("Index");
        }
    }
}
