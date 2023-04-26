using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum UIAnimationType
{
    Move,
    Scale,
    ScaleX,
    ScaleY,
    Fade
}

public class UITweener : MonoBehaviour
{
    public GameObject objectToAnimate;

    public UIAnimationType animationType;
    public LeanTweenType easeType;
    public float duration;
    public float delay;

    public bool loop;
    public bool pingpong;

    public Vector3 from;
    public Vector3 to;

    private LTDescr _tweenObject;

    public UnityEvent onShowCompleteCallback;
    public UnityEvent onHideCompleteCallback;

    private bool isReversed;
    private bool hasSetup;

    private void Setup()
    {
        if (objectToAnimate == null)
            objectToAnimate = gameObject;

        if (hasSetup)
            return;

        switch (animationType)
        {
            case UIAnimationType.Move:
                objectToAnimate.GetComponent<RectTransform>().anchoredPosition = from;
                break;
            case UIAnimationType.Fade:
                CanvasGroup canvGroup = objectToAnimate.GetComponent<CanvasGroup>();
                if (canvGroup == null)
                    canvGroup = objectToAnimate.AddComponent<CanvasGroup>();
                canvGroup.alpha = from.x;
                break;
            case UIAnimationType.ScaleX:
            case UIAnimationType.ScaleY:
            case UIAnimationType.Scale:
                objectToAnimate.GetComponent<RectTransform>().localScale = from;
                break;
        }
        hasSetup = true;
    }

    public void Show()
    {
        gameObject.SetActive(true);
        HandleTween(false);
    }

    public void Hide()
    {
        HandleTween(true);
    }

    public void Toggle()
    {
        if (objectToAnimate.activeSelf)
            isReversed = true;

        if (!isReversed)
            objectToAnimate.SetActive(true);

        HandleTween(isReversed);

        isReversed = !isReversed;
    }

    private void HandleTween(bool reversed)
    {
        Setup();
        LeanTween.reset();

        if (objectToAnimate == null)
            objectToAnimate = gameObject;

        switch (animationType)
        {
            case UIAnimationType.Fade:
                Fade(reversed);
                break;
            case UIAnimationType.Move:
                MoveAbsolute(reversed);
                break;
            case UIAnimationType.ScaleX:
            case UIAnimationType.ScaleY:
            case UIAnimationType.Scale:
                Scale(reversed);
                break;
        }

        _tweenObject.setDelay(delay);
        _tweenObject.setEase(easeType);
        _tweenObject.setIgnoreTimeScale(true);

        if (reversed)
            _tweenObject.setOnComplete(OnHideComplete);
        else
            _tweenObject.setOnComplete(OnShowComplete);

        if (loop)
            _tweenObject.loopCount = int.MaxValue;

        if (pingpong)
            _tweenObject.setLoopPingPong();
    }

    private void OnShowComplete()
    {
        if (onShowCompleteCallback != null)
            onShowCompleteCallback.Invoke();
    }

    private void OnHideComplete()
    {
        if (onHideCompleteCallback != null)
            onHideCompleteCallback.Invoke();
    }

    private void Fade(bool reversed)
    {
        float fromValue = from.x;
        float toValue = to.x;

        if (reversed)
        {
            fromValue = to.x;
            toValue = from.x;
        }

        _tweenObject = LeanTween.alphaCanvas(objectToAnimate.GetComponent<CanvasGroup>(), toValue, duration);
    }

    private void MoveAbsolute(bool reversed)
    {
        Vector3 fromValue = from;
        Vector3 toValue = to;

        if (reversed)
        {
            fromValue = to;
            toValue = from;
        }

        _tweenObject = LeanTween.move(GetComponent<RectTransform>(), toValue, duration);
    }

    private void Scale(bool reversed)
    {
        Vector3 fromValue = from;
        Vector3 toValue = to;

        if (reversed)
        {
            fromValue = to;
            toValue = from;
        }

        _tweenObject = LeanTween.scale(objectToAnimate, toValue, duration);
    }
}
