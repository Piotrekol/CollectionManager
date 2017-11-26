using System;
using System.Collections.Generic;
using CollectionManagerExtensionsDll.DataTypes;
using CollectionManagerExtensionsDll.Enums;

namespace CollectionManagerExtensionsDll.Modules.Msn
{
    public class MsnManager : IMsnGetter,IDisposable
    {
        private readonly MsnListener _listener;
        private readonly object _lockingObject = new object();
        private string _lastMsnString = "";
        public EventHandler<MsnResult> NewMsnMessage;
        public MsnManager()
        {
            _listener = new MsnListener(new List<IMsnGetter> { this });
        }

        public void HandleMsn(MsnResult result)
        {
            lock (_lockingObject)
            {
                var duplicated = IsFalsePlay(result.Raw(), result.OsuState, _lastMsnString);
                result.OsuState = duplicated ? OsuState.FalsePlaying : result.OsuState;
                _lastMsnString = result.Raw();

                NewMsnMessage?.Invoke(this,result);
            }
        }
        #region MSN double-send fix
        private readonly string[] _lastListened = new string[2];
        bool IsFalsePlay(string msnString, OsuState osuStatus, string lastMapString)
        {
            lock (_lastListened)
            {
                // if we're listening to a song AND it's not already in the first place of our Queue
                if (osuStatus == OsuState.Listening && msnString != _lastListened[0])
                {
                    //first process our last listened song "Queue" 
                    _lastListened[1] = _lastListened[0];
                    _lastListened[0] = msnString;
                }
                //we have to be playing for bug to occour...
                if (osuStatus != OsuState.Playing)
                    return false;
                //if same string is sent 2 times in a row
                if (msnString == lastMapString)
                {
                    //this is where it gets checked for actual bug- Map gets duplicated only when we just switched from another song
                    //so check if we switched by checking if last listened song has changed 
                    if (_lastListened[0] != _lastListened[1])
                    {
                        //to avoid marking another plays(Retrys) as False- we "break" our Queue until we change song.
                        _lastListened[1] = _lastListened[0];
                        return true;
                    }
                }
                return false;
            }
        }

        #endregion //MSN FIX

        public void Dispose()
        {
            _listener?.Dispose();
        }
    }
}