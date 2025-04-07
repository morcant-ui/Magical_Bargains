using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject outline;
    public Color originalColor;
    private SpriteRenderer spriteRenderer;


    void Start()
    {
        outline.SetActive(false);
        spriteRenderer = outline.GetComponent<SpriteRenderer>();
        spriteRenderer.color = originalColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseEnter()
    {
        outline.SetActive(true);
    }

    void OnMouseExit()
    {
        outline.SetActive(false);
    }

    void OnMouseDown()
    {
        spriteRenderer.color = originalColor*0.8f;
    }
}
