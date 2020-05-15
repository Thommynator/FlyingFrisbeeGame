using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreCount : MonoBehaviour
{

    public int playerScore;
    public int opponentScore;
    public GameObject scoreObject;

    void Start()
    {
        GameEvents.current.onPlayerScoredPoint += () => { playerScore += 1; };
        GameEvents.current.onOpponentScoredPoint += () => { opponentScore += 1; };

        playerScore = 0;
        opponentScore = 0;
    }

    void Update()
    {
        ShowScore();
    }

    private void ShowScore()
    {
        string scoreText = $"{playerScore} : {opponentScore}";
        scoreObject.GetComponent<TextMeshProUGUI>().SetText(scoreText);
    }

}
