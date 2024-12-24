using System.Collections;
using UnityEngine;

/// <summary>
/// Animates the enter and exit of questions
/// </summary>
public class QuestionAnimator : MonoBehaviour
{
    [SerializeField]
    private RectTransform _questionUIRectTransfrom;
    [SerializeField]
    private AnimationCurve _enterAnimationCurve;
    [SerializeField]
    private AnimationCurve _exitAnimationCurve;

    private Vector2 _originalPosition;
    
    public bool IsAnimating { get; private set; } = false;

    private void Start()
    {
        _originalPosition = _questionUIRectTransfrom.localPosition;
    }

    /// <summary>
    /// Plays the enter animation
    /// </summary>
    public void PlayEnterAnimation()
    {
        StartCoroutine(AnimationRoutine(AnimationDirection.Enter));
    }

    /// <summary>
    /// Plays the exit animation
    /// </summary>
    public void PlayExitAnimation()
    {
        StartCoroutine(AnimationRoutine(AnimationDirection.Exit));
    }

    /// <summary>
    /// Resets the animation position
    /// </summary>
    public void ResetPosition()
    {
        _questionUIRectTransfrom.localPosition = _originalPosition;  
    }

    /// <summary>
    /// Coroutine to perform the enter or exit animation
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    private IEnumerator AnimationRoutine(AnimationDirection direction)
    {
        IsAnimating = true;

        // Assign the animation curve based on the direction
        AnimationCurve animationCurve = null;
        switch (direction)
        {
            case AnimationDirection.Enter:
                animationCurve = _enterAnimationCurve;
                break;
            case AnimationDirection.Exit:
                animationCurve = _exitAnimationCurve;
                break;
        }

        // Step through animation curve, and assign position based on curve
        float elapsedTime = 0f;
        while (elapsedTime < animationCurve.keys[^1].time) //^1 means last index
        {
            yield return null;
            elapsedTime += Time.deltaTime;
            float newPosition = animationCurve.Evaluate(elapsedTime);
            _questionUIRectTransfrom.localPosition = new Vector2(newPosition, 0);
        }
        IsAnimating = false;
    }

    /// <summary>
    /// Enumeration defining the animation direction
    /// </summary>
    private enum AnimationDirection
    {
        Enter,
        Exit
    }

}
