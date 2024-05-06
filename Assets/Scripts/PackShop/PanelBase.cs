using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PanelBase : MonoBehaviour
{
    protected bool OnClick;
    public bool CanClick => !OnClick;

    public virtual bool CanShowPanel()
    {
        return true;
    }

    public virtual void OnInitScreen(object arg)
    {
        
    }

    public virtual void OnShowScreen()
    {
        gameObject.SetActive(true);
    }

    public virtual void OnShowScreen(object arg)
    {
        gameObject.SetActive(true);
    }

    public virtual void OnCloseScreen()
    {
        gameObject.SetActive(false);
    }

    public void BlockMultyClick()
    {
        OnClick = true;
        DOVirtual.DelayedCall(0.2f, () => OnClick = false).SetUpdate(true);
    }
}