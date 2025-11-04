using System.Collections.Generic;
using System.Linq;
using Checkers.Application;
using Checkers.Domain;
using Checkers.View;
using UnityEngine;

namespace Checkers.Presenter
{
    public class GamePresenter : IGamePresenter
    {
        private readonly MatchService _match;
        private readonly IBoardView _board;
        private readonly IInGameHUD _hud;

        enum UiState { Idle, FromSelected }
        UiState _state = UiState.Idle;
        Coord _from;
        Dictionary<Coord, List<Move>> _byFrom = new();
        
        public GamePresenter(IInGameHUD hud, IBoardView board, MatchService match)
        {
            Debug.Log("GamePresenter: constructed & wiring events");

            _match = match;
            _board = board;
            _hud = hud;
            
            _match.PositionChanged += HandelPositionChanged;
            _match.GameOver += HandelGameOver;

            _board.TileClicked += OnTileClicked;

        }

        public void OnTileClicked(Coord c)
        {
            if (_state != UiState.Idle) return;

            if (_byFrom.TryGetValue(c, out var movesFromHere))
            {
                _from = c;
                _state = UiState.FromSelected;
                _board.ClearHighlights();
                _board.HighlightTargets(movesFromHere.Select(m => m.To));
            }

        }


        public void OnMoveTweenComplete()
        {
            throw new System.NotImplementedException();
        }

        public void OnStartMatch()
        {
            _board.ShowPosition(_match.Snapshot());
            _hud.ShowTurn(_match.SideToMove);
        }
        
        private void HandelPositionChanged(BoardState board, PlayerColor side)
        {
            _board.ShowPosition(board);
            _hud.ShowTurn(side);
            RebuildLegalCache();
        }

        private void HandelGameOver(PlayerColor? winner)
        {
            
        }
        
        void RebuildLegalCache()
        {
            _byFrom.Clear();
            foreach (var m in _match.LegalMoves())
            {
                if (!_byFrom.TryGetValue(m.From, out var list))
                    _byFrom[m.From] = list = new List<Move>();
                list.Add(m);
            }
        }

    }
}