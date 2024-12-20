using TMPro;
using UnityEngine;

public class QuoteManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _quoteText;

    private Quote[] _quotes =
    {
        new Quote("Real knowledge is to know the extent of one’s ignorance.",
                  "Confucius"),
        new Quote("There are only two kinds of people who are really fascinating; people who know absolutely everything, and people who know absolutely nothing.",
                  "Oscar Wilde"),
        new Quote("All men by nature desire to know.",
                  "Aristotle"),
        new Quote("If knowledge can create problems, it is not through ignorance that we can solve them.",
                  "Isaac Asimov"),
        new Quote("Ignorance is the curse of God; knowledge is the wing wherewith we fly to heaven.",
                  "William Shakespeare"),
        new Quote("Imagination is more important than knowledge, for knowledge is limited while imagination embraces the entire world.",
                  "Albert Einstein"),
        new Quote("The more extensive a man’s knowledge of what has been done, the greater will be his power of knowing what to do.",
                  "Benjamin Disraeli"),
        new Quote("If a little knowledge is dangerous, where is a man who has so much as to be out of danger?",
                  "Thomas Henry Huxley"),
        new Quote("Integrity without knowledge is weak and useless, and knowledge without integrity is dangerous and dreadful.",
                  "Samuel Johnson"),
        new Quote("Knowing is not enough; we must apply. Willing is not enough; we must do.",
                  "Johann Wolfgang von Goethe"),
        new Quote("Knowledge is knowing that we cannot know.",
                  "Ralph Waldo Emerson"),
        new Quote("One’s mind, once stretched by a new idea, never regains its original dimensions.",
                  "Oliver Wendell Holmes"),
        new Quote("The greater our knowledge increases the more our ignorance unfolds.",
                  "John F. Kennedy"),
    };

    private void OnEnable()
    {
        ShowRandomQuote();
    }

    public void ShowRandomQuote()
    {
        int randIndex = Random.Range(0, _quotes.Length);
        _quoteText.text = _quotes[randIndex].FormatQuote();
    }

    private class Quote
    {
        public string Text;
        public string Author;

        public Quote(string quoteText, string authorName)
        {
            Text = quoteText;
            Author = authorName;
        }

        public string FormatQuote()
        {
            return $"{Text}\n\n- {Author}";
        }
    }
}
