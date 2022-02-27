using System;
using Newtonsoft.Json;
    
namespace Palota.Assessment.Countries.Models {
    public class Location {
        [JsonProperty("lattitude")]
        public object Lattitude { get; set; }

        [JsonProperty("longitude")]
        public object Longitude { get; set; }
    }
}