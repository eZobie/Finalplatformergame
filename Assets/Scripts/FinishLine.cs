using UnityEngine;

public class FinishLine : MonoBehaviour
{
    public GameObject optionScreen; // Assign the OptionScreen panel in the Inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Show the option screen
            optionScreen.SetActive(true);
            
            // You might also want to pause the game
            Time.timeScale = 0f; // This pauses the game
            
            // Finish the game
        }
    }
}