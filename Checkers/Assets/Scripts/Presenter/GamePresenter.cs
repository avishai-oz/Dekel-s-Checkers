using Checkers.Application;
using Checkers.Domain;
using Checkers.View;

namespace Checkers.Presenter
{
    public class GamePresenter : IGamePresenter
    {
        private readonly MatchService _match;
        private readonly IBoardView _board;
        private readonly IInGameHUD _hud;

        
        public GamePresenter(IInGameHUD hud, IBoardView board, MatchService match)
        {
            _match = match;
            _board = board;
            _hud = hud;
            
            _match.PositionChanged += HandelPositionChanged;
            _match.GameOver += HandelGameOver;
            
        }
        
        public void OnTileClicked(Coord c)
        {
            throw new System.NotImplementedException();
        }

        public void OnMoveTweenComplete()
        {
            throw new System.NotImplementedException();
        }

        public void OnStartMatch()
        {
            throw new System.NotImplementedException();
        }
        
        private void HandelPositionChanged(BoardState board, PlayerColor side)
        {
            _board.ShowPosition(board);
            _hud.ShowTurn(side);
        }

        private void HandelGameOver(PlayerColor? winner)
        {
            
        }
    }
}