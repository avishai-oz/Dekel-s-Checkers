using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Checkers.Domain
{
    public sealed class AmericanRules : IRules
    { 
        public IReadOnlyList<Move> LegalMoves(BoardState board, PlayerColor side)
       {
           if (board == null)
               throw new ArgumentNullException(nameof(board));
           
           var captures = new List<Move>();
           var steps    = new List<Move>();
           
           for(int r=0; r<BoardState.Size; r++)
           for (int c = 0; c < BoardState.Size; c++)
           {
               var p = board[r, c];
               if (p == null || p.Owner != side) continue;
               
               CollectCaptures(board, new Coord(r,c), side, captures);
               CollectSteps(board, new Coord(r, c), side, steps);
               
           }
           return captures.Count > 0 ? captures : steps;
       }
        public bool IsGameOver(BoardState board, PlayerColor side, out PlayerColor? winner)
         {
              if (board == null)
                throw new ArgumentNullException(nameof(board));
              
              winner = null;
              return false;
         }
         
         
         // ---------- Helpers ----------
         
        // צעדים רגילים (לא אכילות)
         private void CollectSteps(BoardState board, Coord from, PlayerColor side, List<Move> results)
         {
             var piece = board[from];
             if (piece == null) return;
             
             int[] rowDirs = piece.Kind == PieceKind.Queen ? new[] { -1, +1 } 
                 : (side == PlayerColor.White ? new[] { -1 } : new[] { +1 });

             int[] colDirs = new[] { -1, +1 };

             foreach (var dr in rowDirs)
             foreach (var dc in colDirs)
             {
                 var to = new Coord(from.Row + dr, from.Col + dc);
                 if (!board.InBounds(to)) continue;
                 if (!to.IsDarkSquare) continue;              
                 if (board[to] != null) continue;              

                 results.Add(Move.Simple(from, to));
             }
         }
        
         // אכילה בודדת (ללא שרשור)
         /*private void CollectSingleCaptures(BoardState board, Coord from, PlayerColor side, List<Move> results)
         {
             var piece = board[from];
             if (piece == null) return;
             
             int[] rowDirs = piece.Kind == PieceKind.Queen ? new[] { -1, +1 } 
                 : (side == PlayerColor.White ? new[] { -1 } : new[] { -1,+1 });
             int[] colDirs = new[] { -1, +1 };
             foreach (var dr in rowDirs)
             foreach (var dc in colDirs)
             {
                 var over = new Coord(from.Row + dr, from.Col + dc);
                 var to   = new Coord(from.Row + 2 * dr, from.Col + 2 * dc);
                    
                 if (!board.InBounds(over) || !board.InBounds(to)) continue;
                 if (!to.IsDarkSquare) continue;              
                    
                 var jumpedPiece = board[over];
                 if (jumpedPiece == null || jumpedPiece.Owner == side) continue; 
                 if (board[to] != null) continue;
                 
                 results.Add(Move.Capture(from, to, over));
             }
         }*/
         

         // אכילות (כולל שרשור)
         private void CollectCaptures(BoardState board, Coord from, PlayerColor side, List<Move> results)
         {
             var piece = board[from];
             if (piece == null) return;

             // אמריקאית: Single אוכל גם קדימה וגם אחורה; Queen כרגיל דו-כיוונית
             int[] rowDirs = piece.Kind == PieceKind.Queen ? new[] { -1, +1 } : new[] { -1, +1 };
             int[] colDirs = new[] { -1, +1 };

             // DFS: מנסים קפיצה, מסירים את הנאכל, וממשיכים משם. אם אין המשך — סוגרים מהלך מורכב.
             void Dfs(BoardState b, Coord current, List<Coord> capturedSoFar)
             {
                 bool extended = false;

                 foreach (var dr in rowDirs)
                 foreach (var dc in colDirs)
                 {
                     var mid = new Coord(current.Row + dr, current.Col + dc);
                     var to  = new Coord(current.Row + 2*dr, current.Col + 2*dc);

                     if (!b.InBounds(mid) || !b.InBounds(to)) continue;
                     if (!to.IsDarkSquare) continue;

                     var middlePiece = b[mid];
                     if (middlePiece == null || middlePiece.Owner == side) continue;
                     if (b[to] != null) continue;

                     // מסמלצים: מעבירים כלי, מסירים את היריב, וממשיכים לחפש עוד אכילות
                     var b2 = b.Clone();
                     var moving = b2[current];
                     b2.MovePiece(current, to);
                     b2.Remove(mid);

                     var capturedNext = new List<Coord>(capturedSoFar) { mid };
                     extended = true;
                     Dfs(b2, to, capturedNext);
                 }

                 // אם לא הצלחנו להאריך (אין עוד אכילות) אבל כבר יש לפחות אכילה אחת → סוגרים מהלך
                 if (!extended && capturedSoFar.Count > 0)
                 {
                     results.Add(new Move(from, current, capturedSoFar.ToArray()));
                 }
             }

             Dfs(board, from, new List<Coord>());
         }


         // האם היעד גורם להכתרה (Single → Queen)
         internal static bool ShouldCrown(Piece piece, Coord to)
         {
             if (piece.Kind == PieceKind.Queen) return false;
             return (piece.Owner == PlayerColor.White && to.Row == 0) ||
                    (piece.Owner == PlayerColor.Black && to.Row == BoardState.Size - 1);
         }
    }
}