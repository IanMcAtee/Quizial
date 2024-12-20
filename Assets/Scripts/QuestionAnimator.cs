using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

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


    public void PlayEnterAnimation()
    {
        StartCoroutine(AnimationRoutine(AnimationDirection.Enter));
    }

    public void PlayExitAnimation()
    {
        StartCoroutine(AnimationRoutine(AnimationDirection.Exit));
    }

    public void ResetPosition()
    {
        _questionUIRectTransfrom.localPosition = _originalPosition;  
    }

    private IEnumerator AnimationRoutine(AnimationDirection direction)
    {
        IsAnimating = true;

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

        float elapsedTime = 0f;

        while (elapsedTime < animationCurve.keys[^1].time)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
            float newPosition = animationCurve.Evaluate(elapsedTime);
            _questionUIRectTransfrom.localPosition = new Vector2(newPosition, 0);
            
            
        }
        IsAnimating = false;
    }

    private enum AnimationDirection
    {
        Enter,
        Exit
    }

}
