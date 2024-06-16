using UnityEngine;
using System.Collections;

public class KnockbackHandler : MonoBehaviour
{
    public static KnockbackHandler Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator ApplyKnockback(SkierController player, float knockbackForce, float knockbackDuration)
    {
        player.SetControl(false);
        Rigidbody rb = player.GetComponent<Rigidbody>();
        Vector3 knockbackDirection = -player.transform.forward;
        float knockbackEndTime = Time.time + knockbackDuration;

        while (Time.time < knockbackEndTime)
        {
            rb.velocity = knockbackDirection * knockbackForce;
            yield return null;
        }

        rb.velocity = Vector3.zero;
        player.SetControl(true);
    }
}