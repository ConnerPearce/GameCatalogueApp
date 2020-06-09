using GameCatalogueApp.Pages.Home;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace GameCatalogueApp.Classes.ConnectionManager
{
    public class CheckConnection : ICheckConnection
    {

        // This class checks the users internet connection
        public bool hasConnection(HomePage.ErrorHandling connectionAlert)
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
