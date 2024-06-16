using UnityEngine;

public abstract class Obstacle : MonoBehaviour
{
    protected virtual void OnCollisionEnter(Collision collision)
    {
        SkierController player = collision.gameObject.GetComponent<SkierController>();
        if (player != null)
        {
            Debug.Log("Collision detected with SkierController."); // Add this line
            HandleCollision(player);
        }
    }

    protected abstract void HandleCollision(SkierController player); // Ensure this is protected
}