using UnityEngine;

public class IcePatch : Obstacle
{
    private Color[] rainbowColors = {
        Color.red,
        new Color(1.0f, 0.5f, 0f), // Orange
        Color.yellow,
        Color.green,
        Color.blue,
        new Color(0.29f, 0f, 0.51f), // Indigo
        new Color(0.56f, 0f, 1.0f) // Violet
    };

    [SerializeField]
    private AudioClip collectSound; // Sound when player collects the ice patch

    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    protected override void HandleCollision(SkierController player)
    {
        Debug.Log("Player collided with IcePatch, changing color.");
        ChangePlayerColor(player);
        Destroy(gameObject);
    }

    private void ChangePlayerColor(SkierController player)
    {
        Color newColor = rainbowColors[Random.Range(0, rainbowColors.Length)];
        Renderer[] playerRenderers = player.GetComponentsInChildren<Renderer>();
        if (playerRenderers != null && playerRenderers.Length > 0)
        {
            foreach (Renderer renderer in playerRenderers)
            {
                Debug.Log("Changing color for renderer: " + renderer.name);
                Material newMaterial = new Material(renderer.material);
                newMaterial.color = newColor;
                renderer.material = newMaterial;
            }
           // Debug.Log("Player color changed to: " + newColor);
            PlaySound(); // Play sound when the color changes
        }
        else
        {
            Debug.LogWarning("Player does not have any Renderer components.");
        }
    }

    private void PlaySound()
    {
        if (collectSound != null && _audioSource != null)
        {
            Debug.Log("Playing sound: " + collectSound.name);
            _audioSource.PlayOneShot(collectSound);
        }
        else
        {
            Debug.LogWarning("Collect sound or audio source is missing.");
        }
    }
}
    