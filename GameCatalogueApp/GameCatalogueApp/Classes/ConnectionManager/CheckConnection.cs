using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace GameCatalogueApp.Classes.ConnectionManager
{
    public class CheckConnection : ICheckConnection
    {
        // This delegate will return an error message to be popped up in the apps Xaml Page
        public delegate void ConnectionAlert(string message);

        // This class checks the users internet connection
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
