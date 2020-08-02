using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
    public CatRaceData[] catRaceDatas;
    public RaceTrackPoints[] raceTrack;
    public CatBehaviour[] cats;

    private void Awake()
    {
        if (cats.Length <= 0)
        {
            cats = GameObject.FindObjectsOfType<CatBehaviour>();
        }
        CreateRaceData();
    }

    void CreateRaceData()
    {
        catRaceDatas = new CatRaceData[cats.Length];
        for (int i = 0; i < catRaceDatas.Length; i++)
        {
            catRaceDatas[i] = new CatRaceData();
            catRaceDatas[i].createData((i + 1) * 2, raceTrack);
            cats[i].setRace(catRaceDatas[i]);
        }
        reshuffleCats(cats);
        for (int i = 0; i < catRaceDatas.Length; i++)
        {
            cats[i].setIndex(i);
        }
    }

    // https://forum.unity.com/threads/randomize-array-in-c.86871/
    void reshuffleCats(CatBehaviour[] cats)
    {
        // Knuth shuffle algorithm :: courtesy of Wikipedia :)
        for (int t = 0; t < cats.Length; t++)
        {
            CatBehaviour tmp = cats[t];
            int r = Random.Range(t, cats.Length);
            cats[t] = cats[r];
            cats[r] = tmp;
        }
    }
}

[System.Serializable]
public struct RaceTrackPoints
{
    public Transform[] points;
}

[System.Serializable]
public struct CatRaceData {
    [SerializeField]
    RaceSegment[] raceSegments;
    public float overallTime;
    public RaceTrackPoints[] raceTrack;
    public float getSegmentTime(int segmentIndex)
    {
        return raceSegments[segmentIndex].time;
    }
    public CatState getSegmentState(int segmentIndex)
    {
        return raceSegments[segmentIndex].segmentState;
    }
    public void createData(float speedModifier, RaceTrackPoints[] raceTrack)
    {
        raceSegments = new RaceSegment[6];
        overallTime = 0;
        this.raceTrack = raceTrack;
        for (int i = 0; i < raceSegments.Length; i++)
        {
            float speed = Mathf.Clamp(Random.Range(speedModifier - 2.5f, speedModifier + 2.5f), 3, 100) / 2;
            CatState ev = CatState.running;
            float avgTime = overallTime / i;
            if (i == 0)
            {
                ev = CatState.running;
            }
            else if (speed < avgTime)
            {
                ev = CatState.zooming;
            }
            else if (speed > avgTime * 1.25f)
            {
                ev = CatState.exhausted;
            }
            else
            {
                ev = CatState.running;
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
    public CatState segmentState;
    public float time;

    public void init(float speedModifier, CatState ev)
    {
        segmentState = ev;//(RaceEvent)Random.Range(0, 3);
        time = speedModifier;
    }
}