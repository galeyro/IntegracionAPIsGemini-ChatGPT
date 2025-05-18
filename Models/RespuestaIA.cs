using System.ComponentModel.DataAnnotations;

namespace IntegracionGemini.Models
{
    public class RespuestaIA
    {
        [Key]
        public int Id { get; set; } // Identificador único de la respuesta

        [Required]
        public string Respuesta { get; set; } // Respuesta generada por el chatbot

        [Required]
        public DateTime Fecha { get; set; } //Fecha que se genero la respuesta
        [Required]
        public string Proveedor { get; set; } // Proveedor del servicio de IA (ej: "Gemini", "OpenAI")

        [Required]
        public string GuardadoPor { get; set; }

    }
}
