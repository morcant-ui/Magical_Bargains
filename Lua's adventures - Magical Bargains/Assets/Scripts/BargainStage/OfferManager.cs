using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OfferManager : MonoBehaviour
{

    [Header("Dialogue UI")]
    [SerializeField] private GameObject offerPanel;
    [SerializeField] private TextMeshProUGUI offerText;
    [SerializeField] private TextMeshProUGUI offeredPrice;

    private int originalOffer;
    private int yourOffer;

    private bool isBargainOn = false;
    private bool choosingAction = false;



    public void StartBargain(string currentOffer, string maxSavings)
    {

        isBargainOn = true;



        originalOffer = int.Parse(currentOffer);
        yourOffer = (int)Mathf.Round((float)double.Parse(maxSavings));


        UpdateOfferDisplay();
    }

    public void ChooseAction(string currentOffer, string maxSavings)
    {
        choosingAction = true;

        UpdateOfferDisplay();
        
    }

    // Update is called once per frame
    void Update()
    {
        offerPanel.SetActive(isBargainOn || choosingAction);
    }

    public void ChangeOffer(int delta)
    {
        yourOffer = Mathf.Max(0,yourOffer + delta);
        UpdateOfferDisplay();
        
    }

    public void ChoiceActionDone()
    {
        choosingAction = false;
    }
    public void AcceptOffer()
    {
        isBargainOn = false;
        choosingAction = false;
    }

    public void RefuseOffer()
    {
        isBargainOn = false;
        choosingAction = false;
        yourOffer = 0;
    }

    void UpdateOfferDisplay()
    {
        if (isBargainOn)
        {
            offerText.text = "The client offer is " + originalOffer.ToString() + " coins!";
            offeredPrice.text = "Your offer:\n" + yourOffer.ToString("00.00");
        }
        else if (choosingAction)
        {
            offerText.text = "What do you want to try?";
        }
        
    }

    //Getter to get final Offer
    public int FinalOffer => yourOffer;
}