using Cysharp.Threading.Tasks;
using DefaultNamespace;
using UnityEngine;

public interface IPieceSelector
{
    UniTask SelectPiece(Piece piece);
}