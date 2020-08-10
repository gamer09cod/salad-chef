using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    public float GetCurrentTick { get { return tick; } }

    public static Action OnPlayerTimeOver;

    private float tick;

    IEnumerator TimeTicker()
    {
        while (tick > 0)
        {
            tick -= Time.deltaTime;
            timerText.text = Mathf.Round(tick).ToString();
            yield return null;
        }

        OnPlayerTimeOver?.Invoke();
        yield return null;
    }

    public void StartTimer(float t)
    {
        tick += t;
        StartCoroutine(TimeTicker());
    }
}
