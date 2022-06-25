using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;


namespace UnityExercise.Services
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class MessageContentInput
    {
        public string Foo { get; set; }
        public string Baz { get; set; }
    }

    /// <summary>
    /// The DTO type for creating entities.
    /// </summary>
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class PayloadCreateInput 
    {
        [Required]
        public string Ts { get; set; }

        [Required]
        public string Sender { get; set; }

        [Required]
        public MessageContentInput Message { get; set; }

        [JsonProperty("sent-from-ip")]
        public string SentFromIp { get; set; }

        public int? Priority { get; set; }
    }
}
