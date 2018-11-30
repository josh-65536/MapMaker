using System;

namespace MapMaker.DocumentModel
{
    public class LevelEventArgs : EventArgs
    {
        public Level Level { get; }

        public LevelEventArgs(Level level)
        {
            Level = level;
        }
    }
}