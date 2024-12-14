using UnityEngine;
using TMPro;

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

    //private void InstantiateCategoryButtons()
    //{
    //    foreach (TriviaCategory category in SettingsManager.Instance.AvailableCategories)
    //    {
    //        GameObject button = Instantiate(_categoryButtonPrefab, _scrollViewContent);
    //        CategoryButton categoryButton = button.GetComponent<CategoryButton>();
    //        if (category.Name == SettingsManager.Instance.Settings.Category.Name)
    //        {
    //            categoryButton.SetToggleState(true);
    //        }
    //        else
    //        {
    //            categoryButton.SetToggleState(false); 
    //        }

    //        // Set button text
    //        categoryButton.SetButtonText(category.Name);

    //        // Set the button's associated category property
    //        categoryButton.AssociatedCategory = category;
    //    }
    //}

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
