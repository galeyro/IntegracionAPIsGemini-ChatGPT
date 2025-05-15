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

        // Clave de API de OpenAI (debería protegerse en producción)
        private readonly string openAIApiKey = "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";

        public OpenAIRepository()
        {
            // Inicializa el cliente HTTP
            _httpClient = new HttpClient();
        }

        // Envía un prompt a la API de OpenAI y devuelve la respuesta del chatbot
        public async Task<string> ObtenerRespuestaChatbot(string prompt)
        {
            string url = "https://api.openai.com/v1/chat/completions";

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
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", openAIApiKey);

            // Envía la solicitud POST a la API
            var response = await _httpClient.PostAsync(url, content);
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
