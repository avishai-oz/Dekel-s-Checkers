// Update PlayerWonHeader.cs

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Resources.UI.Scripts
{
    public class PlayerWonHeader : MonoBehaviour
    {
        [SerializeField] private TMP_Text winnerText;

        private void Start()
        {
            string winningPieceType = PlayerPrefs.GetString("WinningPieceType", "None");
            winnerText.text = $"{winningPieceType} won!";
        }
    }
}