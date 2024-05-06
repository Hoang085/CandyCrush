using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using H2910.Defines;
using H2910.UI.Popups;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace H2910.UI.Popups
{
    public class BasePopUp : MonoBehaviour
    {
        [SerializeField] private List<Button> listButtonControl;
        [SerializeField] private bool enableGoldUI;
        [SerializeField] private PopupShowingType showingType = PopupShowingType.FadeInOut;
        [SerializeField] private GameObject blockRaycast;
        public PopupName currentScreen;
        private CanvasGroup _canvasGroup;
        private Tween _tween,_tween2;
        protected bool OnClick;
        public bool CanClick => !OnClick;
        public bool IsShow => _canvasGroup.alpha == 1 && IsClosing == false;
        public bool IsShowing { get; private set; }
        public bool IsClosing { get; private set; }
        public bool EnableGoldUI => enableGoldUI;

        protected void Awake()
        {
            this.OnInitScreen();
            if (_canvasGroup == null)
            {
                _canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
        }
        public virtual void OnInitScreen(){}

        public virtual void OnShowScreen()
        {
            if(blockRaycast != null)
                blockRaycast.SetActive(false);
            BlockMultyClick();
            if (showingType == PopupShowingType.FadeInOut)
                OnFadeIn();
            else
                ShowFullScreen();
        }

        public virtual void OnShowScreen(object arg)
        {
            BlockMultyClick();
            if(blockRaycast!=null)
                blockRaycast.SetActive(false);
            if (showingType == PopupShowingType.FadeInOut)
                OnFadeIn();
            else
                ShowFullScreen();
        }

        public virtual void OnShowScreen(object[] args)
        {
            BlockMultyClick();
            if(blockRaycast!=null)
                blockRaycast.SetActive(false);
            if (showingType == PopupShowingType.FadeInOut)
                OnFadeIn();
            else
                ShowFullScreen();
        }

        public virtual void OnCloseScreen()
        {
            if (showingType == PopupShowingType.FadeInOut)
                OnFadeOut();
            else
                HideScreen();
            BlockMultyClick();
        }

        public void OnDeActived()
        {
            this.transform.localScale = Vector3.zero;
            _canvasGroup.alpha = 0f;
        }

        private void ShowFullScreen()
        {
            gameObject.SetActive(true);
            transform.SetAsLastSibling();
            _canvasGroup.alpha = 1;
            IsShowing = false;
            transform.localScale = Vector3.zero;
            if (enableGoldUI)
                PopupManager.Instance.ToggleGoldPanel(true);
        }

        private void HideScreen()
        {
            IsClosing = false;
            transform.localScale = new Vector3(0, 1, 1);
            gameObject.SetActive(false);
            _canvasGroup.alpha = 0;
            DOVirtual.DelayedCall(0.3f, () => { PopupManager.Instance.CheckResumeGame(); }).SetUpdate(true);
            IsClosing = false;
            if(enableGoldUI)
                PopupManager.Instance.ToggleGoldPanel(false);
        }

        private void OnFadeIn()
        {
            if(IsShowing)
                return;
            IsShowing = true;
            gameObject.SetActive(true);
            transform.SetAsLastSibling();
            _canvasGroup.alpha = 0.5f;
            transform.localScale = Vector3.one * 0.8f;
            _tween?.Kill();
            _tween2?.Kill();
            _tween = _canvasGroup.DOFade(1f, 0.2f).SetEase(Ease.InQuad).SetUpdate(true).OnComplete(() =>
            {
                _canvasGroup.alpha = 1;
                IsShowing = false;
            });
            _tween2 = transform.DOScale(1, 0.2f).SetUpdate(true);
            if(enableGoldUI)
                PopupManager.Instance.ToggleGoldPanel(true);
        }

        private void OnFadeOut()
        {
            IsClosing = true;
            IsShowing = false;
            _tween?.Kill();
            _tween2?.Kill();
            _canvasGroup.alpha = 0.5f;
            _tween2 = transform.DOScale(1.15f, 0.15f).SetUpdate(true);
            _tween = _canvasGroup.DOFade(0f, 0.2f).SetEase(Ease.OutQuad).SetUpdate(true).OnComplete(() =>
            {
                this.transform.localScale = new Vector3(0, 1, 1);
                gameObject.SetActive(false);
                _canvasGroup.alpha = 0;
                PopupManager.Instance.CheckResumeGame();
                IsClosing = false;
            });
            if(enableGoldUI)
                PopupManager.Instance.ToggleGoldPanel(false);
        }

        public void BlockMultyClick()
        {
            OnClick = true;
            DOVirtual.DelayedCall(0.2f, () => OnClick = false);
            for (int i = 0; i < listButtonControl.Count; i++)
            {
                listButtonControl[i].interactable = false;
            }

            DOVirtual.DelayedCall(0.2f, () =>
            {
                for (int i = 0; i < listButtonControl.Count; i++)
                {
                    listButtonControl[i].interactable = true;
                }
            });
        }

        public void SetInteractableControlButton(bool value)
        {
            foreach (var button in listButtonControl)
            {
                button.interactable = value;
            }
        }

        public void BlockRayCast(bool isActive)
        {
            if (blockRaycast != null)
                blockRaycast.SetActive(isActive);
        }

        public void BlockRayCast(float timeBlock)
        {
            if (blockRaycast == null)
                return;
            blockRaycast.SetActive(true);
            DOVirtual.DelayedCall(timeBlock, () =>
            {
                blockRaycast.SetActive(false);
            }).SetUpdate(true);
        }
    }
}