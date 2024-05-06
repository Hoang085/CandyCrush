using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIHomePanel : MonoBehaviour
{
    [SerializeField] private Vector2 anchorHidePos;
    [SerializeField] private Ease easeIn;
    [SerializeField] private Ease easeOut;
    [SerializeField] private float duration;
    private Vector2 _anchorShowPos;
    private RectTransform _rect;
    private Tween _tween;
    public bool isShow;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
        _anchorShowPos = _rect.anchoredPosition;
    }

    public void OnShow()
    {
        _tween?.Kill();
        isShow = true;
        _tween = _rect.DOAnchorPos(_anchorShowPos, duration).SetEase(easeIn).SetUpdate(true);
    }

    public void OnHide()
    {
        _tween?.Kill();
        isShow = false;
        _tween = _rect.DOAnchorPos(anchorHidePos, duration).SetEase(easeOut).SetUpdate(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && !isShow)
            OnShow();
        if(Input.GetKeyDown(KeyCode.O) && isShow)
            OnHide();
    }
}