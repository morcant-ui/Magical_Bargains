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

    public bool gameActivated = false;



    public IEnumerator PlayFishingGame(Action<bool> onComplete)
    {
        hitRegistered = false;
        gameActivated = true;

        // Set targetArrow at a random position around center
        SetRandomTargetPosition();

        float angle = 0f;
        float radius = (fish.position - center.position).magnitude;

        while (!hitRegistered)
        {
            if (!gameActivated) {
                Debug.Log("ABORT");
                break;
            }

            angle += rotationSpeed * Time.deltaTime;

            float radians = angle * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0f) * radius;
            fish.position = center.position + offset;

            if (Input.GetMouseButtonDown(1))
            {
                if (CheckHit())
                {
                    hitRegistered = true;
                    onComplete?.Invoke(true);  // success
                    gameActivated = false;
                    yield break;
                }
                else
                {
                    onComplete?.Invoke(false); // fail
                    gameActivated = false;
                    yield break;
                }
            }

            

            yield return null;
        }

        gameActivated = false;
        Debug.Log("finished");
    }

    private void SetRandomTargetPosition()
    {
        float radius = (targetArrow.position - center.position).magnitude;
        float randomAngle = UnityEngine.Random.Range(45f, 340f);
        float radians = randomAngle * Mathf.Deg2Rad;

        Vector3 offset = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0f) * radius;
        targetArrow.position = center.position + offset;

        // Rotate the arrow so it faces outward
        targetArrow.rotation = Quaternion.Euler(0f, 0f, randomAngle);
    }

    bool CheckHit()
    {
        Vector2 toRect = fish.position - center.position;
        Vector2 toArrow = targetArrow.position - center.position;

        float angleBetween = Vector2.Angle(toRect, toArrow);

        // if (angleBetween < hitThreshold)
        // {
        //     //Debug.Log("Hit!");
        //     hitRegistered = true;
        // }
        return angleBetween < hitThreshold;
    }


}
