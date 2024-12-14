using System;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
   // Implement singleton pattern

    public static GameManager Instance {  get; private set; }

    public static event Action<GameState> OnGameStateUpdate;

    public GameState State { get; private set; }


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    public void UpdateGameState(GameState newState)
    {
        State = newState;

        switch (newState)
        {
            case GameState.MainMenu:
                break;
            case GameState.Playing:
                break; 
            case GameState.GameOver:
                break;
        }

        OnGameStateUpdate?.Invoke(State);
    }
}

public enum GameState
{
    MainMenu,
    Playing,
    GameOver
}
