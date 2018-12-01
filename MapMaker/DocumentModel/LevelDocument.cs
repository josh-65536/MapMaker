using System;

namespace MapMaker.DocumentModel
{
    public class LevelDocument : IDocument<Level>
    {
        public Level Medium { get; }

        public event EventHandler<LevelEventArgs> ChangeMap;

        public LevelDocument(Level level)
        {
            Medium = level;

            ChangeMap += (o, e) => { };
        }

        public void NotifyMapChange() => ChangeMap(this, new LevelEventArgs(Medium));

        public void ExecuteCommand(ICommand command)
        {
            throw new NotImplementedException();
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }

        public void Redo()
        {
            throw new NotImplementedException();
        }
    }
}