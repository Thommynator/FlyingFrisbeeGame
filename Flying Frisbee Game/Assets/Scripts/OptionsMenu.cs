using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
{

    public void Back()
    {
        int gameSceneIndex = 0;
        SceneManager.LoadScene(gameSceneIndex);
        Time.timeScale = 1f;
    }

    public void ResetScore()
    {
        PlayerPrefs.SetInt(ScoreCount.playerScorePrefKey, 0);
        PlayerPrefs.SetInt(ScoreCount.opponentScorePrefKey, 0);
    }

}
