using UnityEngine;
using Checkers.Domain;
using Checkers.Application;
using Checkers.Presenter;
using Checkers.View;

public class GameComposer : MonoBehaviour
{
    [SerializeField] private BoardView boardView;   // גרור את ה-BoardView MB
    [SerializeField] private InGameHUD hudView;     

    private MatchService match;
    private GamePresenter _presenter;

    void Awake()
    {
        // 1) Domain
        var initial = BoardState.CreateInitial(); // או פונקציית אתחול שלך
        var rules   = new AmericanRules();

        // 2) Application
        match = new MatchService(rules, initial, PlayerColor.White);

        // 3) Presenter
        _presenter = new GamePresenter(match, boardView, hudView);
        Debug.Log("Composer: calling OnStartRequested");
        _presenter.OnStartRequested();
        Debug.Log("Composer: returned from OnStartRequested");

        // 4) ציור פתיחה
        //_presenter.OnStartMatch();
    }
}