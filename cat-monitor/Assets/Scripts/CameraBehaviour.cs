using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{

    private CatBehaviour[] cats;
    private RaceManager _rm;

    public Transform[] cameraPoints;
    public int catFollowIndex;

    private void Awake()
    {
        _rm = GameObject.FindObjectOfType<RaceManager>();
    }

    void Start()
    {
        cats = _rm.cats;
        catFollowIndex = 0;

        Debug.Log(cats);
        cats[catFollowIndex].giveCamera(this);
    }

    public void UpdatePoint(int pointIndex)
    {
        Vector3 newPosition = cameraPoints[pointIndex].position;
        transform.position = new Vector3(newPosition.x, newPosition.y, -10);
    }

    public void SwapCat()
    {
        
    }
}
