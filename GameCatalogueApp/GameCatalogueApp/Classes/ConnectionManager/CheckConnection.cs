using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace GameCatalogueApp.Classes.ConnectionManager
{
    public class CheckConnection
    {
        public delegate void ConnectionAlert(string message);

        public bool hasConnection(ConnectionAlert connectionAlert)
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.Internet)
            {
                return true;
            }
            else
            {
                connectionAlert("No Connection, Please connect to the internet");
                return false;
            }
        }


 
    }
}
