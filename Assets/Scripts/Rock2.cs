using UnityEngine;

public class Rock2 : MonoBehaviour
{
    public float speedIncrease = 2.0f;

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name);
        SkierController player = collision.gameObject.GetComponent<SkierController>();
        if (player != null)
        {
            Debug.Log("Player detected, increasing speed and destroying rock.");
            player.IncreaseSpeed(speedIncrease);
            Destroy(gameObject); // Destroy the rock after increasing the player's speed
        }
        else
        {
            Debug.Log("No SkierController component found on the collided object.");
        }
    }
}