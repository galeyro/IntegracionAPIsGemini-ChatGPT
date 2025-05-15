namespace IntegracionGemini.Models
{
    //Modelos para enviar el prompt
    public class GeminiRequest
    {
        public List<GeminiContent> contents {  get; set; }
    }

    public class GeminiContent
    {
        public List<GeminiPart> parts { get; set; }
    }

    public class GeminiPart
    {
        public string text { get; set; }
    }

    //Modelos para deserializar la respuesta
    public class GeminiResponse
    {
        public List<Candidate> candidates { get; set; }
    }

    public class Candidate
    {
        public Content content { get; set; }
    }

    public class Content
    {
        public List<Part> parts { get; set; }
    }

    public class Part
    {
        public string text { get; set; }
    }

}
