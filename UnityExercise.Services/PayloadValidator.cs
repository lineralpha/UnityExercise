using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.IO;

namespace UnityExercise.Services
{
    public static class PayloadValidator
    {
        private static readonly Lazy<JSchema> _schema = new Lazy<JSchema>(() =>
        {
            // embbeded resources: UnityExercise.Services.payload-schema.json
            var stream = typeof(PayloadValidator).Assembly.GetManifestResourceStream("UnityExercise.Services.payload-schema.json");
            using StreamReader sr = new StreamReader(stream);
            using JsonReader reader = new JsonTextReader(sr);
            return JSchema.Load(reader);
        });

        public static bool Validate(PayloadCreateInput input, out IList<string> errors)
        {
            try
            {
                JObject json = JObject.Parse(
                    JsonConvert.SerializeObject(input,
                    new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                    }));

                bool result = json.IsValid(_schema.Value, out errors);

                if (result)
                {
                    // ensure the ts has valid unix timestamp value
                    string ts = json.SelectToken("ts")?.ToString();
                    int.Parse(ts); // throws if ts is out of range
                }

                return result;
            }
            catch (Exception ex)
            {
                errors = new List<string>
                {
                    "ts: " + ex.Message
                };
                return false;
            }
        }

    }
}
