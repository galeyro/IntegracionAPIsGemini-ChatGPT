using System.Diagnostics;
using IntegracionGemini.Interfaces;
using IntegracionGemini.Models;
using IntegracionGemini.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace IntegracionGemini.Controllers
{
    // Controlador principal para la página de inicio y el chat
    public class HomeController : Controller
    {
        private const string GeminiMessagesKey = "GeminiMessages";
        private const string OpenAIMessagesKey = "OpenAIMessages";

        // Servicios para interactuar con Gemini y OpenAI
        private readonly GeminiRepository _geminiService;
        private readonly OpenAIRepository _openAIService;


        // Contexto de la base de datos
        private readonly AppDbContext _context;


        // Constructor: recibe las dependencias de los servicios
        public HomeController(GeminiRepository geminiService, OpenAIRepository openAIService, AppDbContext context)
        {
            _geminiService = geminiService;
            _openAIService = openAIService;
            _context = context;
        }

        // Muestra la página principal con el historial de mensajes de ambos chats
        [HttpGet]
        public IActionResult Index()
        {
            // Recupera los mensajes de Gemini del TempData (si existen)
            ViewBag.SelectedUserGemini = TempData["SelectedUserGemini"]?.ToString() ?? "Galo";
            ViewBag.GeminiMessages = TempData[GeminiMessagesKey] != null
                ? JsonConvert.DeserializeObject<List<ChatMessage>>(TempData[GeminiMessagesKey].ToString())
                : new List<ChatMessage>();

            // Recupera los mensajes de OpenAI del TempData (si existen)
            ViewBag.SelectedUserOpenAI = TempData["SelectedUserOpenAI"]?.ToString() ?? "Galo";
            ViewBag.OpenAIMessages = TempData[OpenAIMessagesKey] != null
                ? JsonConvert.DeserializeObject<List<ChatMessage>>(TempData[OpenAIMessagesKey].ToString())
                : new List<ChatMessage>();

            TempData.Keep(); // Mantiene TempData para el siguiente request
            return View();
        }

        // Procesa el envío de un mensaje a uno de los chatbots
        [HttpPost]
        public async Task<IActionResult> Index(string prompt, string modelo, string usuarioGemini, string usuarioOpenAI)

        {
            var now = DateTime.Now.ToString("HH:mm"); // Hora actual para mostrar en el chat

            // Recupera el historial de mensajes de ambos chats
            List<ChatMessage> geminiMessages = TempData[GeminiMessagesKey] != null
                ? JsonConvert.DeserializeObject<List<ChatMessage>>(TempData[GeminiMessagesKey].ToString())
                : new List<ChatMessage>();

            List<ChatMessage> openAIMessages = TempData[OpenAIMessagesKey] != null
                ? JsonConvert.DeserializeObject<List<ChatMessage>>(TempData[OpenAIMessagesKey].ToString())
                : new List<ChatMessage>();

            // Según el modelo seleccionado, envía el mensaje y guarda la respuesta
            if (modelo == "Gemini")
            {
                // Agrega el mensaje del usuario
                geminiMessages.Add(new ChatMessage { User = usuarioGemini, Text = prompt, Time = now });
                // Obtiene la respuesta del bot Gemini
                var respuesta = await _geminiService.ObtenerRespuestaChatbot(prompt);
                // Agrega la respuesta del bot
                geminiMessages.Add(new ChatMessage { User = "Gemini", Text = respuesta, Time = now });

                //Guardar respuesta en la BD de Gemini
                var nuevaRespuesta = new RespuestaIA
                {
                    Respuesta = respuesta,
                    Fecha = DateTime.Now,
                    Proveedor = "Gemini",
                    GuardadoPor = usuarioGemini
                };
                _context.RespuestasIA.Add(nuevaRespuesta);
                await _context.SaveChangesAsync();


            }
            else if (modelo == "OpenAI")
            {
                openAIMessages.Add(new ChatMessage { User = usuarioOpenAI, Text = prompt, Time = now });
                var respuesta = await _openAIService.ObtenerRespuestaChatbot(prompt);
                openAIMessages.Add(new ChatMessage { User = "OpenAI", Text = respuesta, Time = now });

                var nuevaRespuesta = new RespuestaIA
                {
                    Respuesta = respuesta,
                    Fecha = DateTime.Now,
                    Proveedor = "OpenAI",
                    GuardadoPor = usuarioOpenAI
                };
                _context.RespuestasIA.Add(nuevaRespuesta);
                await _context.SaveChangesAsync();

            }

            // Guarda los historiales actualizados en TempData
            TempData[GeminiMessagesKey] = JsonConvert.SerializeObject(geminiMessages);
            TempData[OpenAIMessagesKey] = JsonConvert.SerializeObject(openAIMessages);

            //Guardar ultimo usuario que envio la peticion 
            TempData["SelectedUserGemini"] = usuarioGemini;
            TempData["SelectedUserOpenAI"] = usuarioOpenAI;


            // Redirige a la vista principal para mostrar el chat actualizado
            return RedirectToAction("Index");
        }

        

    }
}
