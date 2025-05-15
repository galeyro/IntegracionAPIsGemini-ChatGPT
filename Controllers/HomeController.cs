using System.Diagnostics;
using IntegracionGemini.Interfaces;
using IntegracionGemini.Models;
using IntegracionGemini.Repositories;
using Microsoft.AspNetCore.Mvc;

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
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string prompt, string modelo)
        {
            if (string.IsNullOrWhiteSpace(prompt) || string.IsNullOrWhiteSpace(modelo))
            {
                ViewBag.respuesta = "Por favor, ingresa un mensaje y selecciona un modelo.";
                return View();
            }

            string respuesta = string.Empty;

            if (modelo == "Gemini")
            {
                respuesta = await _geminiService.ObtenerRespuestaChatbot(prompt);
            }
            else if (modelo == "OpenAI")
            {
                respuesta = await _openAIService.ObtenerRespuestaChatbot(prompt);
            }
            else
            {
                respuesta = "Modelo no reconocido.";
            }

            ViewBag.respuesta = respuesta;
            return View();
        }





    }
}
