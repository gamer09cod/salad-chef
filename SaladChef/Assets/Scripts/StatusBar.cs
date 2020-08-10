using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
    //Status Bar
    public Slider slider;

    public TextMeshProUGUI vegetableText;

    private float time;

    // Update is called once per frame
    void Update()
    {
        if (!this.enabled)
            return;

        if (time < 0)
            this.gameObject.SetActive(false);

        time -= Time.deltaTime;
        SetSliderValue(Mathf.Round(time));
    }

    public void Init(float time, string vegetable)
    {
        this.time = time;
        slider.maxValue = time;
        slider.value = time;

        vegetableText.text = vegetable;
    }

    void SetSliderValue(float value)
    {
        slider.value = value;
    }
}
