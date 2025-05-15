namespace IntegracionGemini.Models
{
    // Modelo para enviar un prompt simple a la API de OpenAI
    public class ChatGPTRequest
    {
        public string model { get; set; }   // Nombre del modelo a usar (ej: "gpt-3.5-turbo")
        public string input { get; set; }   // Texto del prompt
    }

    // Modelo para deserializar la respuesta de OpenAI
    public class OpenAIResponse
    {
        public List<Choice> choices { get; set; } // Lista de posibles respuestas
    }

    // Representa una opción de respuesta de OpenAI
    public class Choice
    {
        public Message message { get; set; } // Mensaje estructurado (chat)
        public string text { get; set; }     // Texto plano (para compatibilidad)
    }

    // Mensaje individual en una conversación (chat)
    public class Message
    {
        public string role { get; set; }     // Rol: "user", "assistant", etc.
        public string content { get; set; }  // Contenido del mensaje
    }

    // Modelo para enviar un chat con historial a la API de OpenAI
    public class OpenAIChatRequest
    {
        public string model { get; set; }    // Nombre del modelo a usar
        public bool store { get; set; } = true; // Indica si se almacena el historial
        public List<OpenAIMessage> messages { get; set; } // Historial de mensajes
    }

    // Mensaje en el historial de chat de OpenAI
    public class OpenAIMessage
    {
        public string role { get; set; }     // Rol del mensaje
        public string content { get; set; }  // Contenido del mensaje
    }
}
