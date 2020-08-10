using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TicketUI : MonoBehaviour
{
    private int ticketAmount;
    private int ticketTarget;

    public TextMeshProUGUI tickets;

    public void updateTicketAmount(int amount)
    {
        ticketTarget = amount;
    }
    public void setTicketAmount(int amount)
    {
        ticketTarget = amount;
        ticketAmount = amount;
        tickets.text = ticketAmount.ToString();
    }

    private void Update()
    {
        if (ticketAmount != ticketTarget)
        {
            ticketAmount = Mathf.CeilToInt(Mathf.MoveTowards(ticketAmount, ticketTarget, 1));
            tickets.text = ticketAmount.ToString();
        }
    }
}
