using System;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool isGamePaused = false;

    public GameObject pauseMenuUI;

    public GameObject minimap;

    public GameObject score;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                ContinueGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
        pauseMenuUI.SetActive(true);
        minimap.SetActive(false);
        score.SetActive(false);
        isGamePaused = true;
    }

    public void ContinueGame()
    {
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
        minimap.SetActive(true);
        score.SetActive(true);
        isGamePaused = false;
    }

    public void LoadMenu()
    {
        Debug.Log("LOAD MENU");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
