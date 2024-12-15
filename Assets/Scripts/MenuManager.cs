using UnityEngine;
using TMPro;
using UnityEngine.Rendering.LookDev;

public class MenuManager : MonoBehaviour
{
    

    [Header("Category Properties")]
    [SerializeField]
    private GameObject _categoryButtonPrefab;
    [SerializeField]
    private Transform _scrollViewContent;

   
    private void Start()
    {
        InstantiateCategoryButtons();
    }

    private void InstantiateCategoryButtons()
    {
        foreach (TriviaCategory category in SettingsManager.Instance.AvailableCategories)
        {
            GameObject buttonObject = Instantiate(_categoryButtonPrefab, _scrollViewContent);
            CategoryButton categoryButton = buttonObject.GetComponent<CategoryButton>();
            categoryButton.InitializeCategoryButton(category);
        }
    }
}
