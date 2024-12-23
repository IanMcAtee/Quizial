using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// Extends UnityEngine.UI.Button to implement a toggle button
/// </summary>
public class ToggleButton : Button
{

    [field: SerializeField, Header("Toggle Properties")]
    public bool IsToggled { get; private set; } = false;
    [field: SerializeField]
    public Color ToggledColor { get; private set; } = Color.white;
    [field: SerializeField]
    public Sprite ToggledSprite { get; private set; } = null;
    [field: SerializeField]
    public UnityEvent
        OnToggleSelect = null,
        OnToggleDeselect = null;

    protected Sprite _normalSprite = null;
     
    protected override void Awake()
    {
        base.Awake();
        // Add the toggle method to the onClick event of the base button
        onClick.AddListener(Toggle); 
        // Cache the original normal sprite
        _normalSprite = image.sprite;
    }

    /// <summary>
    /// Public method to toggle the button. <br/>
    /// Invokes OnToggleSelect/Deselect depending on button state.
    /// </summary>
    public void Toggle()
    {
        IsToggled = !IsToggled;
        if (IsToggled)
        {
            SetToggleSelectedAppearance();
            OnToggleSelect?.Invoke();
        }
        else
        {
            SetToggleDeselectedAppearance();
            OnToggleDeselect?.Invoke();
        }
    }

    /// <summary>
    /// Private method to set the color of button image or sprite to match toggled state
    /// </summary>
    /// <exception cref="Exception"></exception>
    private void SetToggleSelectedAppearance()
    {
        switch (transition)
        {
            case Transition.None:
                image.color = ToggledColor;
                break;
            case Transition.ColorTint:
                image.color = ToggledColor;
                // Need the button to not be in "selected" state after toggle
                EventSystem.current.SetSelectedGameObject(null);
                break;
            case Transition.SpriteSwap:
                image.sprite = ToggledSprite;
                // Need the button to not be in "selected" state after toggle
                EventSystem.current.SetSelectedGameObject(null);
                break;
            case Transition.Animation:
                throw new Exception("Animation transition not currently supported on Toggle Button");
        }
    }

    /// <summary>
    /// Private method to set the color of button image or sprite to match untoggled state
    /// </summary>
    /// <exception cref="Exception"></exception>
    private void SetToggleDeselectedAppearance()
    {
        switch (transition)
        {
            case Transition.None:
                image.color = colors.normalColor;
                break;
            case Transition.ColorTint:
                image.color = colors.normalColor; ;
                EventSystem.current.SetSelectedGameObject(null);
                break;
            case Transition.SpriteSwap:
                image.sprite = _normalSprite;
                EventSystem.current.SetSelectedGameObject(null);
                break;
            case Transition.Animation:
                throw new Exception("Animation transition not currently supported on Toggle Button");
          
        }
    }
}