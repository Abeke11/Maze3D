
using System;
using UniRx;

public class GameStateService : IGameStateService
{
    readonly ReactiveProperty<GameState> _state = new ReactiveProperty<GameState>(GameState.None);

    public IObservable<GameState> OnStateChanged => _state;

    public GameState CurrentState => _state.Value;

    public void StartGame()
    {
        _state.Value = GameState.Playing;
    }

    public void EndGame(bool isWin)
    {
        _state.Value = isWin ? GameState.Win : GameState.Lose;
    }
}
