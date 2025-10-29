
using Checkers.Domain;

namespace Checkers.View
{
    public interface IInGameHUD
    {
        public void ShowTurn(PlayerColor side);
    }
}