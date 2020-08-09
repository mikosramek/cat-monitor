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
    public Image timeBar;

    public void UpdateUI(CatPortrait cat, int lapCount, int lapTotal)
    {
        portaitUI.sprite = cat.PortraitSprite;
        nameUI.text = cat.Name;
        quoteUI.text = cat.Quote;
        lapsText.text = (lapCount + 1) + "/" + lapTotal;
    }
    public void UpdateTimeUntilSwap(float percent)
    {
        timeBar.fillAmount = 1 - percent;
    }
}
