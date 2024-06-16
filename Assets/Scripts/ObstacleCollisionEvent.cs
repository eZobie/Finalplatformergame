using UnityEngine;
using System;

public class ObstacleCollisionEvent : MonoBehaviour
{
    public static event Action<Obstacle, SkierController> OnObstacleCollision;

    private void OnCollisionEnter(Collision collision)
    {
        SkierController player = collision.gameObject.GetComponent<SkierController>();
        if (player != null)
        {
            OnObstacleCollision?.Invoke(GetComponent<Obstacle>(), player);
        }
    }
}