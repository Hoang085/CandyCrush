using System;
using System.Collections;
using System.Collections.Generic;
using H2910.Defines;
using UnityEngine;

namespace H2910.PackShop
{
    public class GoldPanel : PanelBase
    {
        [SerializeField] private List<GameObject> goldItem;
        private TabGroup _tabGroup;
        private Param _param;
        private PopupShopAll _popupShopAll;
        private Coroutine _coroutine;

        public override void OnInitScreen(object arg)
        {
            base.OnInitScreen(arg);
            _popupShopAll = arg as PopupShopAll;
            _tabGroup = GetComponent<TabGroup>();
            _tabGroup.Init(OnTabChange);
        }

        public override void OnShowScreen()
        {
            base.OnShowScreen();
            _tabGroup.OnTabSelected(0);
            _param = null;
            if(_coroutine != null)
                StopCoroutine(_coroutine);
            _coroutine = StartCoroutine(PlayAnimShowItem(false));
            OnTabChange(0);
        }

        public override void OnShowScreen(object param)
        {
            base.OnShowScreen();
            _param = (Param)param;
            _tabGroup.SelectDefault(_param.TabDefault);
            OnTabChange(_param.TabDefault);
        }

        public override void OnCloseScreen()
        {
            base.OnCloseScreen();
            _param?.CallBack?.Invoke();
        }
        
        public class Param
        {
            public Action CallBack;
            public int TabDefault = 0;
        }
        
        public void OnTabChange(int tabIndex)
        {
            if (tabIndex == 1)
            {
                if (_coroutine != null)
                    StopCoroutine(_coroutine);
                _coroutine = StartCoroutine(PlayAnimShowItem(true));
            }else if (tabIndex == 0)
            {
                if (_coroutine != null)
                    StopCoroutine(_coroutine);
                _coroutine = StartCoroutine(PlayAnimShowItem(false));
            }
        }

        IEnumerator PlayAnimShowItem(bool isCoin)
        {
            if (isCoin)
            {
                NoticeManager.Instance.TriggerEvent(Notice.DailyGiftGold,false);
                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                NoticeManager.Instance.TriggerEvent(Notice.DailyGiftGem,false);
                yield return new WaitForSeconds(0.1f);
                _coroutine = null;
            }
        }
    }
}