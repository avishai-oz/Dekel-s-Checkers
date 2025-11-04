using UnityEngine;
using Checkers.Domain;
using Checkers.Application;
using Checkers.Presenter;
using Checkers.View;

public class GameComposerMB : MonoBehaviour
{
    [SerializeField] private BoardView boardView;   // גרור את ה-BoardView MB
    [SerializeField] private IInGameHUD hudView;     // אם עוד אין — צור דמה שמיישם IInGameHUD

    private MatchService _match;
    private GamePresenter _presenter;

    void Awake()
    {
        // 1) Domain
        var initial = BoardState.CreateInitial(); // או פונקציית אתחול שלך
        var rules   = new AmericanRules();

        // 2) Application
        _match = new MatchService(rules, initial, PlayerColor.White);

        // 3) Presenter
        _presenter = new GamePresenter(hudView, boardView, _match);

        // 4) ציור פתיחה
        _presenter.OnStartMatch();
    }
}