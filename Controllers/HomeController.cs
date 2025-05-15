using System.Diagnostics;
using IntegracionGemini.Interfaces;
using IntegracionGemini.Models;
using IntegracionGemini.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Xunit;

namespace IntegracionGemini.Controllers
{
    // Controlador principal para la página de inicio y el chat
    public class HomeController : Controller
    {
        // Servicios para interactuar con Gemini y OpenAI
        private readonly GeminiRepository _geminiService;
        private readonly OpenAIRepository _openAIService;

        // Constructor: recibe las dependencias de los servicios
        public HomeController(GeminiRepository geminiService, OpenAIRepository openAIService)
        {
            _geminiService = geminiService;
            _openAIService = openAIService;
        }

        // Muestra la página principal con el historial de mensajes de ambos chats
        [HttpGet]
        public IActionResult Index()
        {
            // Recupera los mensajes de Gemini del TempData (si existen)
            ViewBag.GeminiMessages = TempData["GeminiMessages"] != null
                ? JsonConvert.DeserializeObject<List<ChatMessage>>(TempData["GeminiMessages"].ToString())
                : new List<ChatMessage>();

            // Recupera los mensajes de OpenAI del TempData (si existen)
            ViewBag.OpenAIMessages = TempData["OpenAIMessages"] != null
                ? JsonConvert.DeserializeObject<List<ChatMessage>>(TempData["OpenAIMessages"].ToString())
                : new List<ChatMessage>();

            TempData.Keep(); // Mantiene TempData para el siguiente request
            return View();
        }

        // Procesa el envío de un mensaje a uno de los chatbots
        [HttpPost]
        public async Task<IActionResult> Index(string prompt, string modelo, string usuario)
        {
            var now = DateTime.Now.ToString("HH:mm"); // Hora actual para mostrar en el chat

            // Recupera el historial de mensajes de ambos chats
            List<ChatMessage> geminiMessages = TempData["GeminiMessages"] != null
                ? JsonConvert.DeserializeObject<List<ChatMessage>>(TempData["GeminiMessages"].ToString())
                : new List<ChatMessage>();

            List<ChatMessage> openAIMessages = TempData["OpenAIMessages"] != null
                ? JsonConvert.DeserializeObject<List<ChatMessage>>(TempData["OpenAIMessages"].ToString())
                : new List<ChatMessage>();

            // Según el modelo seleccionado, envía el mensaje y guarda la respuesta
            if (modelo == "Gemini")
            {
                // Agrega el mensaje del usuario
                geminiMessages.Add(new ChatMessage { User = usuario, Text = prompt, Time = now });
                // Obtiene la respuesta del bot Gemini
                var respuesta = await _geminiService.ObtenerRespuestaChatbot(prompt);
                // Agrega la respuesta del bot
                geminiMessages.Add(new ChatMessage { User = "Gemini", Text = respuesta, Time = now });
            }
            else if (modelo == "OpenAI")
            {
                openAIMessages.Add(new ChatMessage { User = usuario, Text = prompt, Time = now });
                var respuesta = await _openAIService.ObtenerRespuestaChatbot(prompt);
                openAIMessages.Add(new ChatMessage { User = "OpenAI", Text = respuesta, Time = now });
            }

            // Guarda los historiales actualizados en TempData
            TempData["GeminiMessages"] = JsonConvert.SerializeObject(geminiMessages);
            TempData["OpenAIMessages"] = JsonConvert.SerializeObject(openAIMessages);

            // Redirige a la vista principal para mostrar el chat actualizado
            return RedirectToAction("Index");
        }

    }
}
