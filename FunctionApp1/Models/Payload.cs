using System;
using Newtonsoft.Json;
    
namespace Palota.Assessment.Countries.Models {
    public class Payload {
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}