using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OskApi.Services.OpenAI;

namespace OskApi.Controllers
{
    

        [ApiController]
        [Route("api/[controller]")]
        public class ChatController : ControllerBase
        {
            private readonly ChatService _chatService;

            public ChatController(ChatService chatService)
            {
                _chatService = chatService;
            }

            [HttpGet("ask")]
            public async Task<IActionResult> Ask( string prompt)
            {
                var answer = await _chatService.AskAsync(prompt);
                return Ok(new { response = answer });
            }
        }
    
}
