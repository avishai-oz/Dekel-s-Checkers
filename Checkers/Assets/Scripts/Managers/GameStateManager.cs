using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class GameStateManager : MonoBehaviour
    {
        public void CheckGameOver(int blackPiecesCount, int whitePiecesCount, PieceType currentPieceType)
        {
            if (blackPiecesCount == 0 || whitePiecesCount == 0)
            {
                if (blackPiecesCount == 0)
                {
                    PlayerPrefs.SetString("WinningPieceType", PieceType.White.ToString());
                }
                else
                {
                    PlayerPrefs.SetString("WinningPieceType", PieceType.Black.ToString());
                }
                SceneManager.LoadScene("Credits");
            }
        }
    }
}