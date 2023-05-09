using System;

namespace UnityExercise.MQ
{
    /// <summary>
    /// Configuration for RabbitMQ
    /// </summary>
    public class RabbitMQConfiguration
    {
        public string Hostname { get; set; }
        public string QueueName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
