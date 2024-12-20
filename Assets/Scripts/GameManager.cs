using System;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
   // Implement singleton pattern

    public static GameManager Instance {  get; private set; }

    public static event Action<GameState> OnGameStateUpdate;

    public GameSettings Settings { get; set; } = new GameSettings();
    public List<TriviaCategory> AvailableCategories { get; private set; }

    public GameState State { get; private set; } = GameState.MainMenu;

    public int Score { get; private set; } = 0;

    public bool IsPlaying { get; private set; } = false;


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

        // Get the list of available trivia categories from the API
        PopulateCategories();
        

    }


    public void UpdateGameState(GameState newState)
    {
        State = newState;

        switch (newState)
        {
            case GameState.MainMenu:
                IsPlaying = false;
                break;
            case GameState.Playing:
                Time.timeScale = 1f;
                if (!IsPlaying)
                {
                    IsPlaying = true;
                    QuestionManager.Instance.StartQuestions();
                }
                break; 
            case GameState.GameOver:
                IsPlaying = false;
                break;
            case GameState.Paused:
                Time.timeScale = 0f;
                break;
        }

        OnGameStateUpdate?.Invoke(State);
    }

    private void PopulateCategories()
    {
        AvailableCategories = OpenTdbAPIHelper.GetCategories();
        //Insert the "any" category at the beginning
        AvailableCategories.Insert(0, new TriviaCategory());
    }

    public void UpdateSettings(GameSettings newSettings)
    {
        Settings = newSettings;
    }
}



public enum GameState
{
    MainMenu,
    Playing,
    Paused,
    GameOver,
    Error
}

#region Settings Classes
public class GameSettings
{
    public int NumQuestions = 10;
    public TriviaCategory Category = new TriviaCategory();
    public TriviaDifficulty Difficulty = TriviaDifficulty.Any;
    public TriviaQuestionType QuestionType = TriviaQuestionType.Mixed;
    public float TimePerQuestion = 30f;
}


public class TriviaCategory
{
    public string Name;
    public int Id;

    public TriviaCategory()
    {
        Name = "Any";
        Id = -1;
    }
}

public enum TriviaDifficulty
{
    Any,
    Easy,
    Medium,
    Hard
}

public enum TriviaQuestionType
{
    Mixed,
    MultipleChoice,
    TrueFalse,
    WriteIn
}
#endregion
