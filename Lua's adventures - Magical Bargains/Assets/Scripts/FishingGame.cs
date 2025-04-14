using System.Collections;
using UnityEngine;
using System;

public class FishingGame : MonoBehaviour
{
    public Transform center;
    public Transform targetArrow;
    public Transform fish;
    public float rotationSpeed = 100f;
    public float hitThreshold = 10f;

    private bool hitRegistered = false;

    private void Start()
    {
        center.gameObject.SetActive(false);
        fish.gameObject.SetActive(false);
        targetArrow.gameObject.SetActive(false);
    }
    public IEnumerator PlayFishingGame()
    {
        center.gameObject.SetActive(true);
        fish.gameObject.SetActive(true);
        targetArrow.gameObject.SetActive(true);
        // Reset state
        hitRegistered = false;

        float angle = 0f;
        float radius = (fish.position - center.position).magnitude;

        while (!hitRegistered)
        {
            angle += rotationSpeed * Time.deltaTime;

            // Move the rotatingRect in a circle
            float radians = angle * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0f) * radius;
            fish.position = center.position + offset;

            if (Input.GetMouseButtonDown(0))
            {
                CheckHit();
            }

            yield return null;
        }
        center.gameObject.SetActive(false);
        fish.gameObject.SetActive(false);
        targetArrow.gameObject.SetActive(false);

        //Debug.Log("fISHY CATCHED");
    }

    void CheckHit()
    {
        Vector2 toRect = fish.position - center.position;
        Vector2 toArrow = targetArrow.position - center.position;

        float angleBetween = Vector2.Angle(toRect, toArrow);

        if (angleBetween < hitThreshold)
        {
            //Debug.Log("Hit!");
            hitRegistered = true;
        }
    }
}
