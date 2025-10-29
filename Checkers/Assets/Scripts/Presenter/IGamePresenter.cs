using Checkers.Domain;

namespace Checkers.Presenter
{
    public interface IGamePresenter
    {
        void OnTileClicked(Coord c);
        void OnMoveTweenComplete();
        void OnStartMatch();
        
    }
}