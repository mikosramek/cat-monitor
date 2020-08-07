using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{

    private CatBehaviour[] cats;

    public Transform[] cameraPoints;
    private int catFollowIndex;

    private CatBehaviour fastestCat;
    private CatBehaviour slowestCat;

    public CurrentCatCamUI _cccUI;

    private float startingTime;
    public float catAirTime;

    private void Update()
    {
        float timePercentage = (Time.time - startingTime) / catAirTime;
        _cccUI.UpdateTimeUntilSwap(timePercentage);
        if (Time.time - startingTime > catAirTime)
        {
            SwapCat();
            startingTime = Time.time;
        }
    }

    public void UpdatePoint(int pointIndex)
    {
        Vector3 newPosition = cameraPoints[pointIndex].position;
        transform.position = new Vector3(newPosition.x, newPosition.y, -10);
    }

    public void SwapCat()
    {
        cats[catFollowIndex].removeCamera();
        catFollowIndex += 1;
        if (catFollowIndex >= cats.Length) catFollowIndex = 0;
        cats[catFollowIndex].giveCamera(this);
        UpdateCatUI();
        UpdatePoint(cats[catFollowIndex].getPosition());
    }

    public void UpdateCatUI()
    {
        _cccUI.UpdateUI(cats[catFollowIndex].GetComponent<CatPortrait>());
    }

    public void SetupCamera(CatBehaviour[] cats, CatBehaviour fastCat, CatBehaviour slowCat)
    {
        catFollowIndex = 0;

        this.cats = cats;
        this.cats[catFollowIndex].giveCamera(this);
        
        fastestCat = fastCat;
        slowestCat = slowCat;

        UpdateCatUI();
    }
}
