using UnityEngine;

public class MoveMover : MonoBehaviour
{
    private GameObject player; // Store reference to the player GameObject

    // Start is called before the first frame update
    void Start()
    {
        // Find the "Player" GameObject and store a reference to it
        player = GameObject.Find("Player");

        if (player == null)
        {
            Debug.LogError("Player GameObject not found!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player GameObject reference is valid before using it
        if (player != null)
        {
            // Do whatever you need with the player GameObject here
            // For example, you can move the player or perform other actions
        }
        else
        {
            // Handle the case when the player GameObject is not found
            Debug.LogWarning("Player GameObject reference is null!");
        }
    }
}