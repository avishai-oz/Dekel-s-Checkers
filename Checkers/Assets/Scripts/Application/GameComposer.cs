using UnityEngine;
using Checkers.Domain;
using Checkers.Application;
using Checkers.Presenter;
using Checkers.View;

public class GameComposer : MonoBehaviour
{
    [SerializeField] private BoardView boardView;
    [SerializeField] private InGameHUD hudView;     

    private MatchService match;
    private GamePresenter _presenter;

    void Awake()
    {
        // 1) Domain
        var initial = BoardState.CreateInitial(); 
        var rules   = new AmericanRules();

        // 2) Application
        match = new MatchService(rules, initial, PlayerColor.White);

        // 3) Presenter
        _presenter = new GamePresenter(match, boardView, hudView);
        Debug.Log("Composer: calling OnStartRequested");
        _presenter.OnStartRequested();
        Debug.Log("Composer: returned from OnStartRequested");
        
    }
}