using System.Collections;
using System.Collections.Generic;
using H2910.Common.Singleton;
using H2910.UI.Popups;
using UnityEngine;

public class TutorialManager : ManualSingletonMono<TutorialManager>
{
    public void CheckResumeGame()
    {
        PopupManager.Instance.CheckResumeGame();
        return;
    }
}
