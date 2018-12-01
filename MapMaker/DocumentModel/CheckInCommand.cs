
namespace MapMaker.DocumentModel
{
    public class CheckInCommand<T> : ICommand
    {
        private readonly Revision<T> _property;
        private readonly T _oldValue;
        private readonly T _newValue;

        public CheckInCommand(Revision<T> property, T oldValue, T newValue)
        {
            _property = property;
            _oldValue = oldValue;
            _newValue = newValue;
        }

        public void Execute()
        {
            _property._currentValue = _newValue;
        }

        public void Undo()
        {
            _property._currentValue = _oldValue;
        }
    }
}