namespace IntegracionGemini.Models
{
    /*
     Segun la doc de ChatGPT, nos pide la siguiente solicitud para texto:

    curl "https://api.openai.com/v1/responses" \
    -H "Content-Type: application/json" \
    -H "Authorization: Bearer $OPENAI_API_KEY" \
    -d '{
        "model": "gpt-4.1",
        "input": "Write a one-sentence bedtime story about a unicorn."
    }'

    Por tal motivo, es sufiuciente tener dos atributos model y input, 
    usando model = "gpt-3.5-turbo" como modelo gratuito
     */


    public class ChatGPTRequest
    {
        public string model { get; set; }
        public string input { get; set; }

    }

    public class OpenAIResponse
    {
        public List<Choice> choices { get; set; }
    }

    public class Choice
    {
        public Message message { get; set; }
        public string text { get; set; } // Para compatibilidad con ambos formatos
    }

    public class Message
    {
        public string role { get; set; }
        public string content { get; set; }
    }

    public class OpenAIChatRequest
    {
        public string model { get; set; }
        public bool store { get; set; } = true;
        public List<OpenAIMessage> messages { get; set; }
    }

    public class OpenAIMessage
    {
        public string role { get; set; }
        public string content { get; set; }
    }


}
