using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using H2910.Defines;

public class TextNotice : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Ease ease;
    private RectTransform _rectTransform;
    private Tween _tween1, _tween2;

    public void ShowNotice(string s, NoticeColor color)
    {
        _tween1?.Kill();
        _tween2?.Kill();
        if (_rectTransform == null)
            _rectTransform = GetComponent<RectTransform>();
        gameObject.SetActive(true);
        text.text = s;
        switch (color)
        {
            case NoticeColor.White:
                text.color = Color.white;
                break;
            default:
                text.color = Color.red;
                break;
        };
        _rectTransform.anchoredPosition = Vector2.zero;
        _tween2 = _rectTransform.DOAnchorPosY(170, 2f).SetEase(ease).SetUpdate(true)
            .OnComplete(() => { gameObject.SetActive(false); });
        
    }
}
