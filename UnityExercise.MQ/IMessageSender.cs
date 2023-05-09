using System.Threading.Tasks;

namespace UnityExercise.MQ
{
    /// <summary>
    /// Message sender can get this service from DI.
    /// </summary>
    public interface IMessageSender
    {
        /// <summary>
        /// Sends a message to message queue.
        /// </summary>
        Task SendMessageAsync<T>(T message);
    }
}
