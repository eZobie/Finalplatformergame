using UnityEngine;

public class Rock : Obstacle
{
    [SerializeField]
    private float speedReduction = 2.0f;

    public float SpeedReduction
    {
        get => speedReduction;
        set => speedReduction = Mathf.Max(0, value); // Ensure speed reduction is not negative
    }

    private AudioSource audioSource;
    private SimpleScreenShake screenShake;

    private void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // Ensure the Rigidbody is kinematic to prevent falling

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing on Rock.");
        }

        screenShake = Camera.main.GetComponent<SimpleScreenShake>();
        if (screenShake == null)
        {
            Debug.LogError("SimpleScreenShake component is missing on Main Camera.");
        }
    }

    protected override void HandleCollision(SkierController player)
    {
        Debug.Log("Player collided with Rock, reducing speed.");
        player.ReduceSpeed(SpeedReduction);

        if (screenShake != null)
        {
            screenShake.Shake(0.2f, 0.05f); // Adjusted shake parameters for less chaos
        }
    }
}