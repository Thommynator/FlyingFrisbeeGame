using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCount : MonoBehaviour
{

    public int playerScore;
    public int opponentScore;

    void Start()
    {
        GameEvents.current.onCatchInEndzone += () => { playerScore += 1; };
        GameEvents.current.onPlayerLost += () => { opponentScore += 1; };

        playerScore = 0;
        opponentScore = 0;
    }

    void Update()
    {
        Debug.Log("Player Score: " + playerScore);
        Debug.Log("Opponent Score: " + opponentScore);
    }

}
