using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Entities
{
    [Serializable]
    public class ReadStock
    {
        
        [JsonProperty("id")]
        public int ID { get; set; }
        [JsonProperty("price")]
        public int Price { get; set; }
        [JsonProperty("year")]
        public int Year { get; set; }
        [JsonProperty("kilometers")]
        public int Kilometers { get; set; }
        [JsonProperty("fueltype")]
        public string FuelType { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }
        [JsonProperty("color")]
        public string Color { get; set; }
        [JsonProperty("fueleconomy")]
        public float FuelEconomy { get; set; }
        [JsonProperty("carcompany")]
        public string CarCompany { get; set; }
        [JsonProperty("modelname")]
        public string ModelName { get; set; }
        [JsonProperty("carversionname")]
        public string CarVersion { get; set; }

        public int ImageCount { get; set; }
    }
}