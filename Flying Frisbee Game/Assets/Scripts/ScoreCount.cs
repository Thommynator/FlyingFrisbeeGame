using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreCount : MonoBehaviour
{

    public int playerScore;

    public GameObject playerScoredNotificationView;
    public int opponentScore;
    public GameObject opponentScoredNotificationView;
    private GameObject scoreTextObject;

    void Start()
    {
        GameEvents.current.onPlayerScoredPoint += ShowPlayerScoredNotification;
        GameEvents.current.onOpponentScoredPoint += ShowOpponentScoredNotification;

        scoreTextObject = GameObject.Find("ScoreText");

        GameEvents.current.onPlayerScoredPoint += () => { playerScore += 1; };
        GameEvents.current.onOpponentScoredPoint += () => { opponentScore += 1; };

        playerScore = PlayerPrefs.GetInt("PlayerScore", 0);
        opponentScore = PlayerPrefs.GetInt("OpponentScore", 0);
    }

    void Update()
    {
        ShowScore();
    }

    private void ShowPlayerScoredNotification()
    {
        playerScoredNotificationView.SetActive(true);
        float restartDelay = 2f;
        Invoke("RestartScene", restartDelay);
    }

    private void ShowOpponentScoredNotification()
    {
        opponentScoredNotificationView.SetActive(true);
        float restartDelay = 2f;
        Invoke("RestartScene", restartDelay);
    }


    private void ShowScore()
    {
        string scoreText = $"{playerScore} : {opponentScore}";
        scoreTextObject.GetComponent<TextMeshProUGUI>().SetText(scoreText);
    }

    private void RestartScene()
    {
        PlayerPrefs.SetInt("PlayerScore", playerScore);
        PlayerPrefs.SetInt("OpponentScore", opponentScore);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
