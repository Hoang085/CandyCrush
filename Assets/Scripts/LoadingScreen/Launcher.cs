using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{

    bool adsInitilized;
    bool analyticsInitilized;
    bool LaunchConditon => adsInitilized && analyticsInitilized;

    private void Awake()
    {
        adsInitilized = true;
        analyticsInitilized = true;
    }

    private void Start()
    {
        LoadingScreen.Instance.LoadScene("StartScene", () => LaunchConditon);
    }

}
