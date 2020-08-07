using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CatUI : MonoBehaviour
{
    public TextMeshProUGUI placementText;
    public RectTransform indicatorRect;

    public AnimationCurve movementCurve;

    private float indicatorStartY;
    private RectTransform placementTextRect;

    private void Awake()
    {
        placementTextRect = placementText.GetComponent<RectTransform>();
    }
    private void Start()
    {
        indicatorStartY = indicatorRect.localPosition.y;
    }

    private void Update()
    {
        indicatorRect.localPosition = new Vector3(indicatorRect.localPosition.x, indicatorStartY + movementCurve.Evaluate(Time.time % 1) * 25);
    }

    public void UpdatePlacementText(int position)
    {
        switch (position)
        {
            case 1:
                placementText.text = "1st";
                break;
            case 2:
                placementText.text = "2nd";
                break;
            case 3:
                placementText.text = "3rd";
                break;
            case 4:
                placementText.text = "4th";
                break;
            case 5:
                placementText.text = "5th";
                break;
            case 6:
                placementText.text = "6th";
                break;
            default:
                placementText.text = "???";
                break;
        }
    }
    public void flipText(float textScale)
    {
        placementTextRect.localScale = new Vector3(textScale, 1, 1);
    }
}
