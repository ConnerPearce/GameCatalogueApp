using System;
using System.Collections.Generic;
using System.Text;

namespace GameCatalogueApp.API.Data
{
    public class GameData // Contains all the data for each Game
    {
    }

    public class Rating
    {
        public int id { get; set; }
        public string title { get; set; }
        public int count { get; set; }
        public double percent { get; set; }
    }

    public class AddedByStatus
    {
        public double yet { get; set; }
        public double owned { get; set; }
        public double beaten { get; set; }
        public double toplay { get; set; }
        public double dropped { get; set; }
        public double playing { get; set; }
    }

    public class Platform2
    {
        public int id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public double games_count { get; set; }
        public string image_background { get; set; }
    }

    public class RequirementsEn
    {
        public string minimum { get; set; }
        public string recommended { get; set; }
    }

    public class RequirementsRu
    {
        public string minimum { get; set; }
        public string recommended { get; set; }
    }

    public class Platform
    {
        public Platform2 platform { get; set; }
        public string released_at { get; set; }
        public RequirementsEn requirements_en { get; set; }
        public RequirementsRu requirements_ru { get; set; }
    }

    public class Platform3
    {
        public int id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
    }

    public class ParentPlatform
    {
        public Platform3 platform { get; set; }
    }

    public class Genre
    {
        public int id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public int games_count { get; set; }
        public string image_background { get; set; }
    }

    public class Store2
    {
        public int id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public string domain { get; set; }
        public int games_count { get; set; }
        public string image_background { get; set; }
    }

    public class Store
    {
        public int id { get; set; }
        public Store2 store { get; set; }
        public string url_en { get; set; }
        public string url_ru { get; set; }
    }

    public class Clips
    {
        public string __invalid_name__320 { get; set; }
        public string __invalid_name__640 { get; set; }
        public string full { get; set; }
    }

    public class Clip
    {
        public string clip { get; set; }
        public Clips clips { get; set; }
        public string video { get; set; }
        public string preview { get; set; }
    }

    public class Tag
    {
        public int id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public string language { get; set; }
        public int games_count { get; set; }
        public string image_background { get; set; }
    }

    public class ShortScreenshot
    {
        public int id { get; set; }
        public string image { get; set; }
    }

    public class Result
    {
        public int id { get; set; }
        public string slug { get; set; }
        public string name { get; set; }
        public string released { get; set; }
        public bool tba { get; set; }
        public string background_image { get; set; }
        public double rating { get; set; }
        public double rating_top { get; set; }
        public List<Rating> ratings { get; set; }
        public double ratings_count { get; set; }
        public double reviews_text_count { get; set; }
        public double added { get; set; }
        public AddedByStatus added_by_status { get; set; }
        public double? metacritic { get; set; }
        public double playtime { get; set; }
        public double suggestions_count { get; set; }
        public double reviews_count { get; set; }
        public string saturated_color { get; set; }
        public string dominant_color { get; set; }
        public List<Platform> platforms { get; set; }
        public List<ParentPlatform> parent_platforms { get; set; }
        public List<Genre> genres { get; set; }
        public List<Store> stores { get; set; }
        public Clip clip { get; set; }
        public List<Tag> tags { get; set; }
        public List<ShortScreenshot> short_screenshots { get; set; }
    }

    public class Year2
    {
        public DateTime year { get; set; }
        public double count { get; set; }
        public bool nofollow { get; set; }
    }

    public class Year
    {
        public DateTime from { get; set; }
        public DateTime to { get; set; }
        public string filter { get; set; }
        public int decade { get; set; }
        public List<Year2> years { get; set; }
        public bool nofollow { get; set; }
        public double count { get; set; }
    }

    public class Filters
    {
        public List<Year> years { get; set; }
    }

    public class GameRootObject : IGameRootObject
    {
        public double count { get; set; }
        public string next { get; set; }
        public List<Result> results { get; set; }
        public string seo_title { get; set; }
        public string seo_description { get; set; }
        public string seo_keywords { get; set; }
        public string seo_h1 { get; set; }
        public bool noindex { get; set; }
        public bool nofollow { get; set; }
        public string description { get; set; }
        public Filters filters { get; set; }
        public List<string> nofollow_collections { get; set; }
    }
}
