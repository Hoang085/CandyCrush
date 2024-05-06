using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeCountDown : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private float timeLeft = 300f;
    private bool _isCount;

    private void Start()
    {
        _isCount = true;
    }
    private void Update()
    {
        if (_isCount)
        {
            if (timeLeft > 0)
            {
                timeLeft -=Time.deltaTime;
            }
            else
            {
                _isCount = false;
            }
        }
    }

    private void UpdateTimer(float currentTime)
    {
        currentTime += 1;
        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        timeText.text = string.Format("{0:00} : {1:00}",minutes,seconds);
    }

}
