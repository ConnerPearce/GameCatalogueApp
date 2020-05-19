using System;
using System.Collections.Generic;
using System.Text;

namespace GameCatalogueApp.Classes._Custom_API.Data
{
    public class Wishlist : IWishlist
    {
        public string Id { get; set; }
        public string UId { get; set; }
        public string GameID { get; set; }
    }
}
