using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject.Asteroids;

public interface IGameStateService
{
    void StartGame();

    void EndGame(bool isWin);

    IObservable<GameState> OnStateChanged { get; }

    GameState CurrentState { get; }
}

public enum GameState
{
    None,    
    Playing, 
    Win,      
    Lose     
}
