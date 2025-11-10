using Checkers.Domain;

namespace Checkers.Presenter
{
    public interface IGamePresenter
    {
        void OnTileClicked(Coord c);
        void OnStartMatch();
        
    }
}