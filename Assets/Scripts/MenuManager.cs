using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Menu Properties")]
    [SerializeField]
    private MenuElement[] _menuElements;

    [Header("Paused Menu Properties")]
    [SerializeField]
    private GameObject _questionsPanel;
    [SerializeField]
    private GameObject _pauseButton;
    [SerializeField]
    private GameObject _pausedPanel;

    [Header("Game Over Menu Properties")]
    [SerializeField]
    private Image _scoreCircleImage;
    



    [Header("Category Properties")]
    [SerializeField]
    private GameObject _categoryButtonPrefab;
    [SerializeField]
    private Transform _scrollViewContent;

   
    private void Start()
    {
        GameManager.OnGameStateUpdate += ActivateMenuFromGameState;
        InstantiateCategoryButtons();
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateUpdate -= ActivateMenuFromGameState;
    }

    public void Pause_OnClick()
    {
        GameManager.Instance.UpdateGameState(GameState.Paused);
    }

    public void Resume_OnClick()
    {
        GameManager.Instance.UpdateGameState(GameState.Playing);
    }

    

    public void MainMenu_OnClick()
    {
        GameManager.Instance.UpdateGameState(GameState.MainMenu);
    }

    private void InstantiateCategoryButtons()
    {
        foreach (TriviaCategory category in GameManager.Instance.AvailableCategories)
        {
            GameObject buttonObject = Instantiate(_categoryButtonPrefab, _scrollViewContent);
            CategoryButton categoryButton = buttonObject.GetComponent<CategoryButton>();
            categoryButton.InitializeCategoryButton(category);
        }
    }

    private void ActivateMenuFromGameState(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Paused:
                SetPauseMenu(true);
                break;
            case GameState.Playing:
                SetPauseMenu(false);
                SetSingleMenuActive(GameState.Playing);
                break;
            case GameState.GameOver:
                break;
            default:
                SetSingleMenuActive(gameState);
                break;
        }
    }

    private void SetSingleMenuActive(GameState gameState)
    {
        foreach (MenuElement menuElement in _menuElements)
        {
            if (menuElement.AssociatedGameState == gameState)
            {
                menuElement.gameObject.SetActive(true);
                continue;
            }
            menuElement.gameObject.SetActive(false);
        }
    }

    private void SetPauseMenu(bool isPaused)
    {
        _questionsPanel.SetActive(!isPaused);
        _pauseButton.gameObject.SetActive(!isPaused);
        _pausedPanel.SetActive(isPaused);
    }
}
