using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightOnHover : MonoBehaviour
{

    // reference the GameObject in question
    // note: it's the parent object that has to have a collider !
    public GameObject highlight;


    // Start is called before the first frame update
    void Start()
    {
        highlight.GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // OnMouseEnter: when mouse hits collider
    void OnMouseEnter() {
        highlight.GetComponent<SpriteRenderer>().enabled = true;
        //highlight.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 0f, 0.5f); // Yellow with 50% transparency
    }

    // ONMouseExits, when mouse leaves collider zone
    void OnMouseExit() {
        highlight.GetComponent<SpriteRenderer>().enabled = false;
    }
}
