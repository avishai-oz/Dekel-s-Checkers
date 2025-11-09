using System;
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

        bool _isAnimating = false;
        BoardState _pendingBoard;
        PlayerColor _pendingSide;
        
        enum UiState { Idle, FromSelected }
        UiState _state = UiState.Idle;
        Coord _from;
        Dictionary<Coord, List<Move>> _byFrom = new();
        
        public GamePresenter(MatchService match, IBoardView board, IInGameHUD hud)
        {
            Debug.Log("GamePresenter: constructed & wiring events");

            _match = match ?? throw new ArgumentNullException(nameof(match));
            _board = board ?? throw new ArgumentNullException(nameof(board));
            _hud   = hud ?? throw new ArgumentNullException(nameof(board));
            
            _match.PositionChanged += HandelPositionChanged; //match event → view update
            _match.MoveCommitted += HandleMoveCommitted;     // match event → view update
            _match.GameOver += HandelGameOver;               // match event → view update
            _board.TileClicked += OnTileClicked;             // board event → match update

        }

       
        public void OnTileClicked(Coord c)
        {
            if (_state == UiState.Idle)
            {
                if (_byFrom.TryGetValue(c, out var movesFromHere))
                {
                    _from = c;
                    _state = UiState.FromSelected;
                    _board.ClearHighlights();
                    _board.HighlightTargets(movesFromHere.Select(m => m.To));
                }
            }
        
            if (_state == UiState.FromSelected)
            {
                var moves = _byFrom[_from];
                var chosenMove = moves.FirstOrDefault(m => m.To.Equals(c));
                if (chosenMove != null)
                {
                    _board.ClearHighlights();
                    _state = UiState.Idle;
                    _match.Apply(chosenMove);
                    return;
                }
                if (_byFrom.TryGetValue(c, out var movesFromNew))
                {
                    _from = c;
                    _board.HighlightTargets(movesFromNew.Select(m => m.To));
                    return;
                }
                _board.ClearHighlights();
                _state = UiState.Idle;
                return;
            }
        }
        public void OnStartRequested()
        {
            Debug.Log("Presenter.OnStartRequested → draw initial board");
            _board.ShowPosition(_match.Snapshot());  
            _hud.ShowTurn(_match.SideToMove);        
            RebuildLegalCache();                   
        }
        public void OnStartMatch()
        {
            Debug.Log("Presenter.OnStart... → calling ShowPosition");

            _board.ShowPosition(_match.Snapshot());
            _hud.ShowTurn(_match.SideToMove);
        }
        public void OnMoveTweenComplete()
        {
            throw new System.NotImplementedException();
        }
        private void HandelPositionChanged(BoardState board, PlayerColor side)
        {
            if (_isAnimating)
            {
                _pendingBoard = board;
                _pendingSide = side;
                return;
            }

            _board.ShowPosition(board);
            _hud.ShowTurn(side);
            RebuildLegalCache();
        }
        private void HandleMoveCommitted(PlayerColor mover, Move m)
        {
            _isAnimating = true;
            _board.ClearHighlights();

            _board.AnimateMove(m, () =>
            {
                _isAnimating = false;

                if (_pendingBoard != null)
                {
                    _board.ShowPosition(_pendingBoard);
                    _hud.ShowTurn(_pendingSide);
                    _pendingBoard = null;
                    RebuildLegalCache();
                }
            });        }

        private void HandelGameOver(PlayerColor? winner)
        {
            Debug.Log("Presenter: handling GameOver");
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