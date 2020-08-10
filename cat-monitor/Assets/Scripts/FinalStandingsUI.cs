using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FinalStandingsUI : MonoBehaviour
{
    public RectTransform[] catIconPositions;
    private Vector3[] positions;
    private LeaderboardCatUI[] cats;

    Dictionary<CatBehaviour, LeaderboardCatUI> catUI = new Dictionary<CatBehaviour, LeaderboardCatUI>();

    public TextMeshProUGUI amountChanged;
    public GameObject betText;

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

    public void updateStandings(CatBehaviour[] standings, int betPlacement, string ticketChange)
    {
        for (int i = 0; i < cats.Length; i++)
        {
            LeaderboardCatUI tempCatUI = null;
            if (catUI.TryGetValue(standings[i], out tempCatUI))
            {
                tempCatUI.UpdatePosition(positions[i]);
            }
        }
        betText.GetComponent<RectTransform>().localPosition = new Vector3(betText.GetComponent<RectTransform>().localPosition.x, positions[betPlacement].y);
        amountChanged.text = ticketChange;
    }
}
