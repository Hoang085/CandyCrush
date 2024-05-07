using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ticker : MonoBehaviour
{
    public static float tickTime = 0.1f;

    private float tickerTimer;

    public delegate void TickAction();
    public static event TickAction OnTickAction;

    // Update is called once per frame
    void Update()
    {
        tickerTimer += Time.deltaTime;

        if (tickerTimer >= tickTime)
        {
            tickerTimer = 0;
            TickEvent();
        }
    }
    private void TickEvent()
    {
        OnTickAction?.Invoke();
    }
}
