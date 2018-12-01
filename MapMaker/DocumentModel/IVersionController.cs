
namespace MapMaker.DocumentModel
{
    public interface IVersionController
    {
        void ExecuteCommand(ICommand command);

        void Undo();

        void Redo();
    }
}