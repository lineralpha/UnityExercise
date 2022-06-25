using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityExercise.MQ
{
    /// <summary>
    /// Message receiver should implement this interface and register the implementation
    /// type to DI. For this exercise there is only one receiver.
    /// </summary>
    public interface IMessageReceiver
    {
        Task HandleMessageAsync(string message);
    }
}
