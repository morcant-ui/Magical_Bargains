using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OfferDialogsManager : MonoBehaviour
{

    [Header("Dialogue UI")]
    [SerializeField] private GameObject offerPanel;
    [SerializeField] private TextMeshProUGUI offerText;


    private int originalOffer;
    private int yourOffer;

    private bool isBargainOn = false;
    // Start is called before the first frame update
    public void StartBargain()
    {
        isBargainOn = true;
        originalOffer = 100;
        yourOffer = originalOffer;
        UpdateOfferDisplay();   
    }

    // Update is called once per frame
    void Update()
    {
        offerPanel.SetActive(isBargainOn);
    }

    public void ChangeOffer(int delta)
    {
        yourOffer = Mathf.Max(0,yourOffer + delta);
        UpdateOfferDisplay();
        
    }

    public void AcceptOffer()
    {
        isBargainOn = false;
        Debug.Log("offerAccepted");
    }

    void UpdateOfferDisplay()
    {
        offerText.text = "The client offer is " + originalOffer.ToString() + " coins! \n Your offer is now " + yourOffer.ToString() + " coins :)";
    }
}