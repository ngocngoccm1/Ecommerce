using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace App.Controllers
{
    public class ChatController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ChatController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetChatResponse(string userInput)
        {
            var apiKey = "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";  // Thay bằng API Key của bạn
            var client = _httpClientFactory.CreateClient();

            // Thiết lập tiêu đề HTTP
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            var requestBody = new
            {
                model = "gpt-3.5-turbo", // Chọn model phù hợp (có thể là gpt-4, gpt-3.5-turbo)
                messages = new[]
                {
                    new { role = "system", content = "You are a helpful assistant." },
                    new { role = "user", content = userInput }
                },
                max_tokens = 150
            };

            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            // Gửi yêu cầu HTTP tới API của OpenAI
            var response = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var chatResponse = System.Text.Json.JsonSerializer.Deserialize<OpenAIResponse>(responseContent);
                return Json(new { success = true, message = chatResponse.Choices[0].Message.Content });
            }
            else
            {
                return Json(new { success = false, message = "Error: " + response.ReasonPhrase });
            }
        }
    }

    public class OpenAIResponse
    {
        public class Choice
        {
            public Message Message { get; set; }
        }

        public class Message
        {
            public string Content { get; set; }
        }

        public Choice[] Choices { get; set; }
    }
}
