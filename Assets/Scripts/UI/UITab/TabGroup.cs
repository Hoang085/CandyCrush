using System;
using System.Collections;
using System.Collections.Generic;
using H2910.UI.Popups;
using TMPro;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    [SerializeField] private List<TabData> tabDatas;
    [SerializeField] private int tabDefault;
    [SerializeField] private TextMeshProUGUI title;
    private Action<int> m_Callback;
    private int _currentTabSelect = -1;

    public void Setup(Action<int> callBack = null)
    {
        for (int i = 0; i < tabDatas.Count; i++)
        {
            tabDatas[i].tabPanel.gameObject.SetActive(true);
        }
    }

    public void Init(Action<int> callBack = null)
    {
        m_Callback = callBack;
    }

    public void SelectDefault(int tabIndex = 0)
    {
        OnTabSelected(tabIndex, true);
    }

    public void OnEnable()
    {
        if (tabDefault >= 0)
            OnTabSelected(tabDefault, true);
    }

    public void OnTabSelected(int tabSelect, bool showPanel = true)
    {
        if (tabDatas.Count < tabSelect)
        {
            return;
        }
        if(!tabDatas[tabSelect].tabPanel.CanShowPanel())
            return;
        for (int i = 0; i < tabDatas.Count; i++)
        {
            if (i == tabSelect)
            {
                if (_currentTabSelect == tabSelect)
                    return;
                _currentTabSelect = tabSelect;
                tabDatas[i]?.tabPanel.gameObject.SetActive(true);
                if (showPanel)
                    tabDatas[i]?.tabPanel?.OnShowScreen();
                tabDatas[i].tabButton.Selected();
            }
            else
            {
                tabDatas[i].tabPanel.gameObject.SetActive(false);
                tabDatas[i].tabButton.DeSelected();
            }
        }
    }
}

[System.Serializable]
public class TabData
{
    public TabButton tabButton;
    public PanelBase tabPanel;
}