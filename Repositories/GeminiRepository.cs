using IntegracionGemini.Interfaces;
using IntegracionGemini.Models;
using Microsoft.AspNetCore.Components.Web;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json.Serialization;

namespace IntegracionGemini.Repositories
{
    public class GeminiRepository : IChatbotService
    {
        private HttpClient _httpClient;
        private readonly string geminiApiKey="AIzaSyCOzwL0lTY9YYqR07O5gkmVnE1OY-5eGGY";

        public GeminiRepository()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> ObtenerRespuestaChatbot(string prompt)
        {
            string url = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key="+geminiApiKey;
            GeminiRequest request = new GeminiRequest
            {
                contents = new List<GeminiContent>
                {
                    new GeminiContent
                    {
                        parts = new List<GeminiPart>
                        {
                            new GeminiPart
                            {
                                text=prompt
                            }
                        }
                    }
                }
            };
            string json_data = JsonConvert.SerializeObject(request);
            var content = new StringContent(json_data, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content); //Para cada metodo asincrono poner await
            return await response.Content.ReadAsStringAsync();
        }

        public bool GuardarRespuestaBaseDatosLocal(string prompt, string respuesta)
        {
            throw new NotImplementedException();
        }
    }
}
