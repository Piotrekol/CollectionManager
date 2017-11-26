using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using CollectionManager;
using CollectionManager.DataTypes;
using CollectionManager.Interfaces;
using CollectionManager.Modules.FileIO;
using CollectionManagerExtensionsDll.DataTypes;
using CollectionManagerExtensionsDll.Enums;

namespace CollectionManagerExtensionsDll.Modules
{
    public class PlayHistoryManager : IDisposable
    {
        private readonly OsuFileIo _osuFileIo;
        private readonly StatusListener _statusListener;
        public EventHandler<PlayHistoryEntry> NewHistoryEntry;
        public readonly ObservableCollection<PlayHistoryEntry> HistoryEntries = new ObservableCollection<PlayHistoryEntry>();

        public readonly OsuState ValidStates = OsuState.Listening | OsuState.Playing | OsuState.Watching;
        private readonly OsuState PassiveStates = OsuState.Listening | OsuState.Watching | OsuState.Playing;
        private string _lastListened = "";
        private OsuState[] _lastStates = new OsuState[2];
        private OsuResult _lastOsuResult;
        private bool _failInNextTick;
        public PlayHistoryManager(OsuFileIo osuFileIo, StatusListener statusListener = null)
        {
            if (statusListener == null)
                throw new ArgumentNullException(nameof(statusListener));
            _osuFileIo = osuFileIo;
            _statusListener = statusListener;
            _statusListener.NewOsuResult += NewOsuResult;
            HistoryEntries.CollectionChanged += (s, a) =>
                NewHistoryEntry?.Invoke(this, HistoryEntries[HistoryEntries.Count - 1]);
        }

        private void NewOsuResult(object sender, OsuResult osuResult)
        {
            if ((ValidStates & osuResult.Msn.OsuState) > 0)
            {
                if (_lastStates[1] == OsuState.Playing && _lastStates[0] == OsuState.Listening
                    && _lastListened == osuResult.Msn.Raw(false))
                {
                    ProcessFinishedPlay(osuResult);
                    _failInNextTick = false;
                }
                else if ((osuResult.Msn.OsuState & PassiveStates) > 0)
                {
                    HistoryEntries.Add(new PlayHistoryEntry
                    {
                        Date = DateTime.Now,
                        Score = null,
                        State = osuResult.Msn.OsuState,
                        Beatmap = osuResult.Beatmap
                    });
                }
                if (osuResult.Msn.OsuState == OsuState.Listening)
                    _lastListened = osuResult.Msn.Raw();

                _lastStates[1] = _lastStates[0];
                _lastStates[0] = osuResult.Msn.OsuState;
                if (_failInNextTick || (_lastStates[1] == OsuState.Playing && _lastStates[0] == OsuState.Listening))
                {//Failed/aborted
                    if (_failInNextTick)
                    {
                        ProcessFinishedPlay(_lastOsuResult, false);
                        _failInNextTick = false;
                    }
                    else
                        _failInNextTick = true;
                }
                _lastOsuResult = osuResult;
            }
        }

        private void ProcessFinishedPlay(OsuResult osuResult, bool passed = true)
        {
            OsuState state = OsuState.Passed;
            var beatmap = osuResult.Beatmap;
            var score = GetRecentScore();
            var map = score.GetMap(_osuFileIo.LoadedMaps);
            if (passed && map != null && map.ArtistRoman == osuResult.Msn.Artist && map.Title == osuResult.Msn.Title)
            {//Finished play with valid replay
                beatmap = map;
            }
            else
            {//Failed/aborted play - discard score
                score = null;
                state = OsuState.Failed;
            }
            HistoryEntries.Add(new PlayHistoryEntry
            {
                Date = DateTime.Now,
                Score = score,
                State = state,
                Beatmap = beatmap
            });
        }
        private Score GetRecentScore()
        {
            Score score = null;
            var osuDirectory = _osuFileIo.OsuPathResolver.ResolvedDirectory;
            var replayDirectory = Path.Combine(osuDirectory, "Data", "r");
            DirectoryInfo info = new DirectoryInfo(replayDirectory);
            var files = info.GetFiles().Where(p => p.Extension == ".osr").OrderByDescending(p => p.CreationTime).ToList();

            if (files.Count > 0)
            {
                var file = files[0];
                if (file != null)
                {
                    using (var fs = file.OpenRead())
                        score = (Score)_osuFileIo.ScoresLoader.ReadReplay(fs);
                }
            }
            return score;
        }

        public void Dispose()
        {
            _statusListener?.Dispose();
        }

        public class PlayHistoryEntry : EventArgs
        {
            public DateTime Date { get; set; }
            public IReplay Score { get; set; }
            public OsuState State { get; set; }
            public Beatmap Beatmap { get; set; }
        }

    }
}
