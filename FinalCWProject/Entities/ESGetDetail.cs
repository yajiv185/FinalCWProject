using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Entities
{
    public class ESGetDetail
    {
        public int ID { get; set; }

        public int Price { get; set; }

        public int Year { get; set; }

        public int Kilometers { get; set; }

        public string FuelType { get; set; }

        public string City { get; set; }

        public string carcompany { get; set; }

        public string modelname { get; set; }

        public string carversionname { get; set; }
    }
}