using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Main game manager class (singleton)
/// </summary>
[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance {  get; private set; }
    public static event Action<GameState> OnGameStateUpdate;
    public GameSettings Settings { get; set; } = new GameSettings();
    public List<TriviaCategory> AvailableCategories { get; private set; }
    public GameState State { get; private set; } = GameState.MainMenu;
    public int Score { get; private set; } = 0;
    public bool IsPlaying { get; private set; } = false;

    [Header("Menu Properties")]
    [SerializeField]
    private MenuElement[] _menuElements;


    private void Awake()
    {
        // Implement singleton pattern 
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
        UpdateGameState(GameState.MainMenu);
    }


    public void UpdateGameState(GameState newState)
    {
        State = newState;
        switch (newState)
        {
            case GameState.MainMenu:
                HandleMainMenuState();
                break;
            case GameState.Playing:
                HandlePlayingState();
                break; 
            case GameState.GameOver:
                HandleGameOverState();
                break;
            case GameState.Paused:
                HandlePausedState();
                break;
            case GameState.Error:
                HandleErrorState();
                break;
        }
        SetSingleMenuActive();
        OnGameStateUpdate?.Invoke(State);
    }

    private void HandleMainMenuState()
    {
        Time.timeScale = 1f;
        IsPlaying = false;
    }

    private void HandlePlayingState()
    {
        Time.timeScale = 1f;
        if (!IsPlaying)
        {
            IsPlaying = true;
            QuestionManager.Instance.StartQuestions();
        } 
    }

    private void HandlePausedState()
    {
        Time.timeScale = 0f;
    }

    private void HandleGameOverState()
    {
        IsPlaying = false;
        Time.timeScale = 1f;
    }

    private void HandleErrorState()
    {
        IsPlaying = false;
    }

    private void SetSingleMenuActive()
    {
        foreach (MenuElement menuElement in _menuElements)
        {
            if (menuElement.AssociatedGameState == State)
            {
                menuElement.gameObject.SetActive(true);
                continue;
            }
            menuElement.gameObject.SetActive(false);
        }
    }

    public void QuitApplication()
    {
        Application.Quit(); 
    }

    private void PopulateCategories()
    {
        AvailableCategories = OpenTdbAPIHelper.GetCategories();
        //Insert the "any" category at the beginning
        AvailableCategories.Insert(0, new TriviaCategory());
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
