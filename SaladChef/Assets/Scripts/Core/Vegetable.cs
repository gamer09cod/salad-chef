using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Vegetable
{
    public string name;
    public int weight;
    public float chopTime;

    public int score;

    public Vegetable(string s, int w, float t, int score)
    {
        this.name = s;
        this.weight = w;
        this.chopTime = t;
        this.score = score;
    }

    public Vegetable(Vegetable v)
    {
        this.name = v.name;
        this.weight = v.weight;
        this.chopTime = v.chopTime;
        this.score = v.score;
    }
}
