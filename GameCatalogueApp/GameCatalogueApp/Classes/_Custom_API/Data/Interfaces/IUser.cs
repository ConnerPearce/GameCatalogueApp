namespace GameCatalogueApp.Classes._Custom_API.Data
{
    public interface IUser
    {
        string Email { get; set; }
        string FName { get; set; }
        string Id { get; set; }
        string LName { get; set; }
        string Pwrd { get; set; }
        string UName { get; set; }
    }
}