using System;

namespace MapMaker.DocumentModel
{
    public class Document
    {
        public Level Level { get; }

        public event EventHandler<LevelEventArgs> ChangeMap;

        public Document(Level level)
        {
            Level = level;

            ChangeMap += (o, e) => { };
        }

        public void NotifyMapChange() => ChangeMap(this, new LevelEventArgs(Level));
    }
}