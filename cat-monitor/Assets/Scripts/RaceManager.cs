using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
    public CatRaceData[] catRaceDatas;
    void Start()
    {
        CreateRaceData();
    }

    void Update()
    {
        
    }

    void CreateRaceData()
    {
        catRaceDatas = new CatRaceData[5];
        for (int i = 0; i < catRaceDatas.Length; i++)
        {
            catRaceDatas[i] = new CatRaceData();
            catRaceDatas[i].createData((i + 1) * 5);
        }
    }
}

public enum RaceEvent
{
    zoom,
    nothing,
    tripped,
    exhausted
}

[System.Serializable]
public struct CatRaceData {
    [SerializeField]
    RaceSegment[] raceSegments;
    public float overallTime;
    public void createData(float speedModifier)
    {
        raceSegments = new RaceSegment[6];
        overallTime = 0;
        for (int i = 0; i < raceSegments.Length; i++)
        {
            float speed = Mathf.Clamp(Random.Range(speedModifier - 10, speedModifier + 10), 4, 100);
            RaceEvent ev = RaceEvent.nothing;
            float avgTime = overallTime / i;
            if (i == 0)
            {
                ev = RaceEvent.nothing;
            }
            else if (speed < avgTime)
            {
                ev = RaceEvent.zoom;
            }
            else if (speed > avgTime * 1.25f)
            {
                ev = RaceEvent.exhausted;
            }
            else
            {
                ev = RaceEvent.nothing;
            }
            raceSegments[i].init(speed, ev);
            overallTime += speed;
        }
    }
}

[System.Serializable]
public struct RaceSegment
{
    [SerializeField]
    public RaceEvent myEvent;
    public float time;

    public void init(float speedModifier, RaceEvent ev)
    {
        myEvent = ev;//(RaceEvent)Random.Range(0, 3);
        time = speedModifier;
    }
}