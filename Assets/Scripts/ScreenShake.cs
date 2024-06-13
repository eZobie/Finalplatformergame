using UnityEngine;
using System.Collections;

public class ScreenShake : MonoBehaviour
{
    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPosition = transform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float offsetX = Random.Range(-3f, 3f) * magnitude;
            float offsetZ = Random.Range(-2f, 2f) * magnitude;

            transform.localPosition = originalPosition + new Vector3(offsetX, 0, offsetZ);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPosition;
    }
}