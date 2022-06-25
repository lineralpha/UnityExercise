using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityExercise.Services.Data;

namespace UnityExercise.Services
{
    public class PayloadService : IPayloadService
    {
        private readonly AppDbContext _dbContext;

        public PayloadService(AppDbContext dbcontext)
        {
            _dbContext = dbcontext;
        }
        public async Task<int> CreatePayloadAsync(PayloadCreateInput input)
        {
            var payload = new Payload()
            {
                Message = JsonConvert.SerializeObject(input, new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore
                }),
            };

            _dbContext.Payloads.Add(payload);
            await _dbContext.SaveChangesAsync();

            return payload.Id;
        }

        public async Task<int> CreatePayloadAsync(string input)
        {
            var payload = new Payload()
            {
                Message = input
            };

            _dbContext.Payloads.Add(payload);
            await _dbContext.SaveChangesAsync();
            return payload.Id;
        }
    }
}
