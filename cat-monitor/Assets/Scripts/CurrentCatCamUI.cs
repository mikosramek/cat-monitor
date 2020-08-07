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
    public RectTransform timeBar;

    public void UpdateUI(CatPortrait cat)
    {
        portaitUI.sprite = cat.PortraitSprite;
        nameUI.text = cat.Name;
        quoteUI.text = cat.Quote;
    }
    public void UpdateTimeUntilSwap(float percent)
    {
        timeBar.localScale = new Vector3(1 - percent, 1, 1);
    }
}
