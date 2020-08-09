using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
    public CatRaceData[] catRaceDatas;
    public RaceTrackPoints[] raceTrack;
    public CatBehaviour[] cats;

    public CameraBehaviour _cb;

    public float raceStartingX;

    public CatBehaviour[] catStandings;

    public LeaderboardUI leaderboardUI;

    private bool isRaceOn;

    public float baseSpeedModifier;

    public float raceEndTimeMod;
    private float fixedDeltaTime;
    public GameObject confetti;

    public int startingTicketAmount;
    private int currentTicketAmount;
    private CatBehaviour catBetOn;


    public GameObject ui_mainMenu, ui_betting, ui_race, ui_standings, ui_pauseMenu;

    public CountdownUI ui_countdown;
    public FinalStandingsUI ui_finalStandings;

    private void Awake()
    {
        setupCats();
        ui_mainMenu.SetActive(true);
        ui_betting.SetActive(false);
        ui_race.SetActive(false);
        ui_standings.SetActive(false);
        ui_pauseMenu.SetActive(false);
        ui_countdown.gameObject.SetActive(false);
    }

    public void setupCats()
    {
        this.fixedDeltaTime = Time.fixedDeltaTime;
        if (cats.Length <= 0)
        {
            cats = GameObject.FindObjectsOfType<CatBehaviour>();
        }
        CreateRaceData();
        catStandings = new CatBehaviour[cats.Length];
        for (int i = 0; i < cats.Length; i++)
        {
            catStandings[i] = cats[i];
        }
    }

    public void CreateRaceData()
    {
        catRaceDatas = new CatRaceData[cats.Length];
        reshuffleCats(cats);
        for (int i = 0; i < catRaceDatas.Length; i++)
        {
            catRaceDatas[i] = new CatRaceData();
            catRaceDatas[i].createData((i + 1) * baseSpeedModifier, raceTrack);
            cats[i].setRace(catRaceDatas[i], this);
            cats[i]._catUI.gameObject.SetActive(false);
        }
        _cb.SetupCamera(cats, cats[0], cats[cats.Length - 1]);
        reshuffleCats(cats);
        for (int i = 0; i < catRaceDatas.Length; i++)
        {
            cats[i].setIndex(i, raceStartingX);
            cats[i].resetCat();
        }
    }

    // https://forum.unity.com/threads/randomize-array-in-c.86871/
    void reshuffleCats(CatBehaviour[] cats)
    {
        // Knuth shuffle algorithm :: courtesy of Wikipedia :)
        for (int t = 0; t < cats.Length; t++)
        {
            CatBehaviour tmp = cats[t];
            int r = UnityEngine.Random.Range(t, cats.Length);
            cats[t] = cats[r];
            cats[r] = tmp;
        }
    }

    private void Update()
    {
        if (isRaceOn)
        {
            // sort out positions
            CatBehaviour[] newStandings = new CatBehaviour[catStandings.Length];
            newStandings = catStandings.OrderBy(cat => cat.getPercentageDone()).ToArray();
            newStandings = newStandings.Reverse().ToArray();

            for (int i = 0; i < newStandings.Length; i++)
            {
                if (catStandings[i] != newStandings[i])
                {
                    // Standings have changed
                    catStandings = (CatBehaviour[])newStandings.Clone();
                    updateCatStandings();
                }
            }
        }
    }

    void updateCatStandings()
    {

        // send out placement to each cat
        for (int i = 0; i < cats.Length; i++)
        {
            cats[i].setPositionInRace(System.Array.IndexOf(catStandings, cats[i]));
        }
        // each cat gives it to their UI
        // send out placement to UI
        leaderboardUI.updateStandings(catStandings);
    }
    public void handlePlayButton()
    {
        // hide main menu
        // show betting ui
        ui_mainMenu.SetActive(false);
        ui_betting.SetActive(true);
    }
    public void handlePlayAgainButton()
    {
        newRace();
    }
    public void handleResumeButton()
    {
        ui_pauseMenu.SetActive(false);
        // continue race
    }
    public void PlaceBet(CatBehaviour cat)
    {
        catBetOn = cat;
        // Hide bet UI
        // Show Race Countdown UI
        // Show Race UI
        // start countdown
        ui_betting.SetActive(false);
        ui_race.SetActive(true);
        ui_countdown.gameObject.SetActive(true);
        ui_countdown.startTimer();
    }
    public void startRace()
    {
        // Hide Race Countdown UI
        ui_countdown.gameObject.SetActive(false);
        ui_countdown.resetTimer();
        isRaceOn = true;
        _cb.startRace();
        for (int i = 0; i < cats.Length; i++)
        {
            cats[i].startRace();
        }
    }
    public void newRace()
    {
        confetti.gameObject.SetActive(false);
        // Show bet UI
        // Hide Race UI
        ui_betting.SetActive(true);
        ui_race.SetActive(false);
        ui_standings.SetActive(false);
        confetti.SetActive(false);
        setupCats();
    }
    public void endRace(CatBehaviour winningCat)
    {
        if (isRaceOn)
        {
            isRaceOn = false;
            _cb.endRace();

            confetti.SetActive(true);
            Time.timeScale = raceEndTimeMod;
            Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;

            // show standings UI after 1 seconds
            Invoke("showStandings", 1f);
        }
    }
    private void showStandings()
    {
        Time.timeScale = 1;
        Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
        ui_finalStandings.updateStandings(catStandings);
        ui_standings.SetActive(true);
    }
    public void quitGame()
    {
        Application.Quit();
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
    public float getTotalInBetweenTime(float inBetweenTime)
    {
        return inBetweenTime * raceSegments.Length;
    }
    public void createData(float speedModifier, RaceTrackPoints[] raceTrack)
    {
        raceSegments = new RaceSegment[6];
        overallTime = 0;
        this.raceTrack = raceTrack;
        for (int i = 0; i < raceSegments.Length; i++)
        {
            float speed = Mathf.Clamp(UnityEngine.Random.Range(speedModifier - 2.5f, speedModifier + 2.5f) * 0.5f, 2, 100);
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
        //overallTime += raceSegments.Length;
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