using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CurrentCatCamUI : MonoBehaviour
{
    public Image portaitUI;
    public TextMeshProUGUI nameUI;
    public TextMeshProUGUI quoteUI;
    public TextMeshProUGUI lapsText;
    public RectTransform timeBar;

    public void UpdateUI(CatPortrait cat, int lapCount)
    {
        portaitUI.sprite = cat.PortraitSprite;
        nameUI.text = cat.Name;
        quoteUI.text = cat.Quote;
        lapsText.text = (lapCount + 1) + "/3 laps";
    }
    public void UpdateTimeUntilSwap(float percent)
    {
        timeBar.localScale = new Vector3(1 - percent, 1, 1);
    }
}
