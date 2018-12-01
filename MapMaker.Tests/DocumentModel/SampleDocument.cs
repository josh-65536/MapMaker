using System.Collections.Generic;
using MapMaker.DocumentModel;

namespace MapMaker.Tests.DocumentModel
{
    class SampleDocument : IDocument<SampleMedium>
    {
        public SampleMedium Medium { get; }

        private List<ICommand> _history;
        private int _lastCommandIndex;

        public SampleDocument()
        {
            Medium = new SampleMedium(this);
            _history = new List<ICommand>();
            _lastCommandIndex = -1;
        }

        public void ExecuteCommand(ICommand command)
        {
            if (_lastCommandIndex != -1)
            {
                while (_history[_history.Count - 1] != _history[_lastCommandIndex])
                    _history.RemoveAt(_history.Count - 1);
            }

            command.Execute();
            _history.Add(command);
            _lastCommandIndex = _history.Count - 1;
        }

        public void Redo()
        {
            if (_lastCommandIndex != _history.Count - 1)
                _history[++_lastCommandIndex].Execute();
        }

        public void Undo()
        {
            if (_lastCommandIndex != -1)
                _history[_lastCommandIndex--].Undo();
        }
    }
}