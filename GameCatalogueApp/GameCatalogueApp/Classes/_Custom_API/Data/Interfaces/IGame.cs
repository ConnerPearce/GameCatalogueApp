using System;

namespace GameCatalogueApp.Classes._Custom_API.Data
{
    public interface IGame
    {
        string Developer { get; set; }
        string Genre { get; set; }
        string id { get; set; }
        string name { get; set; }
        string[] Platform { get; set; }
        double? Rating { get; set; }
        DateTime released { get; set; }
        string Summary { get; set; }
        string background_image { get; set; }
    }
}