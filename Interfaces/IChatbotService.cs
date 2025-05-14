namespace IntegracionGemini.Interfaces
{
    public interface IChatbotService
    {
        //Task para que el metodo sea asincrono
        //Asincrono: permite que el programa no se detenga mientras espera la respuesta
        public Task<string> ObtenerRespuestaChatbot(string prompt);
        public bool GuardarRespuestaBaseDatosLocal(string prompt, string respuesta);
    }
}

