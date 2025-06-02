using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShakingThermo : MonoBehaviour
{
    public GameObject holder;

    private float holdTime = 0f;
    private float minHoldTime = 0.5f;
    private bool isMouseOver = false;
    private bool isShaking = false;
    private bool triedShaking = false;
    private Vector3 originalPos;

    private Color flashColor;
    private Coroutine flashCoroutine;
    private Coroutine shakeCoroutine;

    public float shakeMagnitude = 0.1f;

    public Image screenOverlay;

    public float fadeDuration = 1f;

    [Header("poof sound Clip")]
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private float volume = 0.1f;

    public void StartProcess()
    {
        originalPos = transform.localPosition;
        screenOverlay.color = Color.clear;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isMouseOver && Input.GetMouseButton(0)) // Left click
        {
            holdTime += Time.deltaTime;
            // if you forgot to reset
            if (holdTime < minHoldTime && !isShaking)
            {
                triedShaking = true;
            }
            // if you hold mouse down for holdTime it will shake == reset
            if (holdTime >= minHoldTime && !isShaking)
            {
                //StartCoroutine(Shake());
                shakeCoroutine = StartCoroutine(Shake());
                triedShaking = false;
            }

           
        }
        else
        {
            holdTime = 0f;

            if (isShaking)
            {
                StopAllCoroutines();
                transform.localPosition = originalPos;
                isShaking = false;
            }
        }
    }

    void OnMouseEnter()
    {
        isMouseOver = true;
    }

    void OnMouseUp()
    {
        if (!triedShaking){
            // get color and intensity
            ThermoColor colorComponent = GameObject.FindWithTag("currentArtifact").GetComponent<ThermoColor>();
            if (colorComponent != null)
            {
                string hex = colorComponent.thermoColor;
                float alpha = colorComponent.intensity;
                flashColor = ParseColor(hex, alpha);
            }
            
            
           TriggerFlash(flashColor);
        }
        if (triedShaking){
            Color newFlashColor = Random.ColorHSV(0f, 1f, 0f, 0.9f, 0.5f, 1f);
            TriggerFlash(newFlashColor);
            triedShaking = false;
        }
    }

    void OnMouseExit()
    {
        isMouseOver = false;
        holdTime = 0f;

        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
            transform.localPosition = originalPos;
            shakeCoroutine = null;
            isShaking = false;
        }
        
        
        
    }

    System.Collections.IEnumerator Shake()
    {
        isShaking = true;

        while (true)
        {
            Vector3 randomPoint = originalPos + Random.insideUnitSphere * shakeMagnitude;
            transform.localPosition = new Vector3(randomPoint.x, randomPoint.y, originalPos.z);
            yield return null;
        }

        
    }

    public void TriggerFlash(Color flashColor)
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
            transform.localPosition = originalPos;
            shakeCoroutine = null;
            isShaking = false;
        }

        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }

        flashCoroutine = StartCoroutine(FlashEffect(flashColor));
    }

    private IEnumerator FlashEffect(Color flashColor)
    {
        holder.SetActive(true);

        // play clip
        Transform position = holder.GetComponent<Transform>();
        AudioManager.GetInstance().PlayClip(audioClip, position, volume);

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
        yield return new WaitForSeconds(0.3f);

        // Fade out
        t = 0;
        while (t < fadeDuration)
        {
            screenOverlay.color = Color.Lerp(flashColor, Color.clear, t / fadeDuration);
            t += Time.deltaTime;
            yield return null;
        }
        
        screenOverlay.color = Color.clear;
        holder.SetActive(false);
    }


    // helper function to get color from game data
    Color ParseColor(string hex, float alpha)
    {

        Debug.Log("-----PARSE COLOR: HEX: " + hex + ", ALPHA: " + alpha);

        if (alpha == 0.0)
        {
            Color outColor = ColorUtility.TryParseHtmlString(hex, out var color)
                ? color
                : Color.black;

            if (outColor == Color.black) { Debug.Log("PB1"); }
            return outColor;
        }
        else
        {

            // manual conversion from hex to rgb, then add alpha channel at the end
            hex = hex.Replace("#", "");

            if (hex.Length != 6) { Debug.Log("PB2");  return Color.black; }

            int red = int.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            int green = int.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            int blue = int.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

            float r = (float)red / 255f;
            float g = (float)green / 255f;
            float b = (float)blue / 255f;

            Color c = new Color(r, g, b, alpha);

            Debug.Log("------C: " + c);

            return c;
        }
    }
}
