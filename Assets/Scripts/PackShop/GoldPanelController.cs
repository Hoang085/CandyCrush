using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using H2910.Data;
using H2910.Defines;
using H2910.UI.Popups;
using TMPro;
using UnityEngine;

public class GoldPanelController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldTxt;
    [SerializeField] private UIButtonNotScale goldBtn;
    [SerializeField] private TextMeshProUGUI heartTxt;
    [SerializeField] private UIButtonNotScale heartBtn;
    private float _currentGold;
    private float _goldValue;
    private float _currentHeart;
    private float _coolDownHear;
    private float _hearValue;
    private float _durationFx = 0.5f;

    private void OnEnable()
    {
        if (PlayerData.Instance == null)
            return;
        goldTxt.text = _currentGold.ToString("NO",CultureInfo.InvariantCulture);
        _currentGold = 100;
        heartTxt.text = _currentHeart.ToString("NO", CultureInfo.InvariantCulture);
        _currentHeart = 5;
    }
    
    /*public void RunEffect()
    {
        if (currentGold != PlayerData.Instance.PlayerProp.Gold)
        {
            DOTween.To(() => currentGold, x =>
            {
                goldValue = x;
                UpdateGoldTxt();
            }, PlayerData.Instance.PlayerProp.Gold, durationFx).SetUpdate(true).SetEase(Ease.Linear).OnComplete(()=> { currentGold = PlayerData.Instance.PlayerProp.Gold; });
        }
        if (currentGem != PlayerData.Instance.PlayerProp.Gem)
        {
            DOTween.To(() => currentGem, y =>
            {
                gemValue = y;
                UpdateGemTxt();
            }, PlayerData.Instance.PlayerProp.Gem, durationFx).SetUpdate(true).SetEase(Ease.Linear).OnComplete(() => { currentGem = PlayerData.Instance.PlayerProp.Gem; });
        }
    }*/

    public void ClickGoldPanel()
    {
        if (PopupManager.Instance.IsPopupShowing(PopupName.ShopAll))
        {
            var popup = PopupManager.Instance.GetPopup(PopupName.ShopAll);
            if (popup != null)
                popup.GetComponent<PopupShopAll>().ShowPanel(PopupShopAll.ShopTab.Gold, 1);
        }
        else
        {
            PopupManager.Instance.OnShowScreen(PopupName.ShopAll,
                new PopupShopAll.Param { TabDefault = 1, ShopTab = PopupShopAll.ShopTab.Gold });
        }
    }

    public void UpdateGoldTxt()
    {
        goldTxt.text = _goldValue.ToString("NO", CultureInfo.InvariantCulture);
    }

    public void ToggleGoldBtnRaycast(bool enable)
    {
        goldBtn.enabled = enable;
        heartBtn.enabled = enable;
    }
}