using System.Threading.Tasks;

namespace UnityExercise.MQ
{
    /// <summary>
    /// Message sender can get this service from DI.
    /// </summary>
    public interface IMessageSender
    {
        Task SendMessageAsync<T>(T message);
    }
}
