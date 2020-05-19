using System;
using System.Collections.Generic;
using System.Text;

namespace GameCatalogueApp.Classes._Custom_API.Data
{
    public class User : IUser
    {
        public string Id { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Email { get; set; }
        public string Pwrd { get; set; }
        public string UName { get; set; }
    }
}
