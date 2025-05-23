using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class OfferDecisionManager : MonoBehaviour
{
    private string srcToDialoguePath = Path.Combine("Assets", "Resources");


    public void tryRand() {

        Debug.Log("RAND: " + Random.Range(1,4));
    }

    public TextAsset LoadDialogue(string dialogueName, string folderName) {


        string PathName = Path.Combine("Dialogues", folderName);
        TextAsset dialogue;
        string finalPath;

        // if dialogue name could not be found - pull a random dialogue
        if (dialogueName == null)
        {

            string PathRand = Path.Combine("test", folderName);
            int nbOfFiles = Directory.GetFiles(Path.Combine(srcToDialoguePath, PathRand), "*.ink").Length;

            int fileNumber = Random.Range(1, nbOfFiles + 1);
            string fileName = folderName + "Rand" + fileNumber;

            finalPath = Path.Combine(PathRand, fileName);

            dialogue = Resources.Load<TextAsset>(finalPath);
        } else {

            finalPath = Path.Combine(PathName, dialogueName);

            dialogue = Resources.Load<TextAsset>(finalPath);
        }

        Debug.Log("Search for a path: \n" + finalPath);

        if (dialogue != null)
        {
            return dialogue;
        } else {
            Debug.Log("Problem !");
            return null;
        }
    }
}
