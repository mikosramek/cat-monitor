using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CurrentCatCamUI : MonoBehaviour
{
    public Image portaitUI;
    public TMPro.TextMeshProUGUI nameUI;
    public TMPro.TextMeshProUGUI quoteUI;

    public void UpdateUI(CatPortrait cat)
    {
        portaitUI.sprite = cat.PortraitSprite;
        nameUI.text = cat.Name;
        quoteUI.text = cat.Quote;
    }
}
