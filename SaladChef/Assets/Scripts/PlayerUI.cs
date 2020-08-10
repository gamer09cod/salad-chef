using TMPro;
using UnityEngine;

/// <summary>
/// Class to manage player UI related items.
/// </summary>
public class PlayerUI : MonoBehaviour
{
    public TextMeshProUGUI score;
    public TextMeshProUGUI time;
    public TextMeshProUGUI bag;
    public TextMeshProUGUI choppedVegetables;
    public TextMeshProUGUI vegetableOnPlate;

    //Status Bar
    public StatusBar statusBar;

    public void UpdateScore(int scr)
    {
        score.text = scr.ToString();
    }

    public void ShowStatusBar(float t, string name)
    {
        statusBar.Init(t, name);
        statusBar.gameObject.SetActive(true);
    }

    public void HideStatusBar()
    {
        statusBar.gameObject.SetActive(false);
    }

    public void UpdateBag(string items)
    {
        bag.text = items;
    }

    public void UpdateChoppedVegetables(string items)
    {
        choppedVegetables.text = items;
    }

    public void UpdateVegetableOnPlate(string items)
    {
        vegetableOnPlate.text = items;
    }
}
