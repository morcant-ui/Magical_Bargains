using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShakingThermo : MonoBehaviour
{
    private float holdTime = 0f;
    private bool isMouseOver = false;
    private bool isShaking = false;
    private bool triedShaking = false;
    private Vector3 originalPos;

    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.1f;

    //flashbang data
    public FlashBangThermo flashbang;
    public Color flashColor;
    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMouseOver && Input.GetMouseButton(0)) // Left click
        {
            holdTime += Time.deltaTime;
            // if you forgot to reset
            if (holdTime < 5f && !isShaking)
            {
                triedShaking = true;
            }
            // if you hold mouse down for holdTime it will shake == reset
            if (holdTime >= 5f && !isShaking)
            {
                StartCoroutine(Shake());
                triedShaking = false;
            }

            //Debug.Log(triedShaking);
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
            flashbang.TriggerFlash(flashColor);
        }
        if (triedShaking){
            Color newFlashColor = Random.ColorHSV(0f, 1f, 0f, 0.9f, 0.5f, 1f);
            flashbang.TriggerFlash(newFlashColor);
            triedShaking = false;
        }
    }

    void OnMouseExit()
    {
        isMouseOver = false;
        holdTime = 0f;
        StopAllCoroutines();
        transform.localPosition = originalPos;
        isShaking = false;
        
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
}
