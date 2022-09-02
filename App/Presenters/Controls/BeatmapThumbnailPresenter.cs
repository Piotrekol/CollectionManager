using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Threading;
using CollectionManager.DataTypes;
using CollectionManagerExtensionsDll.Utils;
using App.Interfaces;
using GuiComponents.Interfaces;
using System.Threading.Tasks;
using Common;
using System.IO;
using CollectionManagerExtensionsDll.Modules.API.StreamCompanion;

namespace App.Presenters.Controls
{
    public class BeatmapThumbnailPresenter
    {
        private IBeatmapThumbnailView _view;
        private readonly IBeatmapThumbnailModel _model;
        private readonly object _lockingObject = new object();
        private BeatmapExtension _currentBeatmap = null;
        private readonly MRUFileCache fileCache;
        private readonly StreamCompanionApi StreamCompanionApi;
        private bool performanceCalculatorAvailable = false;

        public BeatmapThumbnailPresenter(IBeatmapThumbnailView view, IBeatmapThumbnailModel model)
        {
            _view = view;

            _model = model;
            _model.BeatmapChanged += _model_BeatmapChanged;
            fileCache = new MRUFileCache(Path.Combine(Path.GetTempPath(), "CM-map-previews"));
            StreamCompanionApi = new StreamCompanionApi();
        }

        private void _model_BeatmapChanged(object sender, EventArgs e)
        {
            lock (_lockingObject)
            {
                _currentBeatmap = (BeatmapExtension)_model.CurrentBeatmap;
                if (_currentBeatmap == null)
                {
                    //SetViewData(null, null);
                    return;
                }
                LoadData(_currentBeatmap);
            }
        }

        private long changeId = 0;
        private void LoadData(BeatmapExtension beatmap)
        {
            string imgPath = beatmap.GetImageLocation();
            BackgroundWorker bw = new BackgroundWorker();
            var currentId = Interlocked.Increment(ref changeId);
            bw.DoWork += async (s, e) =>
            {
                if (beatmap.Equals(_currentBeatmap))
                {
                    Image img = null;
                    string url = null;
                    if (imgPath != "")
                        img = Image.FromFile(imgPath);
                    else if (beatmap.MapSetId > 0)
                        url = "https://assets.ppy.sh/beatmaps/" + beatmap.MapSetId + "/covers/card.jpg";
                    if (currentId != Interlocked.Read(ref changeId))
                    {
                        img?.Dispose();
                        return;
                    }

                    SetViewData(img, beatmap, url);
                    await LoadAndSetPerformanceStats(beatmap);
                }
            };
            bw.RunWorkerAsync();
        }

        private async Task LoadAndSetPerformanceStats(BeatmapExtension beatmap)
        {
            if (!performanceCalculatorAvailable)
            {
                _view.AdditionalStatsRtf = getRtfString("Checking for running performance calculator...");
                performanceCalculatorAvailable = await StreamCompanionApi.IsAvailableAsync();
                if (!performanceCalculatorAvailable)
                {
                    _view.AdditionalStatsRtf = getRtfString("Couldn't reach StreamCompanion, is it running?");
                    return;
                }
            }

            _view.AdditionalStatsRtf = getRtfString("Loading...");
            var osuFileLocation = beatmap.LocalBeatmapMissing && beatmap.MapId > 0
                ? fileCache.DownloadAndAdd($"https://osu.ppy.sh/osu/{beatmap.MapId}")
                : beatmap.FullOsuFileLocation();

            var mods = _model.SelectedMods.ToString().Replace(",", "").Replace(" ", "").ToUpperInvariant();
            var stats = await StreamCompanionApi.GetMapStatsAsync(osuFileLocation, mods);
            if (stats == null)
            {
                _view.AdditionalStatsRtf = getRtfString("Error while getting performance stats from StreamCompanion.");
                performanceCalculatorAvailable = false;
                return;
            }

            var str = $@"{(mods.Length >= 4 ? "            " : (mods.Length == 2 ? "       " : string.Empty))}\b SS:\b0  {stats.osu_SSPP:0.00} \b 99:\b0  {stats.osu_99PP:0.00} \b 98:\b0  {stats.osu_98PP:0.00} \b 96:\b0  {stats.osu_96PP:0.00} \par ";
            if (mods.Length == 0 || _model.SelectedMods == Mods.Nm)
                str += "select additional mods by searching for \"mods=HR\"(or DT,FL,EZ) ";
            else
                str += $@"{mods}: \b SS:\b0  {stats.osu_mSSPP:0.00} \b 99:\b0  {stats.osu_m99PP:0.00} \b 98:\b0  {stats.osu_m98PP:0.00} \b 96:\b0  {stats.osu_m96PP:0.00} ";

            _view.AdditionalStatsRtf = getRtfString(str);//.Replace(' ', ' ');
        }

        private string getRtfString(string text)
        {
            return @"{\rtf1\ansi " + text + "}";
        }

        private void SetViewData(Image image, Beatmap beatmap, string url = null)
        {
            _view.beatmapImage = image;
            _view.beatmapImageUrl = url;
            if (beatmap == null)
            {
                _view.BasicStats = "";
                _view.BeatmapName = "";
            }
            else
            {
                _view.BasicStats = $"AR: {Math.Round(beatmap.ApproachRate, 2).ToString(CultureInfo.InvariantCulture)} CS: {Math.Round(beatmap.CircleSize, 2).ToString(CultureInfo.InvariantCulture)} " +
                    $"{Environment.NewLine}OD: {Math.Round(beatmap.OverallDifficulty, 2).ToString(CultureInfo.InvariantCulture)} Stars: {Math.Round(beatmap.StarsNomod, 2).ToString(CultureInfo.InvariantCulture)}";
                _view.BeatmapName = ((BeatmapExtension)beatmap).ToString(true);
            }
        }
    }
}