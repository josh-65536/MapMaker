
namespace MapMaker.DocumentModel
{
    public class Revision<T> where T : struct
    {
        private T _currentValue;
        private readonly IVersionController _versionController;

        public Revision(IVersionController versionController)
        {
            _versionController = versionController;
        }

        public View AsView() => new View(this);

        public ref struct View
        {
            private readonly Revision<T> _target;

            public View(Revision<T> target) => _target = target;

            public static implicit operator T(View view) => view._target._currentValue;

            public void CheckIn(T newValue)
            {
                _target._versionController.ExecuteCommand(
                    new CheckInCommand(_target, _target._currentValue, newValue)
                );
            }
        }

        private sealed class CheckInCommand : ICommand
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

            public void Execute() => _property._currentValue = _newValue;

            public void Undo() => _property._currentValue = _oldValue;
        }
    }
}