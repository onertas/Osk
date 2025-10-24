

using OpenAI;
using OpenAI.Managers;
using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;

namespace OskApi.Services.OpenAI
{
   
        public class ChatService
        {
            private readonly OpenAIService _openAIService;

            public ChatService(IConfiguration config)
            {
                var apiKey = config["OpenAI:ApiKey"];
                _openAIService = new OpenAIService(new OpenAiOptions
                {
                    ApiKey = "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"
                });
            }

            public async Task<string> AskAsync(string userPrompt)
            {
                var response = await _openAIService.ChatCompletion.CreateCompletion(
                    new ChatCompletionCreateRequest
                    {
                        Messages = new List<ChatMessage>
                        {
                        ChatMessage.FromUser(userPrompt)
                        },
                        Model = Models.Gpt_4o_mini // daha hızlı ve ucuz model
                    });

                return response.Choices.First().Message.Content;
            }
        }
    
}
