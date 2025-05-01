using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ClientData
{
    public string clientID;
    public string clientSprite;
    public string dialogueA;
    public string dialogueB;
    public string artifactID;
    public string artifactSprite; // store as hex, convert to Color
    public string magnifierSprite;
    public string cameraSprite;
    public string thermoColor;
    public string thermoIntensity;
    public string artifactOffer;
}

[System.Serializable]
public class LevelData
{
    public List<ClientData> clients;
}