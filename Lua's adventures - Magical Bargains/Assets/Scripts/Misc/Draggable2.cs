using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable2 : MonoBehaviour
{
    // Chill tuto: https://www.youtube.com/watch?v=izag_ZHwOtM
    // Main idea: boolean 'held' gets updated whenever OnMouseDown or Up gets triggered,
    // We can make use of this knowledge to update the object's position according to mouse coordinates
    // We use a offset variable to record the center-clicked point difference and update the position correctly

    private bool held = false;
    private Vector3 offset;

    [Header("Bounds")]
    [SerializeField] private bool activateBounds = true;
    [SerializeField] private float leftBoundX = (float)-3.5f;
    [SerializeField] private float rightBoundX = (float)3.5f;
    [SerializeField] private float downBoundY = (float)-3.5f;
    [SerializeField] private float upperBoundY = (float)2.5f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (held) {

            if (activateBounds)
            {
                Vector3 currentPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;

                float constrainedX = Mathf.Clamp(currentPosition.x, leftBoundX, rightBoundX);

                float constrainedY = Mathf.Clamp(currentPosition.y, downBoundY, upperBoundY);


                transform.position = new Vector3(constrainedX, constrainedY, currentPosition.z);
            }
            else {
                transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
            }
        }
    }


    private void OnMouseDown() {
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition); // diff from object center to mouse coords
        held = true;
    }


    private void OnMouseUp() {

        held = false;
    }
}
