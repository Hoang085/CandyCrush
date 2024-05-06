using System;
using System.Collections;
using System.Collections.Generic;
using H2910.Common.Singleton;
using UnityEngine;

public class GameConfig : ManualSingletonMono<GameConfig>
{
    /*[SerializeField] ShopConfig coinExchange;
    [SerializeField] PackConfigData packConfig;
    [SerializeField] DailyRewardConfig dailyRewardConfig;
    [SerializeField] DailyGiftData dailyGiftConfig;
    [SerializeField] TipAndTrickConfig tipAndTrickConfig;
    [SerializeField] KillBossDropItem killBossDropItem;*/
    [SerializeField] int pixelsPerUnit;
    [SerializeField] float gameSpeed;

    private bool isPause;
    public bool IsPause => isPause;

    public event Action<bool> OnPause;
    
    /*public ShopConfig CoinExchange => coinExchange;
    public PackConfigData PackConfig => packConfig;
    public DailyGiftData DailyGiftConfig => dailyGiftConfig;
    public DailyRewardConfig DailyRewardConfig => dailyRewardConfig;
    public TipAndTrickConfig TipAndTrickConfig => tipAndTrickConfig;*/
    public int PixelsPerUnit => pixelsPerUnit;
    public float GameSpeed => gameSpeed;

    public int frameRate = 60;

    private void Start()
    { 
        Application.targetFrameRate = frameRate;
    }

    private void OnApplicationPause(bool pause)
    {
        if (!pause && TutorialManager.Instance!=null)
            TutorialManager.Instance.CheckResumeGame();
    }

    public void PauseGame(bool pause)
    {
        if (isPause == pause)
            return;

        OnPause?.Invoke(pause);
        isPause = pause;
        
        if (pause)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }
}
