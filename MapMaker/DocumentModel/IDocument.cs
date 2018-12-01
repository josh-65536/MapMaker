
namespace MapMaker.DocumentModel
{
    public interface IDocument<TMedium> : IVersionController
    {
        TMedium Medium { get; }
    }
}