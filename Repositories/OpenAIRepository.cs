using IntegracionGemini.Interfaces;
using IntegracionGemini.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace IntegracionGemini.Repositories
{
    // Repositorio para interactuar con la API de OpenAI (ChatGPT)
    public class OpenAIRepository : IChatbotService
    {
        // Cliente HTTP para enviar solicitudes a la API
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _baseUrl;

        public OpenAIRepository(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _apiKey = configuration["OpenAI:ApiKey"];
            _baseUrl = configuration["OpenAI:BaseUrl"];
        }

        // Envía un prompt a la API de OpenAI y devuelve la respuesta del chatbot
        public async Task<string> ObtenerRespuestaChatbot(string prompt)
        {
            // Construye la solicitud con el modelo y el mensaje del usuario
            var request = new OpenAIChatRequest
            {
                model = "gpt-4o-mini",
                store = true,
                messages = new List<OpenAIMessage>
                {
                    new OpenAIMessage { role = "user", content = prompt }
                }
            };

            // Serializa la solicitud a JSON
            string json_data = JsonConvert.SerializeObject(request);
            var content = new StringContent(json_data, Encoding.UTF8, "application/json");

            // Agrega la cabecera de autorización
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

            // Envía la solicitud POST a la API
            var response = await _httpClient.PostAsync(_baseUrl, content);
            var responseString = await response.Content.ReadAsStringAsync();

            // Deserializa la respuesta de la API
            var openAIResponse = JsonConvert.DeserializeObject<OpenAIResponse>(responseString);

            // Extrae el texto de la respuesta o devuelve un mensaje por defecto
            var text = openAIResponse?.choices?.FirstOrDefault()?.message?.content
                ?? "Sin respuesta";

            return text;
        }

        // Método no implementado para guardar respuestas localmente
        public bool GuardarRespuestaBaseDatosLocal(string prompt, string respuesta)
        {
            throw new NotImplementedException();
        }
    }
}
