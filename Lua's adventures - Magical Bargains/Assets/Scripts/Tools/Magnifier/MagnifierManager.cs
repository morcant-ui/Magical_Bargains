using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnifierManager : MonoBehaviour
{

    [SerializeField] private GameObject magnifier;

    private bool active;

    // Start is called before the first frame update
    void Start()
    {
        magnifier.SetActive(false);
        active = false;
        
    }

    public void ButtonClicked() {

        if (active)
        {

            magnifier.SetActive(false);
            active = false;
        }
        else {

            magnifier.SetActive(true);
            active = true;
        }
    }


}
