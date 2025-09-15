namespace CollectionManager.App.Shared.Presenters.Controls;

using CollectionManager.App.Shared.Interfaces.Controls;
using CollectionManager.Common.Interfaces.Controls;
using CollectionManager.Core.Types;
using CollectionManager.Extensions.Utils;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Threading;

public class BeatmapThumbnailPresenter : IDisposable
{
    private readonly IBeatmapThumbnailView _view;
    private readonly IBeatmapThumbnailModel _model;
    private readonly object _lockingObject = new();
    private BeatmapExtension _currentBeatmap;
    private readonly AutoResetEvent resetEvent = new(false);

    public BeatmapThumbnailPresenter(IBeatmapThumbnailView view, IBeatmapThumbnailModel model)
    {
        _view = view;

        _model = model;
        _model.BeatmapChanged += _model_BeatmapChanged;

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

    private long changeId;
    private void LoadData(BeatmapExtension beatmap)
    {//TODO: OPTIMIZATION: do not load same image twice(or more) in a row(Cache last loaded image location and compare?)
        string imgPath = beatmap.GetImageLocation();
        BackgroundWorker bw = new();
        long currentId = Interlocked.Increment(ref changeId);
        bw.DoWork += (s, e) =>
        {
            //resetEvent.WaitOne(100);
            if (beatmap.Equals(_currentBeatmap))
            {
                Image img = null;
                string url = null;
                if (imgPath != "")
                {
                    img = Image.FromFile(imgPath);
                }
                else if (beatmap.MapSetId > 0)
                {
                    url = "https://assets.ppy.sh/beatmaps/" + beatmap.MapSetId + "/covers/card.jpg";
                }

                if (currentId != Interlocked.Read(ref changeId))
                {
                    img?.Dispose();
                    return;
                }

                SetViewData(img, beatmap, url);
            }
        };
        bw.RunWorkerAsync();
    }
    private void SetViewData(Image image, Beatmap beatmap, string url = null)
    {
        _view.beatmapImage = image;
        _view.beatmapImageUrl = url;
        if (beatmap == null)
        {
            _view.AR = "";
            _view.CS = "";
            _view.OD = "";
            _view.Stars = "";
            _view.BeatmapName = "";
        }
        else
        {
            _view.AR = Math.Round(beatmap.ApproachRate, 2).ToString(CultureInfo.InvariantCulture);
            _view.CS = Math.Round(beatmap.CircleSize, 2).ToString(CultureInfo.InvariantCulture);
            _view.OD = Math.Round(beatmap.OverallDifficulty, 2).ToString(CultureInfo.InvariantCulture);
            _view.Stars = Math.Round(beatmap.StarsNomod, 2).ToString(CultureInfo.InvariantCulture);
            _view.BeatmapName = ((BeatmapExtension)beatmap).ToString(true);
        }
    }

    public void Dispose()
    {
        resetEvent?.Dispose();
        GC.SuppressFinalize(this);
    }
}