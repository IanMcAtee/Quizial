using UnityEngine;
using System.Collections.Generic;

[DefaultExecutionOrder(-1)]
public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    public GameSettings Settings {  get; set; } = new GameSettings();
    public List<TriviaCategory> AvailableCategories { get; private set; }

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

        PopulateCategories();
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

public class GameSettings
{
    public int NumQuestions = 10;
    public TriviaCategory Category = new TriviaCategory();
    public TriviaDifficulty Difficulty = TriviaDifficulty.Any;
    public TriviaQuestionType QuestionType = TriviaQuestionType.Mixed;
    public float SFXVolume = 1.0f;
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