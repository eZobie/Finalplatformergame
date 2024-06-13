using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionScreenManager : MonoBehaviour
{
    public GameObject optionScreen;
    public GameObject confirmationDialog; // Assign this in the Inspector

    public void RestartLevel()
    {
        Time.timeScale = 1f; // Resume the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload current scene
    }

    public void NextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            Time.timeScale = 1f; // Resume the game
            SceneManager.LoadScene(nextSceneIndex); // Load next scene
        }
        else
        {
            Debug.Log("No more levels to load.");
        }
    }

    public void EndGame()
    {
        optionScreen.SetActive(false); // Hide the option screen
        confirmationDialog.SetActive(true); // Show the confirmation dialog
    }

    public void ConfirmEndGame()
    {
        Time.timeScale = 1f; // Resume the game
        Application.Quit(); // Quit the game
    }

    public void CancelEndGame()
    {
        confirmationDialog.SetActive(false); // Hide the confirmation dialog
        optionScreen.SetActive(true); // Show the option screen again
    }
}