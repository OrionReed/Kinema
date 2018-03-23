using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _TimeControl : MonoBehaviour
{
    /// Called when speed is changed by _TimeControl.
    public event Action OnSpeedChange = delegate { };
    /// Returns the int that time is being divided by.
    public int timeFraction { get { return timeDivisions[scaleIndex]; } private set { } }

    private int scaleIndex = 0;
    private int[] timeDivisions = { 1, 2, 4, 8, 16, 32, 64, 100, 500, 1000 };

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void Start()
    {
        _Input.OnTimeSpeedUp += SpeedUpTime;
        _Input.OnTimeSpeedDown += SlowDownTime;
    }

    private void OnDisable()
    {
        _Input.OnTimeSpeedUp -= SpeedUpTime;
        _Input.OnTimeSpeedDown -= SlowDownTime;
    }
    private void SlowDownTime()
    {
        if (scaleIndex < timeDivisions.Length - 1)
            scaleIndex++;
        UpdateTimeScale();
        OnSpeedChange();
    }
    private void SpeedUpTime()
    {
        if (scaleIndex > 0)
            scaleIndex--;
        UpdateTimeScale();
        OnSpeedChange();
    }
    private void UpdateTimeScale()
    {
        Time.timeScale = 1f / timeDivisions[scaleIndex];
        Time.fixedDeltaTime = 0.02F * Time.timeScale;
    }
}
