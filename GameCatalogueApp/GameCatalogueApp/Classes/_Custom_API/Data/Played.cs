using System;
using System.Collections.Generic;
using System.Text;

namespace GameCatalogueApp.Classes._Custom_API.Data
{
    public class Played : IPlayed
    {
        public string Id { get; set; }
        public string UId { get; set; }
        public string GameID { get; set; }
    }
}
