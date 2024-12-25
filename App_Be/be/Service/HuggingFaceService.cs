using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

public class HuggingFaceService
{
    private readonly HttpClient _httpClient;

    public HuggingFaceService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetResponseFromGPT(string userInput)
    {
        string huggingFaceApiUrl = "https://api-inference.huggingface.co/models/microsoft/DialoGPT-medium";
        string apiKey = "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";

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        var requestData = new
        {
            inputs = userInput,
            history = new[]
            {
                new { role = "user", content = "Hi, how are you?" },
                new { role = "assistant", content = "I'm good, thanks! How about you?" }
            }
        };

        var jsonContent = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(huggingFaceApiUrl, jsonContent);

        if (response.IsSuccessStatusCode)
        {
            var responseJson = await response.Content.ReadAsStringAsync();
            dynamic responseObject = JsonConvert.DeserializeObject(responseJson);

            // Trích xuất phản hồi
            return responseObject[0]?.generated_text ?? "No response received.";
        }
        else
        {
            return $"Error: {response.ReasonPhrase}";
        }
    }
}
