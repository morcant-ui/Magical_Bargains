using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShakingThermo : MonoBehaviour
{
    public GameObject holder;

    private float holdTime = 0f;
    private bool isMouseOver = false;
    private bool isShaking = false;
    private bool triedShaking = false;
    private Vector3 originalPos;

    private Color flashColor;
    private Coroutine flashCoroutine;
    private Coroutine shakeCoroutine;

    public float shakeMagnitude = 0.1f;

    public Image screenOverlay;

    public float fadeDuration = 1.5f;
    
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
            if (holdTime < 1f && !isShaking)
            {
                triedShaking = true;
            }
            // if you hold mouse down for holdTime it will shake == reset
            if (holdTime >= 1f && !isShaking)
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
                flashColor = colorComponent.thermoColor;
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
        holder.SetActive(false);
    }
}
