using DG.Tweening;
using H2910.Common.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace H2910.Utils
{
    public class UIFocusMask : ManualSingletonMono<UIFocusMask>
    {
        [SerializeField] private Canvas canvas=null;
        [SerializeField] private CutoutMaskUI cutoutMaskUI = null;
        [SerializeField] private Camera uICamera;
        [SerializeField] RectTransform rectTransform = null;

        private Tween _tweener = null;
        public async void Show(Vector2 screenPoint, Vector3 fromScale, Vector3 toScale,float duration, float delay)
        {
            if (uICamera == null)
            {
                uICamera = UIManager.Instance.CameraUI;
            }
            if(_tweener != null)
            {
                _tweener.Kill();
                _tweener = null;
            }
            if (uICamera != null)
            {
                var pos = uICamera.ScreenToWorldPoint(screenPoint);
                rectTransform.position = pos;
            }
            else
            {
                rectTransform.anchoredPosition = Vector3.zero;
            }
            rectTransform.localScale = fromScale;
            gameObject.SetActive(true);
            _tweener = rectTransform.DOScale(toScale, duration).SetDelay(delay).OnComplete(() =>
            {
                _tweener = null;
            }).SetUpdate(true);
        }
        public async void Hide(Vector3 scale,float duration)
        {
            if (_tweener != null)
            {
                _tweener.Kill();
                _tweener = null;
            }
            _tweener = rectTransform.DOScale(scale, duration).OnComplete(() =>
            {
                _tweener = null;
                gameObject.SetActive(false);
            }).SetUpdate(true);
        }
    }
}