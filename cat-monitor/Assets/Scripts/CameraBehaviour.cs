using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{

    private CatBehaviour[] cats;

    public Transform[] cameraPoints;
    private int catFollowIndex;

    private int fastestCatIndex;

    public CurrentCatCamUI _cccUI;

    private float startingTime;
    public float catAirTime;

    private bool isActive;

    private void Update()
    {
        if (isActive)
        {
            float timePercentage = (Time.time - startingTime) / catAirTime;
            _cccUI.UpdateTimeUntilSwap(timePercentage);
            if (Time.time - startingTime > catAirTime)
            {
                SwapCat();
                startingTime = Time.time;
            }
        }
    }

    public void UpdatePoint(int pointIndex)
    {
        if (isActive)
        {
            Vector3 newPosition = cameraPoints[pointIndex].position;
            transform.position = new Vector3(newPosition.x, newPosition.y, -10);
        }
    }

    public void SwapCat()
    {
        cats[catFollowIndex].removeCamera();
        int tempFollowIndex = catFollowIndex;
        while (catFollowIndex == tempFollowIndex)
        {
            catFollowIndex = Random.Range(0, cats.Length);
        }
        cats[catFollowIndex].giveCamera(this);
        UpdateCatUI();
        UpdatePoint(cats[catFollowIndex].getPosition());
    }

    public void UpdateCatUI()
    {
        CatBehaviour updateCat = cats[catFollowIndex];
        _cccUI.UpdateUI(updateCat.GetComponent<CatPortrait>(), updateCat.getLapCount(), updateCat.getTotalLapCount());
    }

    public void SetupCamera(CatBehaviour[] cats, CatBehaviour fastCat, CatBehaviour slowCat)
    {
        catFollowIndex = Random.Range(0, cats.Length);

        this.cats = (CatBehaviour[]) cats.Clone();
        this.cats[catFollowIndex].giveCamera(this);

        fastestCatIndex = System.Array.IndexOf(cats, fastCat);

        UpdateCatUI();
    }

    public void startRace()
    {
        isActive = true;
    }
    public void endRace() {
        UpdatePoint(0);
        catFollowIndex = fastestCatIndex;
        UpdateCatUI();
        isActive = false;
    }
}
