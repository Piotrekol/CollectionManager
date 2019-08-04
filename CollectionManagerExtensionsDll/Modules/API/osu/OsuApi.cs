using CollectionManager.DataTypes;
using CollectionManager.Enums;
using CollectionManagerExtensionsDll.DataTypes;
using CollectionManagerExtensionsDll.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;

namespace CollectionManagerExtensionsDll.Modules.API.osu
{
    public class OsuApi
    {

        private readonly ImpatientWebClient _client = new ImpatientWebClient();
        public string ApiKey { private get; set; }

        private const string ApiUrl = "https://osu.ppy.sh/api/";
        private const string GetBeatmapsURL = ApiUrl + "get_beatmaps";
        private const string GetUserURL = ApiUrl + "get_user";
        private const string GetScoresURL = ApiUrl + "get_scores";
        private const string GetUserBestURL = ApiUrl + "get_user_best";
        private const string GetUserRecentURL = ApiUrl + "get_user_recent";
        private const string GetMatchURL = ApiUrl + "get_match";

        private Dictionary<int, BeatmapExtension> _downloadedBeatmaps = new Dictionary<int, BeatmapExtension>();

        public OsuApi(string apiKey)
        {
            ApiKey = apiKey;
        }

        public Beatmaps GetBeatmaps(DateTime fromDate, DateTime toDate)
        {
            var resultBeatmaps = new Beatmaps();

            DateTime currentDate = fromDate;

            while (currentDate < toDate)
            {
                bool exception;
                RangeObservableCollection<BeatmapExtensionEx> newBeatmaps = null;
                do
                {
                    exception = false;
                    try
                    {
                        newBeatmaps =
                            GetBeatmaps(string.Format(GetBeatmapsURL + "?k={0}&since={1}", ApiKey,
                                currentDate.ToString("yyyy-MM-dd HH:mm:ss")));
                    }
                    catch (WebException)
                    {
                        exception = true;
                    }
                } while (exception);
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

        public IList<BeatmapExtensionEx> GetBeatmaps(int beatmapSetId, PlayMode? gamemode)
        {
            var link = GetBeatmapsURL + "?k=" + ApiKey + "&s=" + beatmapSetId;
            if (gamemode.HasValue)
                link += "&m=" + (int)gamemode;

            return GetBeatmaps(link);
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
                var approvedDate = json["approved_date"];
                beatmap.ApprovedDate = approvedDate != null ? approvedDate.Value<DateTime?>() ?? DateTime.MinValue : DateTime.MinValue;
                beatmap.ModPpStars.Add(PlayMode.Osu, new StarRating
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
            if (_downloadedBeatmaps.ContainsKey(beatmapId))
                return _downloadedBeatmaps[beatmapId];
            var map = GetBeatmapResult(GetBeatmapsURL + "?k=" + ApiKey + "&b=" + beatmapId);
            if (map != null)
                _downloadedBeatmaps.Add(beatmapId, map);
            return map;
        }

        public Beatmap GetBeatmap(int beatmapId, PlayMode? gamemode)
        {
            if (_downloadedBeatmaps.ContainsKey(beatmapId) && _downloadedBeatmaps[beatmapId].PlayMode == gamemode)
                return _downloadedBeatmaps[beatmapId];

            var link = GetBeatmapsURL + "?k=" + ApiKey + "&b=" + beatmapId;
            if (gamemode.HasValue)
                link += "&m=" + (int)gamemode;

            var map = GetBeatmapResult(link);

            if (map != null)
                _downloadedBeatmaps[beatmapId] = map;

            return map;
        }
        public Beatmap GetBeatmap(string hash, PlayMode? gamemode = null)
        {
            var link = GetBeatmapsURL + "?k=" + ApiKey + "&h=" + hash;
            if (gamemode.HasValue)
                link += "&m=" + (int)gamemode;

            return GetBeatmapResult(link);
        }
        public IList<ApiScore> GetUserBest(string username, PlayMode mode, int limit = 100)
        {
            return GetUserScoresResult(GetUserBestURL + "?k=" + ApiKey + "&u=" + username + "&m=" + (int)mode + "&type=string" + "&limit=" + limit);
        }
        private IList<ApiScore> GetUserScoresResult(string url)
        {
            try
            {
                var jsonResponse = _client.DownloadString(url);

                if (jsonResponse == "Please provide a valid API key.")
                    throw new Exception("Invalid osu!Api key");
                if (jsonResponse.Trim(' ') == string.Empty)
                    return null;
                var json = JArray.Parse(jsonResponse);
                var scores = new List<ApiScore>();
                foreach (var kvPair in json)
                {
                    var score = new ApiScore
                    {
                        BeatmapId = kvPair.Value<int>("beatmap_id"),
                        Score = kvPair.Value<int>("score"),
                        Count300 = kvPair.Value<int>("count300"),
                        Count100 = kvPair.Value<int>("count100"),
                        Count50 = kvPair.Value<int>("count50"),
                        Countmiss = kvPair.Value<int>("countmiss"),
                        Maxcombo = kvPair.Value<int>("maxcombo"),
                        Countkatu = kvPair.Value<int>("countkatu"),
                        Countgeki = kvPair.Value<int>("countgeki"),
                        Perfect = kvPair.Value<int>("perfect"),
                        EnabledMods = kvPair.Value<int>("enabled_mods"),
                        UserId = kvPair.Value<int>("user_id"),
                        Date = kvPair.Value<DateTime>("date"),
                        Rank = kvPair.Value<string>("rank"),
                        Pp = kvPair.Value<double>("pp")
                    };
                    scores.Add(score);
                }
                return scores;
            }
            catch
            {
                return null;
            }



        }
        private BeatmapExtension GetBeatmapResult(string url)
        {
            try
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
                beatmap.CircleSize = Convert.ToSingle(json["diff_size"].ToString(), CultureInfo.InvariantCulture);
                beatmap.OverallDifficulty = Convert.ToSingle(json["diff_overall"].ToString(), CultureInfo.InvariantCulture);
                beatmap.ApproachRate = Convert.ToSingle(json["diff_approach"].ToString(), CultureInfo.InvariantCulture);
                beatmap.HpDrainRate = Convert.ToSingle(json["diff_drain"].ToString(), CultureInfo.InvariantCulture);
                beatmap.PlayMode = (PlayMode)Convert.ToUInt32(json["mode"].ToString(), CultureInfo.InvariantCulture);
                //beatmap.bpm
                beatmap.Source = json["source"].ToString();
                beatmap.Tags = json["tags"].ToString();
                beatmap.ModPpStars.Add(beatmap.PlayMode, new StarRating()
                {
                    { 0, Math.Round(double.Parse(json["difficultyrating"].ToString(), CultureInfo.InvariantCulture), 2) }
                });
                beatmap.PlayMode = (PlayMode)Int32.Parse(json["mode"].ToString());
                //beatmap.OverallDifficulty = float.Parse(json["difficultyrating"].ToString(), );
                beatmap.DataDownloaded = true;

                return beatmap;
            }
            catch (WebException)
            {
                return null;
            }
        }
    }
}