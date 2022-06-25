using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityExercise.Services
{
    public interface IPayloadService
    {
        Task<int> CreatePayloadAsync(PayloadCreateInput input);
        Task<int> CreatePayloadAsync(string payload);
    }
}
