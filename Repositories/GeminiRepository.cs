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
        private HttpClient _httpClient; //HttpClient es una clase que permite hacer peticiones HTTP
        private readonly string geminiApiKey="AIzaSyCOzwL0lTY9YYqR07O5gkmVnE1OY-5eGGY"; //Guarda API de Gemini

        public GeminiRepository()
        {
            //Crea una instancia HttpClient en el campo _httpClient
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
            string json_data = JsonConvert.SerializeObject(request);//Convierte el objeto request a una cadena JSON
            var content = new StringContent(json_data, Encoding.UTF8, "application/json");//content es el contenido de la peticion
            var response = await _httpClient.PostAsync(url, content); //Para cada metodo asincrono poner await
            var responseString = await response.Content.ReadAsStringAsync();

            // Deserializa el JSON y extrae el texto
            var geminiResponse = JsonConvert.DeserializeObject<GeminiResponse>(responseString);
            var text = geminiResponse?.candidates?.FirstOrDefault()?.content?.parts?.FirstOrDefault()?.text ?? "Sin respuesta";

            return text;
        }

        public bool GuardarRespuestaBaseDatosLocal(string prompt, string respuesta)
        {
            throw new NotImplementedException();
        }
    }
}
