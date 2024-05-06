using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TabButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TabGroup tabGroup;
    [SerializeField] private GameObject offView = null;
    [SerializeField] private GameObject onView = null;
    [SerializeField] private int index;
    public UnityEvent onTabSelected;
    public UnityEvent onTabDeSelected;

    public void OnPointerClick(PointerEventData eventData)
    {
        tabGroup?.OnTabSelected(index);
    }

    public void Selected()
    {
        if (onTabSelected != null)
            onTabSelected?.Invoke();
        if (offView != null) 
            offView?.SetActive(false);
        if (onView != null)
            onView?.SetActive(true);
    }

    public void DeSelected()
    {
        if (onTabDeSelected != null)
            onTabDeSelected?.Invoke();
        if(offView!=null)
            offView?.SetActive(true);
        if(onView!=null)
            onView?.SetActive(false);
    }
}