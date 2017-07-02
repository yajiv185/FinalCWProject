using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UsedCarUI.Models
{
    public class CitiesList
    {
        public citylist cities { get; set; }
    }
    public enum citylist
    {
        Mumbai,
        Delhi,
        Indore,
        Nagpur,
        Bangalore,
        Aurangabad
    }
}