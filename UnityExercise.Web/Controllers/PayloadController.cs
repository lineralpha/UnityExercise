using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityExercise.MQ;
using UnityExercise.Services;

namespace UnityExercise.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PayloadController : ControllerBase
    {
        private readonly ILogger<PayloadController> _logger;
        private readonly IPayloadService _payloadService;
        private readonly IMessageSender _messageSender;

        public PayloadController(
            ILogger<PayloadController> logger,
            IPayloadService payloadService,
            IMessageSender messageSender)
        {
            _logger = logger;
            _payloadService = payloadService;
            _messageSender = messageSender;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult<PayloadCreateInput>> CreatePayload([FromBody]PayloadCreateInput input)
        {
            if (input == null)
            {
                return BadRequest("no input");
            }

            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            if (!PayloadValidator.Validate(input, out IList<string> errors))
            {
                return BadRequest(errors);
            }

            // int id = await _payloadService.CreatePayloadAsync(input);
            // return CreatedAtAction("Details", new { id = id }, input);

            await _messageSender.SendMessageAsync<PayloadCreateInput>(input);
            return CreatedAtAction("Details", input);
        }

        /// <summary>
        /// For demo only.
        /// </summary>
        [HttpGet]
        [Route("Details/{id?}")]
        public async Task<ActionResult> Details(int? id)
        {
            return await Task.FromResult(NotFound());
        }
    }
}
