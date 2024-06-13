using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
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
    public float knockbackDuration = 0.5f;
    public AudioClip collisionSound;
    public float screenShakeDuration = 0.2f;
    public float screenShakeMagnitude = 0.1f;

    private Rigidbody rb;
    private Vector3 forwardDirection;
    private float currentSpeed;
    private bool isGrounded;
    private bool isBoosting;
    private bool isKnockedBack;
    private float boostTimer;
    private AudioSource audioSource;
    private ScreenShake screenShake;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        screenShake = Camera.main.GetComponent<ScreenShake>();
        forwardDirection = transform.forward;
        currentSpeed = initialSpeed;
    }

    void Update()
    {
        if (!isKnockedBack)
        {
            HandleInput();
        }
        HandleMovement();
        HandleBoost();
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

    private void RotateSkier(Vector3 direction)
    {
        float angle = Vector3.SignedAngle(transform.forward, direction, Vector3.up);
        angle = Mathf.Clamp(angle, -maxTurnAngle, maxTurnAngle);
        Quaternion targetRotation = Quaternion.Euler(0f, angle, 0f) * transform.rotation;
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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Rock"))
        {
            StartCoroutine(HandleCollision());
        }
    }

    private IEnumerator HandleCollision()
    {
        // Play collision sound
        if (collisionSound != null)
        {
            audioSource.PlayOneShot(collisionSound);
        }

        // Reduce speed
        ReduceSpeed(collisionSpeedReduction);

        // Knockback effect
        isKnockedBack = true;
        Vector3 knockbackDirection = (transform.position - rb.position).normalized;
        float knockbackEndTime = Time.time + knockbackDuration;

        // Start screen shake
        if (screenShake != null)
        {
            StartCoroutine(screenShake.Shake(screenShakeDuration, screenShakeMagnitude));
        }

        while (Time.time < knockbackEndTime)
        {
            rb.velocity = knockbackDirection * knockbackForce;
            yield return null;
        }

        isKnockedBack = false;
        rb.velocity = Vector3.zero;
    }

    public void ReduceSpeed(float amount)
    {
        initialSpeed = Mathf.Max(0, initialSpeed - amount);
    }

    public void IncreaseSpeed(float amount)
    {
        initialSpeed += amount;
        Debug.Log("Speed increased to: " + initialSpeed);
    }
 
}