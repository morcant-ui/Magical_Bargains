using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThermometerManager : MonoBehaviour
{
    [SerializeField] private GameObject thermometer;

    private bool active;
    // Start is called before the first frame update
    void Start()
    {
        thermometer.SetActive(false);
        active = false;
    }

    public void ButtonClicked() {

        if (active)
        {

            thermometer.SetActive(false);
            active = false;
        }
        else {

            thermometer.SetActive(true);
            active = true;
        }
    }
}
