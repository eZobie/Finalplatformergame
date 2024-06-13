using UnityEngine;
using System.Collections;

public class SnowmanThrow : MonoBehaviour
{
    public float throwDistance = 10f;
    public float throwSpeed = 10f;
    private bool justThrown = false;
    private GameObject player;

    void Start()
    {
        // Find the player game object in the scene
        player = GameObject.FindWithTag("Player"); // Use tag instead of name for flexibility
        if (player == null)
        {
            Debug.LogError("Player not found!");
        }
    }

    void Update()
    {
        if (player == null)
            return;

        // Calculate the distance to the player
        float distanceToTarget = Vector3.Distance(player.transform.position, transform.position);

        // Check if the player is within throwing distance and the snowman hasn't just thrown
        if (distanceToTarget < throwDistance && !justThrown)
        {
            ThrowSnowball();
        }
    }

    void ThrowSnowball()
    {
        justThrown = true;
        
        // Get a snowball from the pool
        GameObject tempSnowBall = ObjectPool.Instance.GetPooledObject();
        tempSnowBall.transform.position = transform.position;
        tempSnowBall.transform.rotation = transform.rotation;
        tempSnowBall.SetActive(true);

        // Get the Rigidbody component of the instantiated snowball
        Rigidbody tempRb = tempSnowBall.GetComponent<Rigidbody>();
        
        // Calculate the direction to throw the snowball
        Vector3 targetDirection = (player.transform.position - transform.position).normalized;
        
        // Add a small upward angle to the throw
        targetDirection += Vector3.up * 0.33f;
        
        // Adjust the mass if necessary
        tempRb.mass = 0.1f;
        
        // Apply force to the snowball to throw it
        tempRb.AddForce(targetDirection * throwSpeed, ForceMode.Impulse);

        // Start coroutine to reset the throwing status after a short delay
        StartCoroutine(ResetThrownStatus());
    }

    IEnumerator ResetThrownStatus()
    {
        yield return new WaitForSeconds(0.1f);
        justThrown = false;
    }
}