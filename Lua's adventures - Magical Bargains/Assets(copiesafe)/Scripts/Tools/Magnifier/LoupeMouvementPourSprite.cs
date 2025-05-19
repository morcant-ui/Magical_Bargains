using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoupeMouvementPourSprite : MonoBehaviour
{
    // Magnifier: we can only drag along the x axis, and constrained by arbitrary bounds (so that we can't go beyond our desk)

    private GameObject bounds;
    private bool held = false;
    private Vector3 offset;

    // arbitrary constraints for our x axis
    public float leftBoundX = (float) -5.5f;
    public float rightBoundX = (float) 5.5f;

    void Update()
    {

        if (held)
        {
            // update magnifier's position: if it's being dragged, get the position of the mouse + offset to object center,
            // then update withing the bounds (only modify the magnifier's x coordinates)
            Vector3 currentPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Clamp Function: constraints the current x position to our arbitrary bounds leftBoundX and rightBoundX
            ////////////// for now it does weird things so i comment it out.
            float constrainedX = Mathf.Clamp(currentPosition.x, leftBoundX, rightBoundX);
            //float constrainedX = currentPosition.x;

            // Update the object's position:
            transform.position = new Vector3(constrainedX, transform.position.y, transform.position.z);
        }
    }

    private void OnMouseDown()
    {
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        held = true;
    }

    private void OnMouseUp()
    {
        held = false;
    }
}
