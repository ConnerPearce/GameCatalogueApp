using System.Collections.Generic;

namespace GameCatalogueApp.API.Data
{
    public interface IGameRootObject
    {
        double count { get; set; }
        string description { get; set; }
        Filters filters { get; set; }
        string next { get; set; }
        bool nofollow { get; set; }
        List<string> nofollow_collections { get; set; }
        bool noindex { get; set; }
        List<Result> results { get; set; }
        string seo_description { get; set; }
        string seo_h1 { get; set; }
        string seo_keywords { get; set; }
        string seo_title { get; set; }
    }
}