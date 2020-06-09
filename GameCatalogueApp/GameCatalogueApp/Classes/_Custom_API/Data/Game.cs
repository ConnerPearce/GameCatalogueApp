using System;
using System.Collections.Generic;
using System.Text;

namespace GameCatalogueApp.Classes._Custom_API.Data
{
    public class Game : IGame
    {
        public string id { get; set; }
        public string name { get; set; }
        public string Summary { get; set; }
        public string Genre { get; set; }
        public string Developer { get; set; }
        public double? Rating { get; set; } // Can be null as some games dont have ratings (NOT AGE RATING)
        public string[] Platform { get; set; }
        public DateTime released { get; set; }
        public string background_image { get; set; }
    }
}
