using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalStandingsUI : MonoBehaviour
{
    public RectTransform[] catIconPositions;
    private Vector3[] positions;
    private LeaderboardCatUI[] cats;

    Dictionary<CatBehaviour, LeaderboardCatUI> catUI = new Dictionary<CatBehaviour, LeaderboardCatUI>();

    private void Awake()
    {
        positions = new Vector3[catIconPositions.Length];
        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] = catIconPositions[i].localPosition ;
        }
        cats = gameObject.GetComponentsInChildren<LeaderboardCatUI>();

        for (int i = 0; i < cats.Length; i++)
        {
            catUI.Add(cats[i].getMyCat(), cats[i]);
        }
    }

    public void updateStandings(CatBehaviour[] standings)
    {
        for (int i = 0; i < cats.Length; i++)
        {
            LeaderboardCatUI tempCatUI = null;
            if (catUI.TryGetValue(standings[i], out tempCatUI))
            {
                tempCatUI.UpdatePosition(positions[i]);
            }
        }
    }
}
