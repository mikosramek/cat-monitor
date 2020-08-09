using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardCatUI : MonoBehaviour
{
    private Sprite zoom, exhausted;
    public Image icon;
    public CatBehaviour myCat;
    private RectTransform myRect;

    private void Awake()
    {
        LeaderboardUI _lbUI = GameObject.FindObjectOfType<LeaderboardUI>();
        myRect = GetComponent<RectTransform>();
        zoom = _lbUI.zoom;
        exhausted = _lbUI.exhausted;
    }

    public void setState(CatState newState)
    {
        switch (newState)
        {
            case CatState.exhausted:
                icon.enabled = true;
                icon.sprite = exhausted;
                break;
            case CatState.zooming:
                icon.enabled = true;
                icon.sprite = zoom;
                break;
            default:
                icon.enabled = false;
                break;
        }
    }

    public void setMyCat(CatBehaviour newCat)
    {
        myCat = newCat;
    }
    public CatBehaviour getMyCat()
    {
        return myCat;
    }

    public void UpdatePosition(Vector3 position)
    {
        myRect.localPosition = position;
    }
}
