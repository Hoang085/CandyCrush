using System;
using System.Collections;
using System.Collections.Generic;
using H2910.Defines;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IAPPanel : PanelBase
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private GameObject commonTag;
    [SerializeField] private GameObject characterTag;
    [SerializeField] private GameObject supportTag;
    [SerializeField] private GameObject commonPanel;
    [SerializeField] private GameObject characterPanel;
    [SerializeField] private GameObject supportPanel;
    [SerializeField] private TextMeshProUGUI outOfStockTxt;
    private Param _param;
    private PopupShopAll _popupShopAll;
    private GameObject _currentPanelSelect;
    
     public override void OnCloseScreen()
    {
        base.OnCloseScreen();
        /*for (int i = 0; i < items.Count; i++)
        {
            
        }
        {
            items[i].ToggleGameObject(false);
        }
        IAPManager.Instance.OnPurchasedComplete -= OnPurcharsingComplete;
        IAPManager.Instance.OnUpdatePack -= Refresh;*/
    }

    public override void OnInitScreen(object arg)
    {
        base.OnInitScreen(arg);
        //_popup = arg as PopupShopAll;
    }

    public override void OnShowScreen(object arg)
    {
        base.OnShowScreen(arg);
        _param = (Param)arg;
        /*IAPManager.Instance.OnPurchasedComplete += OnPurcharsingComplete;
        IAPManager.Instance.OnUpdatePack += Refresh;*/
        Refresh();
        //scroll.normalizedPosition = Vector2.zero;
        if (_param.PackTab == PackTab.Support)
            OnSupportTagClick();
        else if (_param.PackTab == PackTab.Character)
            OnCharacterTagClick();
        else
            CheckHasPackFistOpen();
    }
    public override void OnShowScreen()
    {
        base.OnShowScreen();
        /*IAPManager.Instance.OnPurchasedComplete += OnPurcharsingComplete;
        IAPManager.Instance.OnUpdatePack += Refresh;*/
        Refresh();
        //scroll.normalizedPosition = Vector2.zero;
        CheckHasPackFistOpen();
    }

    private void CheckHasPackFistOpen()
    {
        var listPack = commonPanel.GetComponentsInChildren<UIPSItem>();
        if (listPack.Length == 0)
        {
            listPack = characterPanel.GetComponentsInChildren<UIPSItem>();
            if (listPack.Length == 0)
                OnSupportTagClick();
            else
                OnCharacterTagClick();
        }
        else
            OnCommonTagClick();
    }

    private void OnPurcharsingComplete(IAPType iAPType)
    {
        Refresh();
        EnableOutOfStockTxt();
    }

    private void Refresh()
    {
        /*for (int i = 0; i < items.Count; i++)
        {
            if (!IAPManager.Instance.IsActivePack(items[i].IAPType))
            {
                items[i].ToggleGameObject(false);
            }
            else
            {
                items[i].ToggleGameObject(true);
            }
        }*/
    }
    public void OnCommonTagClick()
    {
        SetUpTagAndPanel(commonTag, commonPanel);
        DeactiveAnotherTagAndPanel(characterTag, characterPanel);
        DeactiveAnotherTagAndPanel(supportTag, supportPanel);
    }

    public void OnCharacterTagClick()
    {
        SetUpTagAndPanel(characterTag, characterPanel);
        DeactiveAnotherTagAndPanel(commonTag, commonPanel);
        DeactiveAnotherTagAndPanel(supportTag, supportPanel);
    }

    public void OnSupportTagClick()
    {
        SetUpTagAndPanel(supportTag, supportPanel);
        DeactiveAnotherTagAndPanel(commonTag, commonPanel);
        DeactiveAnotherTagAndPanel(characterTag, characterPanel);
    }

    private void SetUpTagAndPanel(GameObject tag, GameObject panel)
    {
        panel.SetActive(true);
        tag.SetActive(true);
        _currentPanelSelect = panel;
        EnableOutOfStockTxt();
        scrollRect.content = panel.GetComponent<RectTransform>();
        panel.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
        StartCoroutine(PlayAnimItem());
    }
    IEnumerator PlayAnimItem()
    {
        /*for (int i = 0; i < items.Count; i++)
        {
            if (!items[i].gameObject.activeInHierarchy)
                continue;
            else
            {
                yield return new WaitForSecondsRealtime(.1f);
                items[i].gameObject.GetComponent<AppearanceEffect>().StartEffect();
            }
        }*/
        yield return new WaitForSecondsRealtime(.1f);
    }
    private void EnableOutOfStockTxt()
    {
        if (_currentPanelSelect != null)
        {
            var listPack = _currentPanelSelect.GetComponentsInChildren<UIPSItem>();
            outOfStockTxt.gameObject.SetActive(listPack.Length == 0);
        }
    }

    private void DeactiveAnotherTagAndPanel(GameObject tag, GameObject panel)
    {
        tag.SetActive(false);
        panel.SetActive(false);
    }

    public class Param
    {
        public Action CallBack;
        public PackTab PackTab = PackTab.Common;
    }

    public enum PackTab
    {
        Common = 1,
        Character = 2,
        Support = 3
    }
}
