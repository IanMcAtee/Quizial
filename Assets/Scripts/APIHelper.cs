using System;
using System.Net.Http;
using System.Web;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Class to interact with Open Trivia Database Rest API
/// </summary>
public static class OpenTdbAPIHelper
{

    /// <summary>
    /// Deserialized response from an OpenTdb question API request
    /// </summary>
    [Serializable]
    private class OpenTdbAPIResponse
    {
        [Serializable]
        public class Result
        {
            public string type;
            public string difficulty;
            public string category;
            public string question;
            public string correct_answer;
            public string[] incorrect_answers;
        }
        public int response_code;
        public Result[] results;
    }

    /// <summary>
    /// Deserialized response from an OpenTdb all categories API request 
    /// </summary>
    [Serializable]
    private class OpenTdbCategoryResponse
    {
        [Serializable]
        public class Category
        {
            public int id;
            public string name;
        }
        public Category[] trivia_categories;
    }

    // Constants for the "get all categories" URL and words we ignore in the categories
    private const string CATEGORIES_URL = "https://opentdb.com/api_category.php";
    private static readonly string[] CATEGORY_STRING_IGNORES = { "Entertainment: ", "Science: " };

    /// <summary>
    /// Retrieves a set of trivia questions by attempting a HTTP Get request to the specified URL
    /// </summary>
    /// <param name="questionURL">The URL to attempt a HTTP Get request to</param>
    /// <param name="responseCode">Reference variable for returning response code from API call</param>
    /// <returns></returns>
    public static TriviaQuestion[] GetQuestionSet(string questionURL, out ResponseCode responseCode)
    {
        // Retrieve the JSON string from OpenTriviaDB and deserialize into OpenTdbAPIResponse class
        string json = RetrieveJSON(questionURL);
        OpenTdbAPIResponse apiResponse = JsonUtility.FromJson<OpenTdbAPIResponse>(json);

        // Get the response code in reference variable, return an empty array if error occurred
        responseCode = (ResponseCode)apiResponse.response_code;
        if (responseCode != ResponseCode.Success)
        {
            return new TriviaQuestion[0];
        }
       
        // Preallocate an array to hold the question set
        TriviaQuestion[] questionSet = new TriviaQuestion[apiResponse.results.Length];
        
        // Iterate through each API result, parsing each field into a TriviaQuestion class, and adding to output array
        TriviaQuestion question;
        OpenTdbAPIResponse.Result result;
        for (int i = 0; i < apiResponse.results.Length; i++)
        {
            question = new TriviaQuestion();
            result = apiResponse.results[i];
            
            // Parse difficulty, convert first letter to uppercase
            question.Difficulty = result.difficulty;
            question.Difficulty = char.ToUpper(question.Difficulty[0]) + question.Difficulty.Substring(1);
            // Parse category, use helper function to decode html encoding and strip ignore words
            question.Category = ParseCategoryString(HttpUtility.HtmlDecode(result.category));
            question.Question = HttpUtility.HtmlDecode(result.question);
            // Parse correct answer, use helper function to decode html encoding
            question.CorrectAnswer = HttpUtility.HtmlDecode(result.correct_answer);
            // Parse incorrect answers, iterate through each and use helper function to decode html encoding
            question.IncorrectAnswers = new string[result.incorrect_answers.Length];
            for (int j = 0; j < result.incorrect_answers.Length; j++)
            {
                question.IncorrectAnswers[j] = HttpUtility.HtmlDecode(result.incorrect_answers[j]);
            }

            // Add the TriviaQuestion to the question set
            questionSet[i] = question;
        }

        // Return the question set
        return questionSet;
        
    }

