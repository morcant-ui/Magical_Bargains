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


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (held) {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
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
