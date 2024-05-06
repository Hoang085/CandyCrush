using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class UIHomeItemMoving : MonoBehaviour
{
    [SerializeField] private Vector2 startPos;
    [SerializeField] private Vector2 endPos;
    [SerializeField] private float duration;
    [SerializeField] private float distanceRandomPosY;
    private RectTransform _rectTransform;
    private Vector2 _randomPosY;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _randomPosY = GetRandomPos();
        _rectTransform.anchoredPosition = startPos + _randomPosY;
        _rectTransform.DOAnchorPos(endPos + _randomPosY, duration).OnComplete(()=> 
        {
            _rectTransform.anchoredPosition = (startPos + _randomPosY); 
        }).SetLoops(int.MaxValue).SetEase(Ease.Linear);
    }

    public Vector2 GetRandomPos()
    {
        return new Vector2(0, Random.Range(-distanceRandomPosY, distanceRandomPosY));
    }
}