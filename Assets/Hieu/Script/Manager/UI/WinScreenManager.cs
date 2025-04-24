using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject winScreenUI;

    public void ShowWinScreen()
    {
        Time.timeScale = 0f; // Optional: pause game
        winScreenUI.SetActive(true);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextLevel()
    {
        Time.timeScale = 1f;
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextIndex < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(nextIndex);
        else
            Debug.Log("No more levels.");
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu"); // Replace with your menu scene name
    }
}
