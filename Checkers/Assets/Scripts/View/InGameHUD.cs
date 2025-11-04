using TMPro;
using UnityEngine;
using Checkers.Domain;

namespace Checkers.View
{
    public class InGameHUD : MonoBehaviour, IInGameHUD
    {
        [SerializeField] private TMP_Text turnLabel;

        public void ShowTurn(PlayerColor side)
        {
            if (turnLabel != null)
                turnLabel.text = side == PlayerColor.White ? "White to move" : "Black to move";
            else
                Debug.Log($"Turn: {side}");
        }
    }
}