using System;
using UnityEngine;
using UnityEngine.UI;
using H2910.Defines;
using UnityEngine.Events;

public class InputSetting : MonoBehaviour
{
    [SerializeField] private Image selectImg;
    [SerializeField] private UIButtonNotScale uiButton;
    [SerializeField] private InputType inputType;
    private int _index;
    
    internal int Index
    {
        get => _index;
        set => _index = value;
    }

    private void Reset()
    {
        uiButton = GetComponent<UIButtonNotScale>();
    }

    internal void AddOnClickListener(UnityAction unityAction)
    {
        uiButton.onClick.AddListener(() =>
        {
            unityAction?.Invoke();
        });
    }

    internal void RemoveListener()
    {
        uiButton.onClick.RemoveAllListeners();
    }

    internal InputType GetInputType()
    {
        return inputType;
    }

    internal void ToggleSelectImg(bool enable)
    {
        selectImg.gameObject.SetActive(enable);
    }
}