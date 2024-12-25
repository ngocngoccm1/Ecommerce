using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/chatbot")]
public class ChatbotController : ControllerBase
{
    private readonly HuggingFaceService _huggingFaceService;

    public ChatbotController(HuggingFaceService huggingFaceService)
    {
        _huggingFaceService = huggingFaceService;
    }

    [HttpPost]
    public async Task<IActionResult> Chat([FromBody] string userInput)
    {
        try
        {
            var botResponse = await _huggingFaceService.GetResponseFromGPT(userInput);
            return Ok(new { botResponse });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
}
