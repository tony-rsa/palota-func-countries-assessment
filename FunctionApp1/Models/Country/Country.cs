using System;
using Newtonsoft.Json;
    
namespace Palota.Assessment.Countries.Models {
    public class Country
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("iso3Code")]
        public string Iso3Code { get; set; }

        [JsonProperty("capital")]
        public string Capital { get; set; }

        [JsonProperty("subregion")]
        public string Subregion { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("population")]
        public int Population { get; set; }

        [JsonProperty("location")]
        public object Location { get; set; }

        [JsonProperty("demonym")]
        public string Demonym { get; set; }

        [JsonProperty("nativeName")]
        public string NativeName { get; set; }

        [JsonProperty("numericCode")]
        public string NumericCode { get; set; }

        [JsonProperty("flag")]
        public string Flag { get; set; }
    }
}
