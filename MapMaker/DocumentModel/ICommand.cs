
namespace MapMaker.DocumentModel
{
    public interface ICommand
    {
        void Execute();

        void Undo();
    }
}