    /// <summary>
    /// Retrieves a list of all trivia categories available
    /// </summary>
    /// <returns></returns>
    public static List<TriviaCategory> GetCategories()
    {
        // Retrieve the JSON string from OpenTriviaDB and deserialize into OpenTdbCategoryResponse class
        string json = RetrieveJSON(CATEGORIES_URL);
        OpenTdbCategoryResponse apiCategoryResponse = JsonUtility.FromJson<OpenTdbCategoryResponse>(json);
        
        // Initialize list of trivia categories
        List<TriviaCategory> categories = new List<TriviaCategory>();

        // Iterate through each API category response and add a new corresponding TrivaCategory to list
        foreach (OpenTdbCategoryResponse.Category apiCategory in apiCategoryResponse.trivia_categories)
        {
            TriviaCategory category = new TriviaCategory();
            category.Id = apiCategory.id;
            // Use helper funtion to remove ignore words from category names
            category.Name = ParseCategoryString(apiCategory.name);
            categories.Add(category);
        }
        // Alphabetize and return categories
        categories = categories.OrderBy(c => c.Name).ToList();
        return categories;
    }

    /// <summary>
    /// Helper function to remove ignore words from a category string <br/>
    /// Example: "Entertainemnt: Movies" -> "Movies"
    /// </summary>
    /// <param name="unparsedString"></param>
    /// <returns></returns>
    private static string ParseCategoryString(string unparsedString)
    {
        foreach (string ignoreString in CATEGORY_STRING_IGNORES)
        {
            if (unparsedString.Contains(ignoreString))
            {
                unparsedString = unparsedString.Remove(0, ignoreString.Length);
            }
        }
        return unparsedString;
    }

    /// <summary>
    /// Helper function to use .Net.http to call API URL and return the JSON <br/>
    /// Note: This runs synchronously and results in "stutter" as thread waits for response
    /// </summary>
    /// <param name="url">The URL to post a Get request to</param>
    /// <returns>JSON string</returns>
    private static string RetrieveJSON(string url)
    {
        /*
         Notes:
            * The syntax "using()" implements the Dispose Pattern
                * The resource in () will be disposed of at end of block
            * We use .Net.Http HttpClient to handle API Get request
                * Call Http Get method at specified URI
                * Parse the http response content as a string for json
                * .Result synchronously returns value from async call 
        */
        using (var client = new HttpClient())
        {
            Uri endpoint = new Uri(url);
            HttpResponseMessage response = client.GetAsync(endpoint).Result;
            HttpContent content = response.Content;
            string json = content.ReadAsStringAsync().Result;
            return json;
        }
    }

    /// <summary>
    /// Helper function to format the proper API URL to call based on game settings.
    /// </summary>
    /// <param name="gameSettings"></param>
    /// <returns>Formatted URL</returns>
    public static string FormURL(GameSettings gameSettings)
    {
        // URL FORM: opentdb.com/api.php?amount=AMOUNT&category=CAT_ID&difficulty=DIFFICULTY&type=TYPE
        
        string url = "https://opentdb.com/api.php?amount=" + gameSettings.NumQuestions;
        if (gameSettings.Category.Id != -1)
        {
            url += "&category=" + gameSettings.Category.Id.ToString();
        }
        if (gameSettings.Difficulty != TriviaDifficulty.Any)
        {
            url += "&difficulty=";
            switch (gameSettings.Difficulty)
            {
                case TriviaDifficulty.Easy:
                    url += "easy";
                    break;
                case TriviaDifficulty.Medium:
                    url += "medium";
                    break;
                case TriviaDifficulty.Hard:
                    url += "hard";
                    break;  
            }
        }
        if (gameSettings.QuestionType != TriviaQuestionType.Mixed)
        {
            url += "&type=";
            switch (gameSettings.QuestionType)
            {
                case TriviaQuestionType.MultipleChoice:
                    url += "multiple";
                    break;
                case TriviaQuestionType.TrueFalse:
                    url += "boolean";
                    break;
            }
        }
        return url;
    }

    /// <summary>
    /// The Response Code from the API call
    /// </summary>
    public enum ResponseCode
    {
        Success = 0,
        NoResults = 1,
        InvalidParameter = 2,
        TokenNotFound = 3,
        TokenEmpty = 4,
        RateLimit = 5,
        None = -1
    }

}

