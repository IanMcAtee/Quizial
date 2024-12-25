using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Main game manager class (singleton) <br/>
/// Handles: <br/>
/// - Game State and State Updates <br/>
/// - Game Settings <br/>
/// - Activating menu elements based on game state
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

        // Set the resolution of the game
        Screen.SetResolution(720, 1280, false);

        // Get the list of available trivia categories from the API
        PopulateCategories();
        UpdateGameState(GameState.MainMenu);
    }

    


    /// <summary>
    /// Update the state of the game, handles logic for each state
    /// </summary>
    /// <param name="newState"></param>
    public void UpdateGameState(GameState newState)
    {
        State = newState;
        switch (newState)
        {
            case GameState.MainMenu:
                HandleMainMenuState();
                break;
            case GameState.Playing:
                /* DIFFERENCE:
                 * Need to activate the playing UI prior to calling its methods
                 * This ensures its scripts are enabled to perform game logic */
                SetSingleMenuActive();
                OnGameStateUpdate?.Invoke(State);
                HandlePlayingState();
                return; 
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

    /// <summary>
    /// Handles logic of main menu state
    /// </summary>
    private void HandleMainMenuState()
    {
        Time.timeScale = 1f;
        IsPlaying = false;
        Score = 0;
    }

    /// <summary>
    /// Handles logic of playing state
    /// </summary>
    private void HandlePlayingState()
    {
        Time.timeScale = 1f;
        // Only start questions if player coming from main menu or game over state
        if (!IsPlaying)
        {
            Score = 0;
            IsPlaying = true;
            QuestionManager.Instance.StartQuestions();
        } 
    }

    /// <summary>
    /// Handles logic of paused state
    /// </summary>
    private void HandlePausedState()
    {
        Time.timeScale = 0f;
    }

    /// <summary>
    /// Handles logic of game over state
    /// </summary>
    private void HandleGameOverState()
    {
        IsPlaying = false;
        Time.timeScale = 1f;
    }

    /// <summary>
    /// Handles logic of error state
    /// </summary>
    private void HandleErrorState()
    {
        IsPlaying = false;
    }

    /// <summary>
    /// Sets a single menu element active based on the current game state
    /// </summary>
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

    /// <summary>
    /// Updates the game score
    /// </summary>
    /// <param name="points"></param>
    public void UpdateScore(int points)
    {
        Score += points;
    }

    /// <summary>
    /// Quits the application
    /// </summary>
    public void QuitApplication()
    {
        Application.Quit(); 
    }

    /// <summary>
    /// Gets a list of all available categories from open trivia DB
    /// </summary>
    private void PopulateCategories()
    {
        AvailableCategories = OpenTdbAPIHelper.GetCategories();
        //Insert the "any" category at the beginning
        AvailableCategories.Insert(0, new TriviaCategory());
    }
}

/// <summary>
/// Game States Enum
/// </summary>
public enum GameState
{
    MainMenu,
    Playing,
    Paused,
    GameOver,
    Error
}

#region Settings Classes

/// <summary>
/// Game Settings Class
/// </summary>
public class GameSettings
{
    public int NumQuestions = 10;
    public TriviaCategory Category = new TriviaCategory();
    public TriviaDifficulty Difficulty = TriviaDifficulty.Any;
    public TriviaQuestionType QuestionType = TriviaQuestionType.Mixed;
    public float TimePerQuestion = 30f;
}

/// <summary>
/// Trivia Category Class
/// </summary>
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

/// <summary>
/// Trivia Difficulty Enum
/// </summary>
public enum TriviaDifficulty
{
    Any,
    Easy,
    Medium,
    Hard
}

/// <summary>
/// Triva Question Type Enum
/// </summary>
public enum TriviaQuestionType
{
    Mixed,
    MultipleChoice,
    TrueFalse,
    WriteIn
}
#endregion
