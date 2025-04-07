using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class FlashBangThermo : MonoBehaviour
{
    public Image screenOverlay;

    public float fadeDuration = 1.5f;

    public void TriggerFlash(Color flashColor)
    {
        StartCoroutine(FlashEffect(flashColor));
    }

    private IEnumerator FlashEffect(Color flashColor)
    {
        // Fade in
        float t = 0;
        float fadeInDuration = 0.3f;
        while (t < fadeInDuration)
        {
            screenOverlay.color = Color.Lerp(Color.clear, flashColor, t / fadeInDuration);
            t += Time.deltaTime;
            yield return null;
        }

        screenOverlay.color = flashColor;

        // Wait a bit
        yield return new WaitForSeconds(0.5f);

        // Fade out
        t = 0;
        while (t < fadeDuration)
        {
            screenOverlay.color = Color.Lerp(flashColor, Color.clear, t / fadeDuration);
            t += Time.deltaTime;
            yield return null;
        }

        screenOverlay.color = Color.clear;
    }
}
