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
                model = "gpt-3.5-turbo", //Modelo gratuito en teoría
                input= prompt

            };

            string json_data = JsonConvert.SerializeObject(request);
            var content = new StringContent(json_data, Encoding.UTF8, "application/json");

            //Agregamos el header de autorizacion
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", openAIApiKey);

            var response = await _httpClient.PostAsync(url, content);
            return await response.Content.ReadAsStringAsync(); //Lee la respuesta de la peticion como string

        }

        public bool GuardarRespuestaBaseDatosLocal(string prompt, string respuesta)
        {
            throw new NotImplementedException();
        }

    }
}
