using System.Diagnostics;
using IntegracionGemini.Interfaces;
using IntegracionGemini.Models;
using IntegracionGemini.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IntegracionGemini.Controllers
{
    public class HomeController : Controller
    {
        private IChatbotService _chatbotService;

        public HomeController(IChatbotService chatbotService)
        {

            _chatbotService = chatbotService;
  
        }

        public async Task<IActionResult> Index()
        {
            var response = await _chatbotService.ObtenerRespuestaChatbot("Resumen de 100 palabras de Avatar");
            ViewBag.respuesta = response;
            return View();
        }

 
    }
}
