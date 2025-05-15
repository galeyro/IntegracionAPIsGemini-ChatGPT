namespace IntegracionGemini.Models
{
    // Representa un mensaje en el chat (usuario, texto y hora)
    public class ChatMessage
    {
        public string User { get; set; }   // Nombre del usuario o bot que envía el mensaje
        public string Text { get; set; }   // Contenido del mensaje
        public string Time { get; set; }   // Hora en la que se envió el mensaje (formato HH:mm)
    }
}
