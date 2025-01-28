using ChatV1.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChatV1.Controllers
{
    public class MessageController : ControllerBase
    {
        private readonly KafkaProducerService _producerService;

        public MessageController(KafkaProducerService producerService)
        {
            _producerService = producerService;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] string message)
        {
            await _producerService.SendMessageAsync("test_topic", message);

            return Ok("Message was sent to Kafka");
        }
    }
}
