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
            string url = "https://api.openai.com/v1/responses";
            var request = new ChatGPTRequest
            {
                model = "gpt-4.1-nano", //Modelo más economico
                input= prompt

            };

            string json_data = JsonConvert.SerializeObject(request);
            var content = new StringContent(json_data, Encoding.UTF8, "application/json");

            //Agregamos el header de autorizacion
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", openAIApiKey);

            var response = await _httpClient.PostAsync(url, content);
            var responseString = await response.Content.ReadAsStringAsync();

            // Deserializa la respuesta y extrae solo el texto
            var openAIResponse = JsonConvert.DeserializeObject<OpenAIResponse>(responseString);

            // Para modelos tipo chat (message.content)
            var text = openAIResponse?.choices?.FirstOrDefault()?.message?.content
                // Para modelos tipo completions (text)
                ?? openAIResponse?.choices?.FirstOrDefault()?.text
                ?? "Sin respuesta";

            return responseString;

        }

        public bool GuardarRespuestaBaseDatosLocal(string prompt, string respuesta)
        {
            throw new NotImplementedException();
        }

    }
}
