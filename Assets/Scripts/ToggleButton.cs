using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

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
        onClick.AddListener(Toggle); 
        _normalSprite = image.sprite;
    }

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

    private void SetToggleSelectedAppearance()
    {
        switch (transition)
        {
            case Transition.None:
                image.color = ToggledColor;
                break;
            case Transition.ColorTint:
                image.color = ToggledColor;
                EventSystem.current.SetSelectedGameObject(null);
                break;
            case Transition.SpriteSwap:
                image.sprite = ToggledSprite;
                EventSystem.current.SetSelectedGameObject(null);
                break;
            case Transition.Animation:
                throw new Exception("Animation transition not currently supported on Toggle Button");
        }
    }

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