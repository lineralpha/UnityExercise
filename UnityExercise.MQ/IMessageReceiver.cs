using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityExercise.MQ
{
    /// <summary>
    /// Message receiver should implement this interface and register the implementation
    /// type to DI.
    /// </summary>
    public interface IMessageReceiver
    {
        /// <summary>
        /// A message receiver implements this method to handle arriving message.
        /// </summary>
        Task HandleMessageAsync(string message);
    }
}
