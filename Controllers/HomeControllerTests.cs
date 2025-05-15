using System.Collections.Generic;
using System.Threading.Tasks;
using IntegracionGemini.Controllers;
using IntegracionGemini.Models;
using IntegracionGemini.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Xunit;

public class HomeControllerTests
{
    [Fact]
    public async Task UsuarioPuedePreguntarAmbosModelos()
    {
        // Arrange: Mocks para los servicios de Gemini y OpenAI  
        var geminiMock = new Mock<GeminiRepository>();
        var openAIMock = new Mock<OpenAIRepository>();

        geminiMock.Setup(s => s.ObtenerRespuestaChatbot(It.IsAny<string>()))
            .ReturnsAsync("Respuesta Gemini");
        openAIMock.Setup(s => s.ObtenerRespuestaChatbot(It.IsAny<string>()))
            .ReturnsAsync("Respuesta OpenAI");

        var controller = new HomeController(geminiMock.Object, openAIMock.Object);

        // Simula TempData (necesario para pruebas de controladores en ASP.NET Core)  
        var tempData = new TempDataDictionary(
            new DefaultHttpContext(),
            Mock.Of<ITempDataProvider>());
        controller.TempData = tempData;

        // Act: El usuario pregunta a ambos modelos  
        await controller.Index("Pregunta a Gemini", "Gemini", "Luis");
        await controller.Index("Pregunta a OpenAI", "OpenAI", "Luis");

        // Recupera los mensajes de la vista  
        var result = controller.Index() as ViewResult;
        var geminiMessages = result?.ViewData["GeminiMessages"] as List<ChatMessage>;
        var openAIMessages = result?.ViewData["OpenAIMessages"] as List<ChatMessage>;

        // Assert: El usuario aparece en ambos historiales  
        Assert.Contains(geminiMessages, m => m.User == "Luis");
        Assert.Contains(openAIMessages, m => m.User == "Luis");
    }
}
