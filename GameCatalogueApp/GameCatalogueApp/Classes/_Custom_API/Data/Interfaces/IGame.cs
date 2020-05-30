using System;

namespace GameCatalogueApp.Classes._Custom_API.Data
{
    public interface IGame
    {
        string Developer { get; set; }
        string Genre { get; set; }
        string Id { get; set; }
        string Name { get; set; }
        string[] Platform { get; set; }
        double? Rating { get; set; }
        DateTime ReleaseDate { get; set; }
        string Summary { get; set; }
        string Image { get; set; }
    }
}