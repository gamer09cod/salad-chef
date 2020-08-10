using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OrderUIItem : MonoBehaviour
{
    public TextMeshProUGUI sequence;
    public TextMeshProUGUI timeText;

    public float startTime;

    public Action<OrderUIItem> onOrderProcessed;

    public int scoreID;//Used to complete this order.
    
    // Update is called once per frame
    void Update()
    {
        if (startTime < 0)
            onOrderProcessed?.Invoke(this);

        startTime -= Time.deltaTime;
        timeText.text = Mathf.Round(startTime).ToString();
    }
}
