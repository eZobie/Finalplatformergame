using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class SkierController : MonoBehaviour
{
    public float initialSpeed = 5f;
    public float maxSpeed = 10f;
    public float acceleration = 2f;
    public float boostSpeed = 15f;
    public float boostDuration = 3f;
    public float rotationSpeed = 200f;
    public float maxTurnAngle = 90f;
    public float raycastDistance = 1f;
    public float collisionSpeedReduction = 2f;
    public float knockbackForce = 10f;
    public float knockbackDuration = 0.7f;
    public AudioClip collisionSound;
    public float screenShakeDuration = 0.2f;
    public float screenShakeMagnitude = 0.1f;

    private Rigidbody rb;
    private Vector3 initialForwardDirection;
    private Vector3 forwardDirection;
    private float currentSpeed;
    private bool isGrounded;
    private bool isBoosting;
    private bool isKnockedBack;
    private float boostTimer;
    private AudioSource audioSource;
    private SimpleScreenShake screenShake; // Reference to SimpleScreenShake
    private bool isControllable = true;

    // Reference to the skiing sound
    public AudioClip skiingSound;
    private AudioSource skiingAudioSource;
    
    // Volume for the skiing sound
    [SerializeField]
    private float skiingSoundVolume = 0.5f; // Adjust this value to make the sound quieter

    private void OnEnable()
    {
        ObstacleCollisionEvent.OnObstacleCollision += HandleObstacleCollision;
    }

    private void OnDisable()
    {
        ObstacleCollisionEvent.OnObstacleCollision -= HandleObstacleCollision;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        screenShake = Camera.main.GetComponent<SimpleScreenShake>(); // Get the SimpleScreenShake component from the main camera

        // Setup skiing audio source
        skiingAudioSource = gameObject.AddComponent<AudioSource>();
        skiingAudioSource.clip = skiingSound;
        skiingAudioSource.loop = true; // Loop the skiing sound
        skiingAudioSource.playOnAwake = false; // Do not play on awake
        skiingAudioSource.volume = skiingSoundVolume; // Set the volume for the skiing sound

        initialForwardDirection = transform.forward;
        forwardDirection = transform.forward;
        currentSpeed = initialSpeed;
    }

    void Update()
    {
        if (!isKnockedBack && isControllable)
        {
            HandleInput();
        }
        HandleMovement();
        HandleBoost();
        HandleSkiingSound(); // Add this to manage the skiing sound
    }

    private void HandleInput()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        if (isGrounded && direction.magnitude >= 0.1f)
        {
            RotateSkier(direction);
        }
    }

    private void HandleMovement()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance))
        {
            if (!isBoosting)
            {
                Accelerate();
            }
            else
            {
                HandleBoostTimer();
            }

            MoveSkier();
            isGrounded = true;
        }
        else
        {
            StopSkier();
            isGrounded = false;
        }
    }

    private void HandleBoost()
    {
        if (Input.GetKeyDown(KeyCode.X) && !isBoosting)
        {
            StartCoroutine(ActivateBoost());
        }
    }

    private void HandleSkiingSound()
    {
        if (isGrounded && currentSpeed > 0f)
        {
            if (!skiingAudioSource.isPlaying)
            {
                skiingAudioSource.Play();
            }
        }
        else
        {
            if (skiingAudioSource.isPlaying)
            {
                skiingAudioSource.Stop();
            }
        }
    }

    private void RotateSkier(Vector3 direction)
    {
        float angleToInitial = Vector3.SignedAngle(initialForwardDirection, transform.forward, Vector3.up);
        float targetAngle = Vector3.SignedAngle(transform.forward, direction, Vector3.up);
        float newAngle = Mathf.Clamp(angleToInitial + targetAngle, -maxTurnAngle, maxTurnAngle);

        Quaternion targetRotation = Quaternion.Euler(0f, newAngle - angleToInitial, 0f) * transform.rotation;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        forwardDirection = transform.forward;
    }

    private void Accelerate()
    {
        currentSpeed = Mathf.Clamp(currentSpeed + acceleration * Time.deltaTime, initialSpeed, maxSpeed);
    }

    private void HandleBoostTimer()
    {
        boostTimer -= Time.deltaTime;
        if (boostTimer <= 0f)
        {
            isBoosting = false;
            currentSpeed = initialSpeed;
        }
    }

    private void MoveSkier()
    {
        Vector3 movement = forwardDirection * currentSpeed;
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
    }

    private void StopSkier()
    {
        currentSpeed = initialSpeed;
        rb.velocity = Vector3.zero;
    }

    IEnumerator ActivateBoost()
    {
        isBoosting = true;
        boostTimer = boostDuration;
        currentSpeed = boostSpeed;
        yield return new WaitForSeconds(boostDuration);
        isBoosting = false;
        currentSpeed = initialSpeed;
        yield return new WaitForSeconds(5f);
    }

    private void HandleObstacleCollision(Obstacle obstacle, SkierController player)
    {
        if (player == this)
        {
            StartCoroutine(KnockbackHandler.Instance.ApplyKnockback(player, knockbackForce, knockbackDuration));
            SoundManager.Instance.PlaySound(collisionSound);
            if (screenShake != null)
            {
                screenShake.Shake(screenShakeDuration, screenShakeMagnitude); // Trigger screen shake
            }
        }
    }

    public void ReduceSpeed(float amount)
    {
        currentSpeed = Mathf.Max(0, currentSpeed - amount);
        Debug.Log("Speed reduced to: " + currentSpeed);
    }

    public void IncreaseSpeed(float amount)
    {
        currentSpeed = Mathf.Min(maxSpeed, currentSpeed + amount);
        Debug.Log("Speed increased to: " + currentSpeed);
    }

    public void SetSpeed(float speed)
    {
        currentSpeed = Mathf.Clamp(speed, 0, maxSpeed);
        Debug.Log("Speed set to: " + currentSpeed);
    }

    public float CurrentSpeed
    {
        get { return currentSpeed; }
    }

    public void SetControl(bool controllable)
    {
        isControllable = controllable;
        if (!controllable)
        {
            Debug.Log("Player control disabled.");
        }
        else
        {
            Debug.Log("Player control enabled.");
        }
    }
}
