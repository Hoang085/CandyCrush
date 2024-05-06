using System;
using System.Collections;
using System.Collections.Generic;
using H2910.PackShop;
using H2910.UI.Popups;
using UnityEngine;

public class PopupShopAll : BasePopUp
{
    [SerializeField] private List<PanelBase> panelBases;
    [SerializeField] private TabGroup tabGroup;
    private Param _param;

    public override void OnInitScreen()
    {
        base.OnInitScreen();
        foreach (var item in panelBases)
        {
            item.OnInitScreen(this);
            item.gameObject.SetActive(false);
        }
    }

    public override void OnShowScreen()
    {
        base.OnShowScreen();
        tabGroup.OnTabSelected(0,true);
        panelBases[0].OnShowScreen(new IAPPanel.Param{PackTab =0});
    }

    public override void OnShowScreen(object arg)
    {
        base.OnShowScreen(arg);
        _param = (Param)arg; 
        tabGroup.OnTabSelected((int)_param.ShopTab, true);
        switch (_param.ShopTab)
        {
            case ShopTab.Packs:
                panelBases[(int)_param.ShopTab].OnShowScreen(new IAPPanel.Param { PackTab = (IAPPanel.PackTab)_param.TabDefault });
                break;
            case ShopTab.Gold:
                panelBases[(int)_param.ShopTab].OnShowScreen(new GoldPanel.Param{ TabDefault = _param.TabDefault });
                break;
            case ShopTab.Item:
                panelBases[(int)_param.ShopTab].OnShowScreen();
                break;
            default:
                break;
        }
    }

   public void ShowPanel(ShopTab shopTab, int tabDefault)
    {
        tabGroup.OnTabSelected((int)shopTab, true);
        switch (shopTab)
        {
            case ShopTab.Packs:
                panelBases[(int)shopTab].OnShowScreen(new IAPPanel.Param{PackTab = (IAPPanel.PackTab)tabDefault});
                break;
            case ShopTab.Gold:
                panelBases[(int)shopTab].OnShowScreen(new GoldPanel.Param { TabDefault = tabDefault });
                break;
            case ShopTab.Item:
                panelBases[(int)shopTab].OnShowScreen();
                break;
            default:
                break;
        }
    }

    public override void OnCloseScreen()
    {
        base.OnCloseScreen();
        foreach (var item in panelBases)
        {
            item.OnCloseScreen();
        }
        _param?.CallBack?.Invoke();
        _param = null;
    }
    public class Param
    {
        public Action CallBack;
        public int TabDefault = 0;
        public ShopTab ShopTab;
    }

    public enum ShopTab
    {
        Packs = 0,
        Gold = 1,
        Item = 2,
    }
}
