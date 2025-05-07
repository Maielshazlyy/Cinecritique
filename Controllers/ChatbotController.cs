using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace CineCritique.Api.Controllers
{
    
    
        [Route("api/chatbot")]
        [ApiController]
        public class ChatBotController : Controller
        {
            private readonly string apiKey = "AIzaSyBm2Ap0vvWgikf5UNAvtYKSoLWMq51-chQ";  // استبدليه بمفتاح API الخاص بك
            private readonly string model = "models/gemini-2.0-pro-exp";
            private readonly string systemPrompt = "You are a helpful assistant that only recommends movies or helps users find films by genre, actor, or mood. Do not answer questions outside this context.";

            private readonly HttpClient _httpClient;

            public ChatBotController(HttpClient httpClient)
            {
                _httpClient = httpClient;
            }

            [HttpPost("chat")]
            public async Task<IActionResult> GetChatResponse([FromBody] ChatRequest request)
            {
                string userInput = request.Message;

                // Create the payload for Gemini API
                var requestBody = new
                {
                    contents = new[]
                    {
                    new {
                        parts = new[] {
                            new { text = systemPrompt },
                            new { text = userInput }
                        }
                    }
                }
                };

                var geminiUrl = $"https://generativelanguage.googleapis.com/v1beta/{model}:generateContent?key={apiKey}";

                // Send the POST request to Gemini API
                var jsonPayload = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(geminiUrl, content);
                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, "Failed to get response from Gemini API");
                }

                var responseBody = await response.Content.ReadAsStringAsync();

                // Parse the response from Gemini API
                var resultJson = JsonDocument.Parse(responseBody);
                var geminiReply = resultJson.RootElement
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString();

                return Ok(new { response = geminiReply });
            }
        }

        public class ChatRequest
        {
            public string Message { get; set; }
        }
    }

