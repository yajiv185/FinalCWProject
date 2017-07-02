using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Entities
{
    [Serializable, DataContract]
    public class ReadStock
    {
        [DataMember]
        [JsonProperty("price")]
        public int ID { get; set; }
        [DataMember]
        [JsonProperty("price")]
        public int Price { get; set; }
        [DataMember]
        [JsonProperty("year")]
        public int Year { get; set; }
        [DataMember]
        [JsonProperty("kilometers")]
        public int Kilometers { get; set; }
        [DataMember]
        [JsonProperty("fueltype")]
        public string FuelType { get; set; }
        [DataMember]
        [JsonProperty("city")]
        public string City { get; set; }
        [DataMember]
        [JsonProperty("color")]
        public string Color { get; set; }
        [DataMember]
        [JsonProperty("fueleconomy")]
        public float FuelEconomy { get; set; }
        [DataMember]
        [JsonProperty("carcompany")]
        public string carcompany { get; set; }
        [DataMember]
        [JsonProperty("modelname")]
        public string modelname { get; set; }
        [DataMember]
        [JsonProperty("carversionname")]
        public string carversionname { get; set; }
    }
}