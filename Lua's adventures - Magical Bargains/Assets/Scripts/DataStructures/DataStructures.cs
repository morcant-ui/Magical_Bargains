using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ClientData
{
    public string clientSprite;
    public string artifactSprite;

    public string magnifierSprite;
    public string cameraSprite;
    public string thermoColor;
    public string thermoIntensity;

    public string dialogueA;
    public string dialogueB;
    public string dialogueC;
    public string dialogueD;
    
    public string artifactOffer;
    public string minOfferAccepted;
    public string finalPrice;
    public string grade;
}

[System.Serializable]
public class ListClients
{
    public List<ClientData> clients;
}

//////////////////////////////////////////////////////////////////////////

[System.Serializable]
public class LevelData
{
    public string listClientsName;
    public string grandpaIntroDialogue;
    public int maxTime;
}


[System.Serializable]
public class ListLevels
{
    public List<LevelData> levelData;
}

//////////////////////////////////////////////////////////////////////////

[System.Serializable]
public class CutsceneData
{
    public List<CutsceneImageData> imageNames;
    public string textName;
}

[System.Serializable]
public class CutsceneImageData
{
    public string imageName;
}