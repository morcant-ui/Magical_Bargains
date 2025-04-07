using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineBehavior : MonoBehaviour
{
    public GameObject outline;
    private SpriteRenderer outlineSprite;
    public float outlineWidth = 20f;
    private Transform parentTransform;
    // Start is called before the first frame update
    void Start()
    {
        outlineSprite = outline.GetComponent<SpriteRenderer>();
        parentTransform = transform;
        outlineSprite.enabled = false;
        AdjustOutlineScale();
    }

    // Update is called once per frame
    void Update()
    {
        AdjustOutlineScale();
    }

    void OnMouseEnter() {
        outlineSprite.enabled = true;
        //highlight.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 0f, 0.5f); // Yellow with 50% transparency
    }

    // ONMouseExits, when mouse leaves collider zone
    void OnMouseExit() {
        outlineSprite.enabled = false;
    }

    void OnMouseDown(){

    }

     void AdjustOutlineScale()
    {
        if (outline != null && parentTransform != null)
        {
            // Get the parent object's current scale
            Vector3 parentScale = parentTransform.localScale;
            
            // Calculate the outline scale based on the parent's size and desired outline width
            float outlineFactor = outlineWidth / 100f;  // Convert the outline width to world units
            outline.transform.localScale = new Vector3(
                parentScale.x + outlineFactor, 
                parentScale.y + outlineFactor, 
                parentScale.z
            );
        }
    }
}

