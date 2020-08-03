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

    private int catFollowCount = 4;
    private int currentFollowCount;

    public CurrentCatCamUI _cccUI;

    public void UpdatePoint(int pointIndex)
    {
        Vector3 newPosition = cameraPoints[pointIndex].position;
        transform.position = new Vector3(newPosition.x, newPosition.y, -10);
        currentFollowCount += 1;
        if (currentFollowCount >= catFollowCount)
        {
            currentFollowCount = 0;
            SwapCat();
        }
    }

    public void SwapCat()
    {
        cats[catFollowIndex].removeCamera();
        catFollowIndex += 1;
        if (catFollowIndex >= cats.Length) catFollowIndex = 0;
        cats[catFollowIndex].giveCamera(this);
        UpdateCatUI();
    }

    public void UpdateCatUI()
    {
        _cccUI.UpdateUI(cats[catFollowIndex].GetComponent<CatPortrait>());
    }

    public void SetupCamera(CatBehaviour[] cats, CatBehaviour fastCat, CatBehaviour slowCat)
    {
        catFollowIndex = 0;
        currentFollowCount = 0;

        this.cats = cats;
        this.cats[catFollowIndex].giveCamera(this);
        
        fastestCat = fastCat;
        slowestCat = slowCat;

        UpdateCatUI();
    }
}
