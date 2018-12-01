
namespace MapMaker.DocumentModel
{
    public class Revision<T>
    {
        internal T _currentValue;
        private readonly IVersionController _versionController;

        public Revision(IVersionController versionController)
        {
            _versionController = versionController;
        }

        public T Value
        {
            get => _currentValue;
            set =>
                _versionController.ExecuteCommand(
                    new CheckInCommand<T>(this, _currentValue, value)
                );
        }
    }
}