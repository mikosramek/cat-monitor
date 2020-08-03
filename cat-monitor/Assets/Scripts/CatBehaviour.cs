using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CatState
{
    standing,
    zooming,
    running,
    exhausted
}

public class CatBehaviour : MonoBehaviour
{
    private CatRaceData myRace;
    public CatState currentState;
    private int raceSegmentIndex;
    private bool racing;
    private Vector3 targetPosition;
    private int internalSegmentIndex;
    private int lapCount;

    private Vector3 startPos;
    private float startTime;
    private float timeToMove = 1f;

    public Animator _animator;
    private SpriteRenderer _sprite;
    public Animator _sfxAnimator;

    private float scale = 0.25f;
    private float CAT_TARGET_OFFSET = 2.5f;

    private int catIndex;

    public CameraBehaviour myCam;

    private float raceCompletionPercentage;
    private float raceStartTime;

    private int LAPS_TO_WIN = 3;
    private float TIME_BETWEEN_SEGMENTS = 0.5f;

    public bool debugCat = false;

    private void Awake()
    {
        _sprite = GetComponentInChildren<SpriteRenderer>();
        scale = transform.localScale.z;
    }

    void Start()
    {
        currentState = CatState.standing;
        raceSegmentIndex = 0;
        internalSegmentIndex = 0;
        racing = false;
        initCat();
        startRace();
    }

    void Update()
    {
        if (!racing) return;
        float timeSinceStarted = Time.time - startTime;
        float percentageCompleted = timeSinceStarted / timeToMove;

        transform.position = Vector3.Lerp(startPos, targetPosition, percentageCompleted);
     
        raceCompletionPercentage = (Time.time - raceStartTime) / (myRace.overallTime * LAPS_TO_WIN);
        if(debugCat)
        {
            Debug.Log(Time.time - raceStartTime + " out of " + myRace.overallTime * LAPS_TO_WIN);
        }

        if (lapCount >= LAPS_TO_WIN)
        {
            checkForWin(percentageCompleted);
        }
        
        if (percentageCompleted >= 1.0f)
        {
            selectNextTarget();
        }
    }

    public void setRace(CatRaceData newRace)
    {
        myRace = newRace;
        myRace.overallTime += myRace.getTotalInBetweenTime(TIME_BETWEEN_SEGMENTS);
    }

    public void setIndex(int catIndex)
    {
        this.catIndex = catIndex;
        _sprite.sortingOrder = 10 - catIndex;
        float offset = (CAT_TARGET_OFFSET * catIndex) - (CAT_TARGET_OFFSET * catIndex / 2);
        transform.position = new Vector3(transform.position.x, offset - 2.5f);
    }

    void startRace()
    {
        startTime = Time.time;
        startPos = transform.position;
        racing = true;
        setState(CatState.running);
        lapCount = 0;
        raceCompletionPercentage = 0;
        raceStartTime = Time.time;
    }

    // Call when race has been setup
    void initCat()
    {
        setState(CatState.standing);
        raceSegmentIndex = 0;
        internalSegmentIndex = 1;
        racing = false;
        setTarget(myRace.raceTrack[raceSegmentIndex].points[internalSegmentIndex]);
        timeToMove = myRace.getSegmentTime(raceSegmentIndex);
    }

    void setTarget(Transform newTarget)
    {
        float offset = (CAT_TARGET_OFFSET * catIndex) - (CAT_TARGET_OFFSET * catIndex / 2);
        if (raceSegmentIndex == 1 || raceSegmentIndex == 2 || raceSegmentIndex == 4 ||  raceSegmentIndex == 5)
        {
            targetPosition = new Vector3(newTarget.position.x + offset, newTarget.position.y);
        }
        else
        {
            targetPosition = new Vector3(newTarget.position.x, newTarget.position.y + offset);
        }
    }

    void setState(CatState newState)
    {
        currentState = newState;
        UpdateVisual();
    }

    // Call when point has been reached
    void selectNextTarget()
    {
        // Add one to internal section transform index
        internalSegmentIndex += 1;

        // If we've gone past the current segment's point count -> move onto next segment
        if (internalSegmentIndex >= myRace.raceTrack[raceSegmentIndex].points.Length)
        {
            raceSegmentIndex += 1;
            internalSegmentIndex = 0;
        }

        //If we've gone past the segment count, return to the first segment
        if (raceSegmentIndex >= myRace.raceTrack.Length)
        {
            raceSegmentIndex = 0;
            internalSegmentIndex = 0;
            lapCount += 1;
        }

        //Debug.Log("Total Segments: " + myRace.raceTrack.Length + " | Current Segment: " + raceSegmentIndex + " | Internal Point: " + internalSegmentIndex);

        setTarget(myRace.raceTrack[raceSegmentIndex].points[internalSegmentIndex]);
        startTime = Time.time;
        startPos = transform.position;
        if (internalSegmentIndex == 1)
        {
            timeToMove = myRace.getSegmentTime(raceSegmentIndex);
            if (myCam) myCam.UpdatePoint(raceSegmentIndex);
        }
        else
        {
            timeToMove = TIME_BETWEEN_SEGMENTS;
        }
        setState(myRace.getSegmentState(raceSegmentIndex));
    }

    void checkForWin(float percentage)
    {
        if (raceSegmentIndex == 0 && internalSegmentIndex == 1 && percentage > 0.2f)
        {
            // Cat Won!
        }
    }

    // Update different animators based on current cat state
    void UpdateVisual()
    {
        if (raceSegmentIndex == 0 || raceSegmentIndex == 1 || raceSegmentIndex == 5)
        {
            gameObject.transform.localScale = new Vector3(-scale, scale, scale);
        }
        else
        {
            gameObject.transform.localScale = new Vector3(scale, scale, scale);
        }
        switch (currentState)
        {
            case CatState.running:
                _animator.SetTrigger("run");
                _sfxAnimator.SetTrigger("off");
                break;
            case CatState.standing:
                _animator.SetTrigger("stand");
                break;
            case CatState.zooming:
                _sfxAnimator.SetTrigger("zoom");
                break;
            case CatState.exhausted:
                break;
            default:
                break;
        }
    }

    public int getPosition()
    {
        return raceSegmentIndex;
    }
    public void giveCamera(CameraBehaviour newCam)
    {
        myCam = newCam;
        myCam.UpdatePoint(raceSegmentIndex);
    }
    public void removeCamera()
    {
        myCam = null;
    }
}
