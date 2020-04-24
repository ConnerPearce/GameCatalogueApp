using System;
using System.Collections.Generic;

namespace GameCatalogueApp.Classes.API.Data
{
    public interface ISingleGameRootObject
    {
        int achievements_count { get; set; }
        int added { get; set; }
        AddedByStatus added_by_status { get; set; }
        int additions_count { get; set; }
        List<string> alternative_names { get; set; }
        string background_image { get; set; }
        string background_image_additional { get; set; }
        Clip clip { get; set; }
        int creators_count { get; set; }
        string description { get; set; }
        string description_raw { get; set; }
        List<Developer> developers { get; set; }
        string dominant_color { get; set; }
        object esrb_rating { get; set; }
        int game_series_count { get; set; }
        List<Genre> genres { get; set; }
        int id { get; set; }
        int metacritic { get; set; }
        string metacritic_url { get; set; }
        int movies_count { get; set; }
        string name { get; set; }
        string name_original { get; set; }
        string parent_achievements_count { get; set; }
        List<ParentPlatform> parent_platforms { get; set; }
        int parents_count { get; set; }
        List<Platform2> platforms { get; set; }
        int playtime { get; set; }
        List<Publisher> publishers { get; set; }
        double rating { get; set; }
        int rating_top { get; set; }
        List<Rating> ratings { get; set; }
        int ratings_count { get; set; }
        Reactions reactions { get; set; }
        int reddit_count { get; set; }
        string reddit_description { get; set; }
        string reddit_logo { get; set; }
        string reddit_name { get; set; }
        string reddit_url { get; set; }
        string released { get; set; }
        int reviews_count { get; set; }
        string reviews_text_count { get; set; }
        string saturated_color { get; set; }
        int screenshots_count { get; set; }
        string slug { get; set; }
        List<Store> stores { get; set; }
        int suggestions_count { get; set; }
        List<Tag> tags { get; set; }
        bool tba { get; set; }
        string twitch_count { get; set; }
        DateTime updated { get; set; }
        object user_game { get; set; }
        string website { get; set; }
        string youtube_count { get; set; }
    }
}