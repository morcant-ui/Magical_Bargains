using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class DialogueLoader : MonoBehaviour
{
    [SerializeField] private int dialogueARandNB = 1;
    [SerializeField] private int dialogueBRandNB = 1;
    [SerializeField] private int dialogueCRandNB = 1;
    [SerializeField] private int dialogueDRandNB = 3;

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
            

            string PathRand = Path.Combine(PathName, "Rand");

            int nbOfFiles = 1; //Directory.GetFiles(Path.Combine(srcToDialoguePath, PathRand), "*.ink").Length;
            if (folderName == "dialogueA") { nbOfFiles = dialogueARandNB; }
            if (folderName == "dialogueB") { nbOfFiles = dialogueBRandNB; }
            if (folderName == "dialogueC") { nbOfFiles = dialogueCRandNB; }
            if (folderName == "dialogueD") { nbOfFiles = dialogueDRandNB; }


            int fileNumber = Random.Range(1, nbOfFiles + 1);
            string fileName = folderName + "Rand" + fileNumber;

            finalPath = Path.Combine(PathRand, fileName);

            
        } else {

            finalPath = Path.Combine(PathName, dialogueName);
        }

        dialogue = Resources.Load<TextAsset>(finalPath);

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
