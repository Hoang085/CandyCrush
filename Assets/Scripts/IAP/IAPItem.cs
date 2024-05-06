using System;
using System.Globalization;
using DG.Tweening;
using UnityEngine;
using H2910.Defines;
using TMPro;

public class IAPItem : MonoBehaviour
{
    [SerializeField] private IAPType IAPType;
    //[SerializeField] private GemExchangeShop iapShop;
    [SerializeField] private TextMeshProUGUI txtName;
    [SerializeField] private int quantity;
    [SerializeField] private TextMeshProUGUI txtBtn;
    private bool _onClick;

    private void Start()
    {
        txtName.text = "x" + quantity.ToString("NO", CultureInfo.InvariantCulture);
        if(txtBtn == null)
            return;
        string txt = IAPManager.Instance.GetTxtPrice(IAPType);
        if (!string.IsNullOrEmpty(txt))
            txtBtn.text = txt;
    }

    public void Onclick()
    {
        if(_onClick)
            return;
        _onClick = true;
        //iapShop.OnItemClick(IAPType);
        DOVirtual.DelayedCall(0.3f, () => { _onClick = false; }, true);
    }
}