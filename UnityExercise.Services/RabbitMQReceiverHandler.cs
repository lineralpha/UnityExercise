using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityExercise.MQ;

namespace UnityExercise.Services
{
    public class RabbitMQReceiverHandler : IMessageReceiver
    {
        private readonly IPayloadService _payloadService;

        public RabbitMQReceiverHandler(IPayloadService payloadService)
        {
            _payloadService = payloadService;
        }
        public async Task HandleMessageAsync(string message)
        {
            await _payloadService.CreatePayloadAsync(message);
        }
    }
}
