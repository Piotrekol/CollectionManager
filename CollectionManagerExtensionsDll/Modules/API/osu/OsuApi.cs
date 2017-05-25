using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using CollectionManager.DataTypes;
using CollectionManager.Enums;
using CollectionManagerExtensionsDll.DataTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CollectionManagerExtensionsDll.Modules.API.osu
{
    public class OsuApi
    {
        private readonly WebClient _client = new WebClient();
        private string _apiKey;

        private const string ApiUrl = "https://osu.ppy.sh/api/";
        private const string GetBeatmapsURL = ApiUrl + "get_beatmaps";
        private const string GetUserURL = ApiUrl + "get_user";
        private const string GetScoresURL = ApiUrl + "get_scores";
        private const string GetUserBestURL = ApiUrl + "get_user_best";
        private const string GetUserRecentURL = ApiUrl + "get_user_recent";
        private const string GetMatchURL = ApiUrl + "get_match";
        public OsuApi(string apiKey)
        {
            _apiKey = apiKey;
        }
        #region get_beatmaps
        public Beatmaps GetBeatmaps(DateTime fromDate, DateTime toDate)
        {
            var resultBeatmaps = new Beatmaps();

            DateTime currentDate = fromDate;

            while (currentDate < toDate)
            {
                var newBeatmaps = GetBeatmaps(string.Format(GetBeatmapsURL + "?k={0}&since={1}", _apiKey, currentDate.ToString("yyyy-MM-dd HH:mm:ss")));

                if (newBeatmaps.Count < 500)
                    currentDate = toDate;
                foreach (var newBeatmap in newBeatmaps)
                {
                    if (newBeatmap.ApprovedDate < toDate)
                    {
                        resultBeatmaps.Add(newBeatmap);
                    }

                    if (currentDate < newBeatmap.ApprovedDate)
                    {
                        currentDate = newBeatmap.ApprovedDate;
                    }
                }
            }
            return resultBeatmaps;
        }

        private RangeObservableCollection<BeatmapExtensionEx> GetBeatmaps(string url)
        {
            var beatmaps = new RangeObservableCollection<BeatmapExtensionEx>();

            var jsonResponse = _client.DownloadString(url);
            if (jsonResponse == "Please provide a valid API key.")
                throw new Exception("Invalid osu!Api key");
            //jsonResponse = jsonResponse.Trim(']', '[');
            if (jsonResponse.Trim(' ') == string.Empty)
                return null;
            var jsonArray = JArray.Parse(jsonResponse);

            foreach (var json in jsonArray)
            {
                //TODO: I'm pretty sure there's a nice and automated way to do this in Newtonsoft.
                var beatmap = new BeatmapExtensionEx();
                beatmap.MapSetId = int.Parse(json["beatmapset_id"].ToString());
                beatmap.MapId = int.Parse(json["beatmap_id"].ToString());
                beatmap.DiffName = json["version"].ToString();
                beatmap.Md5 = json["file_md5"].ToString();
                beatmap.ArtistRoman = json["artist"].ToString();
                beatmap.TitleRoman = json["title"].ToString();
                beatmap.Creator = json["creator"].ToString();
                beatmap.ApprovedDate = DateTime.ParseExact(json["approved_date"].ToString(), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                beatmap.ModPpStars.Add(0, Math.Round(double.Parse(json["difficultyrating"].ToString(), CultureInfo.InvariantCulture), 2));
                beatmap.GenreId = Int32.Parse(json["genre_id"].ToString());
                beatmap.LanguageId = Int32.Parse(json["language_id"].ToString());
                beatmap.DataDownloaded = true;

                beatmaps.Add(beatmap);
            }
            return beatmaps;
        }

        public BeatmapExtensionEx GetBeatmap(int beatmapId)
        {
            var maps = GetBeatmaps(GetBeatmapsURL + "?k=" + _apiKey + "&b=" + beatmapId);
            return maps.Count > 0 ? maps[0] : null;
        }
        public BeatmapExtensionEx GetBeatmap(string hash)
        {
            var maps = GetBeatmaps(GetBeatmapsURL + "?k=" + _apiKey + "&h=" + hash);
            return maps.Count > 0 ? maps[0] : null;
        }

        #endregion

        #region get_user_best
        public List<OnlineScore> GetUserBest(string username, PlayModes mode, int limit = 100)
        {
            return GetUserScoresResult(GetUserBestURL + "?k=" + _apiKey + "&u=" + username + "&m=" + mode + "&type=string" + "&limit=" + limit, username);
        }
        private List<OnlineScore> GetUserScoresResult(string url,string username)
        {
            var jsonResponse = _client.DownloadString(url);

            if (jsonResponse == "Please provide a valid API key.")
                throw new Exception("Invalid osu!Api key");
            if (jsonResponse.Trim(' ') == string.Empty)
                return null;
            var json = JArray.Parse(jsonResponse);
            var scores = new List<OnlineScore>();
            foreach (var kvPair in json)
            {
                //TODO: I'm pretty sure there's a nice and automated way to do this in Newtonsoft.
                var score = new OnlineScore();
                score.BeatmapId = kvPair.Value<int>("beatmap_id");
                score.Score = kvPair.Value<int>("score");
                score.Count300 = kvPair.Value<int>("count300");
                score.Count100 = kvPair.Value<int>("count100");
                score.Count50 = kvPair.Value<int>("count50");
                score.Countmiss = kvPair.Value<int>("countmiss");
                score.Maxcombo = kvPair.Value<int>("maxcombo");
                score.Countkatu = kvPair.Value<int>("countkatu");
                score.Countgeki = kvPair.Value<int>("countgeki");
                score.Perfect = kvPair.Value<int>("perfect");
                score.EnabledMods = kvPair.Value<int>("enabled_mods");
                score.UserId = kvPair.Value<int>("user_id");
                score.Date = kvPair.Value<DateTime>("date");
                score.Rank = kvPair.Value<string>("rank");
                score.Pp = kvPair.Value<double>("pp");
                score.Username = username;
                scores.Add(score);
            }
            return scores;
        }

        #endregion
    }
}