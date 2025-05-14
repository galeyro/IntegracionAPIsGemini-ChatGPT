using IntegracionGemini.Interfaces;

namespace IntegracionGemini.Repositories
{
    public class OpenAIRepository : IChatbotService
    {
        /*
         [] Investigar como implementar chatgpt
         */
        public bool GuardarRespuestaBaseDatosLocal(string prompt, string respuesta)
        {
            throw new NotImplementedException();
        }

        public Task<string> ObtenerRespuestaChatbot(string prompt)
        {
            throw new NotImplementedException();
        }
    }
}
