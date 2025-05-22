using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OfferManager : MonoBehaviour
{

    [Header("Dialogue UI")]
    [SerializeField] private GameObject offerPanel;
    [SerializeField] private TextMeshProUGUI offerText;

    private int originalOffer;
    private int yourOffer;

    private bool isBargainOn = false;



    public void StartBargain(string currentOffer)
    {

        isBargainOn = true;
        originalOffer = int.Parse(currentOffer);
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
    }

    void UpdateOfferDisplay()
    {
        offerText.text = "The client offer is " + originalOffer.ToString() + " coins! \n Your offer is now " + yourOffer.ToString() + " coins :)";
    }

    //Getter to get final Offer
    public int FinalOffer => yourOffer;
}