using System.Collections.Generic;
using System;
using Checkers.Domain;

namespace Checkers.Application
{
    public sealed class MatchService
    {
        private BoardState _board;
        private PlayerColor _turn;
        private readonly Stack<Move> _history;
        private readonly IRules _rules;
        
        public event Action<BoardState, PlayerColor> PositionChanged;
        public event Action<PlayerColor, Move> MoveCommitted;
        public event Action<PlayerColor?> GameOver;
        
        //for presentor
        public IReadOnlyList<Move> LegalMoves() => _rules.LegalMoves(_board, _turn);
        public BoardState Snapshot() => _board.Clone();
        public PlayerColor SideToMove => _turn;
        
        private void OnPositionChanged()
            => PositionChanged?.Invoke(_board.Clone(), _turn);
        
        public MatchService(IRules rules, BoardState initial, PlayerColor sideToMove)
        {
            _board = initial;
            _turn = sideToMove;
            _history = new();
            _rules = rules;
        }

        private static bool SameMove(Move a, Move b)
        {
            if (a.From.Equals(b.From) && a.To.Equals(b.To) && a.Captured.Count == b.Captured.Count)
            {
                for(int i = 0; i < a.Captured.Count; i++)
                    if (!a.Captured[i].Equals(b.Captured[i]))
                        return false;
                return true;
            }
            else
                return false;
        }
        
        public void Apply(Move m)
        {
            EnsureLegal(m);
            RemoveCaptured(m);
            _board.MovePiece(m.From, m.To);
            CheckCrownNeeded(m);
            CommitAndNotify(m);
        }
        private void EnsureLegal(Move move)
        {
            if (move == null)
                throw new System.ArgumentNullException(nameof(move));
            
            var legal = _rules.LegalMoves(_board, _turn);
            bool ok = false;
            foreach (var lm in legal)
                if (SameMove(lm, move))
                {
                    ok = true;
                    break;
                }
            if (!ok)
                throw new InvalidOperationException("Illegal move");
        }
        private void RemoveCaptured(Move m)
        {
            foreach (var c in m.Captured)
            {
                _board.Remove(c);
            }
        }
        private void CheckCrownNeeded(Move m)
        {
            var piece = _board[m.To];
            if (_rules.ShouldCrown(piece, m.To))
                piece.Crown();
        }
        
        private void CommitAndNotify(Move m)
        {
            _history.Push(m);
            _turn = _turn.Opponent();
            OnPositionChanged();
            
            if(_rules.IsGameOver(_board, _turn, out var winner))
            {
                GameOver?.Invoke(winner);
                return;
            }

        }

    }
}