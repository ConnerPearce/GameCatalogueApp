using System;
using System.Collections.Generic;
using System.Text;

namespace GameCatalogueApp.Classes.API.Data
{
    public class SingleGameData
    {
    }

    public class Rating
    {
        public int id { get; set; }
        public string title { get; set; }
        public int count { get; set; }
        public double percent { get; set; }
    }

    public class Reactions
    {
        public int __invalid_name__2 { get; set; }
        public int __invalid_name__3 { get; set; }
        public int __invalid_name__7 { get; set; }
    }

    public class AddedByStatus
    {
        public int yet { get; set; }
        public int owned { get; set; }
        public int beaten { get; set; }
        public int toplay { get; set; }
        public int dropped { get; set; }
        public int playing { get; set; }
    }

    public class Platform
    {
        public int id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
    }

    public class ParentPlatform
    {
        public Platform platform { get; set; }
    }

    public class Platform3
    {
        public int id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public object image { get; set; }
        public object year_end { get; set; }
        public object year_start { get; set; }
        public int games_count { get; set; }
        public string image_background { get; set; }
    }

    public class Requirements
    {
        public string minimum { get; set; }
        public string recommended { get; set; }
    }

    public class Platform2
    {
        public Platform3 platform { get; set; }
        public string released_at { get; set; }
        public Requirements requirements { get; set; }
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
        public string url { get; set; }
        public Store2 store { get; set; }
    }

    public class Developer
    {
        public int id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public int games_count { get; set; }
        public string image_background { get; set; }
    }

    public class Genre
    {
        public int id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public int games_count { get; set; }
        public string image_background { get; set; }
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

    public class Publisher
    {
        public int id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public int games_count { get; set; }
        public string image_background { get; set; }
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

    public class SingleGameRootObject : ISingleGameRootObject
    {
        public int id { get; set; }
        public string slug { get; set; }
        public string name { get; set; }
        public string name_original { get; set; }
        public string description { get; set; }
        public int? metacritic { get; set; }
        public string released { get; set; }
        public bool tba { get; set; }
        public DateTime updated { get; set; }
        public string background_image { get; set; }
        public string background_image_additional { get; set; }
        public string website { get; set; }
        public double rating { get; set; }
        public int rating_top { get; set; }
        public List<Rating> ratings { get; set; }
        public Reactions reactions { get; set; }
        public int added { get; set; }
        public AddedByStatus added_by_status { get; set; }
        public int playtime { get; set; }
        public int screenshots_count { get; set; }
        public int movies_count { get; set; }
        public int creators_count { get; set; }
        public int achievements_count { get; set; }
        public string parent_achievements_count { get; set; }
        public string reddit_url { get; set; }
        public string reddit_name { get; set; }
        public string reddit_description { get; set; }
        public string reddit_logo { get; set; }
        public int reddit_count { get; set; }
        public string twitch_count { get; set; }
        public string youtube_count { get; set; }
        public string reviews_text_count { get; set; }
        public int ratings_count { get; set; }
        public int suggestions_count { get; set; }
        public List<string> alternative_names { get; set; }
        public string metacritic_url { get; set; }
        public int parents_count { get; set; }
        public int additions_count { get; set; }
        public int game_series_count { get; set; }
        public object user_game { get; set; }
        public int reviews_count { get; set; }
        public string saturated_color { get; set; }
        public string dominant_color { get; set; }
        public List<ParentPlatform> parent_platforms { get; set; }
        public List<Platform2> platforms { get; set; }
        public List<Store> stores { get; set; }
        public List<Developer> developers { get; set; }
        public List<Genre> genres { get; set; }
        public List<Tag> tags { get; set; }
        public List<Publisher> publishers { get; set; }
        public object esrb_rating { get; set; }
        public Clip clip { get; set; }
        public string description_raw { get; set; }
    }
}
