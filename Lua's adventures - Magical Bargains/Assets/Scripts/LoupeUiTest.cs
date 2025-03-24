using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoupeUiTest : MonoBehaviour
{

    public GameObject highlight;
    public GameObject magnifier;

    private bool activated = false;



    // Start is called before the first frame update
    void Start()
    {
        highlight.GetComponent<SpriteRenderer>().enabled = false;
        magnifier.GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (activated == true)
        {
            highlight.GetComponent<SpriteRenderer>().enabled = true;
            magnifier.GetComponent<SpriteRenderer>().enabled = true;

        }
        else {
            highlight.GetComponent<SpriteRenderer>().enabled = false;
            magnifier.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    void OnMouseUp() {
        if (activated == true)
        {
            activated = false;
        }
        else
        {
            activated = true;
        }
    }

    //highlight.GetComponent<SpriteRenderer>().enabled = true;


}
