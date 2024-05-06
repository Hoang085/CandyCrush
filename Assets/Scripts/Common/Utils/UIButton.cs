using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using H2910.Base;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace H2910.Common.Utils
{
    public class UIButton : Button
    {
        [SerializeField] private Vector3 targetLargeScale = new Vector3(1.1f, 1.1f, 1f);
        [SerializeField] private Vector3 targetSmallScale = new Vector3(1.05f, 1.05f, 1f);
        [SerializeField] private bool selectLarge;
        [SerializeField] private AudioClipType audioClipType;
        private Vector3 _vector3Default;
        private Vector2 _buttonSize;
        
        protected override void Awake()
        {
            _vector3Default = transform.localScale;
        }

        protected override void Start()
        {
            _buttonSize = transform.GetComponent<RectTransform>().rect.size;
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            if (this.IsInteractable() == false) return;
            if (_buttonSize.x < 800)
                transform.DOScale(targetLargeScale, 0.1f).SetUpdate(true);
            else
            {
                transform.DOScale(targetSmallScale, 0.1f).SetUpdate(true);
            }
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            if (this.IsInteractable() == false) return;
            transform.DOScale(_vector3Default, 0.1f).SetUpdate(true);
        }
        
        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            if (this.IsInteractable() == false) return;
            transform.DOScale(new Vector3(0.9f,0.9f,1f), 0.1f).SetUpdate(true);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            if (this.IsInteractable() == false) return;
            transform.DOScale(_vector3Default, 0.1f).SetUpdate(true);
        }

        public void TranStateToNormal()
        {
            if (this.IsInteractable())
            {
                this.DoStateTransition(SelectionState.Normal,true);
            }
            transform.DOScale(_vector3Default, 0.1f).SetUpdate(true);
        }
    }
}