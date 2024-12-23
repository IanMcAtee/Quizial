/// <summary>
/// Base class for a trivia question
/// </summary>
public class TriviaQuestion
{
    public int Number;
    public string Difficulty { get;  set; }
    public string Category { get;  set; }
    public string Question { get;  set; }
    public string CorrectAnswer { get;  set; }
    public string[] IncorrectAnswers { get;  set; }
}