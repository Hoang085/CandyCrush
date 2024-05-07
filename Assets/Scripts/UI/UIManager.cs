using DG.Tweening;
using H2910.Common.Singleton;
using H2910.Defines;
using H2910.UI.Popups;
using UnityEngine;

public class UIManager : ManualSingletonMono<UIManager>
{
    
    [SerializeField] private Camera UICamera;
    private Tween _tween;
    private bool _onClick;
    public Camera CameraUI => UICamera;

    public void OnBtnMailClick()
    {
        if (_onClick)
            return;
        BlockMultyClick();
        PopupManager.Instance.OnShowScreen(PopupName.Mail);
    }

    private void BlockMultyClick()
    {
        _onClick = true;
        DOVirtual.DelayedCall(0.1f, () => { _onClick = false; });
    }    
    
    public void SetBlockClick(bool isBlock)
    {
        _onClick = isBlock;
    }
    
    public void OnBtnSettingClick()
    {
        if (_onClick)
            return;
        BlockMultyClick();
        PopupManager.Instance.OnShowScreen(PopupName.Setting, ParentPopup.Hight);
    }
    
    
    public void OnBtnShop()
    {
        if (_onClick)
            return;
        BlockMultyClick();
        PopupManager.Instance.OnShowScreen(PopupName.ShopAll);
    }
    
    public void ShowUIHome(bool isShow = true)
    {
        /*canvas.alpha = isShow ? 0 : 1;
        canvas.blocksRaycasts = !isShow;*/
    } 
}