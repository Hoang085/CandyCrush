using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButtonNotScale : Button
{
    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        if (this.IsInteractable() == false) return;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        if (this.IsInteractable() == false) return;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        if (this.IsInteractable() == false) return;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        if (this.IsInteractable() == false) return;
    }
}