using GameCatalogueApp.Pages.Home;

namespace GameCatalogueApp.Classes.ConnectionManager
{
    public interface ICheckConnection
    {
        bool hasConnection(HomePage.ErrorHandling connectionAlert);
    }
}