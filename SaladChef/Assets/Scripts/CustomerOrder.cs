using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CustomerOrder
{  
    public int requiredSum;//Total sum required to complete this order.

    public string sequence;

    public float time;//Time(in seconds) a customer waits before leaving.

    public CustomerBonus bonus;//Bonus given to the player on successful completion of this order.

    public float speedMultiplier;
    public float timeIncrement;
    public int score;
}
