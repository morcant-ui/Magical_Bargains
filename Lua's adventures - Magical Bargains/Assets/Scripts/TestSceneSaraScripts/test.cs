using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{

    public GameObject obj;

    // Start is called before the first frame update
    void Start()
    {
        Sprite sprite = Resources.Load<Sprite>("client1");
        obj.GetComponent<SpriteRenderer>().sprite = sprite;




    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
