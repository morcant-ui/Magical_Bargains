using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    // 2 min tuto (not very verbose): https://www.youtube.com/watch?v=pFpK4-EqHXQ
    Vector3 mousePosition;

    private Vector3 GetMousePos() {
        return Camera.main.WorldToScreenPoint(transform.position);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnMouseDown() {
        mousePosition = Input.mousePosition - GetMousePos();
    }

    private void OnMouseDrag() {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition - mousePosition);
    }
}
