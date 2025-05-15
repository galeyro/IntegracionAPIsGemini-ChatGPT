using IntegracionGemini.Interfaces;
using IntegracionGemini.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Configuration;



namespace IntegracionGemini.Repositories
{
    public class OpenAIRepository : IChatbotService
    {
        private readonly HttpClient _httpClient;
        private readonly string openAIApiKey = "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";

        public OpenAIRepository()
        {
            _httpClient = new HttpClient();
        }
        public async Task<string> ObtenerRespuestaChatbot(string prompt)
        {
            string url = "https://api.openai.com/v1/chat/completions";
            var request = new OpenAIChatRequest
            {
                model = "gpt-4o-mini",
                store = true,
                messages = new List<OpenAIMessage>
        {
            new OpenAIMessage { role = "user", content = prompt }
        }
            };

            string json_data = JsonConvert.SerializeObject(request);
            var content = new StringContent(json_data, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", openAIApiKey);

            var response = await _httpClient.PostAsync(url, content);
            var responseString = await response.Content.ReadAsStringAsync();

            var openAIResponse = JsonConvert.DeserializeObject<OpenAIResponse>(responseString);

            var text = openAIResponse?.choices?.FirstOrDefault()?.message?.content
                ?? "Sin respuesta";

            return text;
        }

        public bool GuardarRespuestaBaseDatosLocal(string prompt, string respuesta)
        {
            throw new NotImplementedException();
        }

    }
}
