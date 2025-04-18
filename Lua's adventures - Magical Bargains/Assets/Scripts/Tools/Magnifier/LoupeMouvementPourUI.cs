using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LoupeMouvementPourUI : MonoBehaviour
{
    // Magnifier: we can only drag along the x axis, and constrained by arbitrary bounds (so that we can't go beyond our desk)
    [SerializeField] private Canvas canvas;

    // arbitrary constraints for our x axis
    public float leftBoundX = (float)-5.5f;
    public float rightBoundX = (float)5.5f;

    public void DragHandler(BaseEventData data) {
        PointerEventData pointerData = (PointerEventData)data;

        Vector2 position;


        // calculates the correct position using the canvas
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform) canvas.transform,
            pointerData.position,
            canvas.worldCamera,
            out position);

        float constrainedX = Mathf.Clamp(canvas.transform.TransformPoint(position).x, leftBoundX, rightBoundX);

        transform.position = new Vector3(constrainedX, transform.position.y, transform.position.z);


    }
}
