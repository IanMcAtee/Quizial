
using System;
using System.Net.Http;
using System.Web;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public static class OpenTdbAPIHelper
{
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

    private const string CATEGORIES_URL = "https://opentdb.com/api_category.php";
    private static readonly string[] CATEGORY_STRING_IGNORES = { "Entertainment: ", "Science: " };

    public static TriviaQuestion[] GetQuestionSet(string questionURL)
    {
            string json = RetrieveJSON(questionURL);

            OpenTdbAPIResponse apiResponse = JsonUtility.FromJson<OpenTdbAPIResponse>(json);

            TriviaQuestion[] questionSet = new TriviaQuestion[apiResponse.results.Length];
            TriviaQuestion curQuestion;
            OpenTdbAPIResponse.Result curResult;
            for (int i = 0; i < apiResponse.results.Length; i++)
            {
                curQuestion = new TriviaQuestion();
                curResult = apiResponse.results[i];
                
                
                curQuestion.Number = i + 1;
                curQuestion.Difficulty = curResult.difficulty;
                curQuestion.Category = HttpUtility.HtmlDecode(curResult.category);
                curQuestion.Question = HttpUtility.HtmlDecode(curResult.question);
                curQuestion.CorrectAnswer = HttpUtility.HtmlDecode(curResult.correct_answer);
                curQuestion.IncorrectAnswers = new string[curResult.incorrect_answers.Length];
                for (int j = 0; j < curResult.incorrect_answers.Length; j++)
                {
                    curQuestion.IncorrectAnswers[j] = HttpUtility.HtmlDecode(curResult.incorrect_answers[j]);
                }

                questionSet[i] = curQuestion;
            }

            return questionSet;
        
    }

    public static List<TriviaCategory> GetCategories()
    {
        string json = RetrieveJSON(CATEGORIES_URL);
        OpenTdbCategoryResponse apiCategoryResponse = JsonUtility.FromJson<OpenTdbCategoryResponse>(json);
        
        List<TriviaCategory> categories = new List<TriviaCategory>();

        foreach (OpenTdbCategoryResponse.Category apiCategory in apiCategoryResponse.trivia_categories)
        {
            TriviaCategory category = new TriviaCategory();
            
            category.Id = apiCategory.id;

            category.Name = apiCategory.name;

            foreach (string ignoreString in CATEGORY_STRING_IGNORES)
            {
                if (category.Name.Contains(ignoreString))
                {
                    category.Name = category.Name.Remove(0, ignoreString.Length);
                }
            }

            categories.Add(category);
        }
        categories = categories.OrderBy(c => c.Name).ToList();
        return categories;
    }
    
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

    private static string FormURL(int numQuestions=10, int categoryID=0, string difficulty="", string type="")
    {
        string url = "https://opentdb.com/api.php?amount=" + numQuestions;
        if (categoryID !=  0)
        {
            url += "&category=" + categoryID.ToString();
        }
        if (difficulty != "")
        {
            url += "&difficulty=" + difficulty;
        }
        if (type != "")
        {
            url += "&type=" + type;
        }
        return url;
    }

    private static string FormURL(GameSettings gameSettings)
    {
        return string.Empty;
    }

}

