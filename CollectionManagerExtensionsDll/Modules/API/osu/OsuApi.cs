using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using CollectionManager.DataTypes;
using CollectionManager.Enums;
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
                var beatmap = new BeatmapExtensionEx();
                beatmap.MapSetId = int.Parse(json["beatmapset_id"].ToString());
                beatmap.MapId = int.Parse(json["beatmap_id"].ToString());
                beatmap.DiffName = json["version"].ToString();
                beatmap.Md5 = json["file_md5"].ToString();
                beatmap.ArtistRoman = json["artist"].ToString();
                beatmap.TitleRoman = json["title"].ToString();
                beatmap.Creator = json["creator"].ToString();
                beatmap.ApprovedDate = DateTime.ParseExact(json["approved_date"].ToString(), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                beatmap.ModPpStars.Add(PlayMode.Osu, new Dictionary<int, double>()
                {
                    { 0, Math.Round(double.Parse(json["difficultyrating"].ToString(), CultureInfo.InvariantCulture), 2) }
                });
                beatmap.GenreId = Int32.Parse(json["genre_id"].ToString());
                beatmap.LanguageId = Int32.Parse(json["language_id"].ToString());
                beatmap.PlayMode = (PlayMode)Int32.Parse(json["mode"].ToString());
                beatmap.DataDownloaded = true;

                beatmaps.Add(beatmap);
            }
            return beatmaps;
        }

        public Beatmap GetBeatmap(int beatmapId)
        {
            return GetBeatmapResult(GetBeatmapsURL + "?k=" + _apiKey + "&b=" + beatmapId);
        }
        public Beatmap GetBeatmap(string hash)
        {
            return GetBeatmapResult(GetBeatmapsURL + "?k=" + _apiKey + "&h=" + hash);
        }

        private Beatmap GetBeatmapResult(string url)
        {
            var jsonResponse = _client.DownloadString(url);
            if (jsonResponse == "Please provide a valid API key.")
                throw new Exception("Invalid osu!Api key");
            jsonResponse = jsonResponse.Trim(']', '[');
            if (jsonResponse.Trim(' ') == string.Empty)
                return null;
            var json = JObject.Parse(jsonResponse);
            var beatmap = new BeatmapExtension();
            //var a = json.Count;
            beatmap.MapSetId = int.Parse(json["beatmapset_id"].ToString());
            beatmap.MapId = int.Parse(json["beatmap_id"].ToString());
            beatmap.DiffName = json["version"].ToString();
            beatmap.Md5 = json["file_md5"].ToString();
            beatmap.ArtistRoman = json["artist"].ToString();
            beatmap.TitleRoman = json["title"].ToString();
            beatmap.Creator = json["creator"].ToString();
            beatmap.ModPpStars.Add(PlayMode.Osu, new Dictionary<int, double>()
            {
                { 0, Math.Round(double.Parse(json["difficultyrating"].ToString(), CultureInfo.InvariantCulture), 2) }
            });
            //beatmap.OverallDifficulty = float.Parse(json["difficultyrating"].ToString(), );
            beatmap.DataDownloaded = true;

            return beatmap;
        }
    }
}