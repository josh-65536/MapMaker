
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

        public static implicit operator T(Revision<T> revision) => revision._currentValue;

        public void CheckIn(T newValue)
        {
            _versionController.ExecuteCommand(
                new CheckInCommand<T>(this, _currentValue, newValue)
            );
        }
    }
}