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
    [SerializeField] private int dialogueERandNB = 1;

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

            finalPath = getPathAtRandom(folderName, PathName);
            
        } else {

            finalPath = Path.Combine(PathName, dialogueName);
        }

        dialogue = Resources.Load<TextAsset>(finalPath);

        if (dialogue != null)
        {
            // , string speakerName="Jean", string speakerColor="#32a852"
            // string colorHeader = $"<color={speakerColor}>{speakerName}:</color>";
            // string modifiedText = colorHeader + dialogue.text;
            // return new TextAsset(modifiedText);
            return dialogue;
        }
        else
        {

            finalPath = getPathAtRandom(folderName, PathName);

            dialogue =  Resources.Load<TextAsset>(finalPath);

            if (dialogue == null) {
                Debug.Log("Reall problem");
                return null;
            }

            return dialogue;
        }
    }


    private string getPathAtRandom(string folderName, string pathName) {


        string PathRand = Path.Combine(pathName, "Rand");

        int nbOfFiles = 1; //Directory.GetFiles(Path.Combine(srcToDialoguePath, PathRand), "*.ink").Length;
        if (folderName == "dialogueA") { nbOfFiles = dialogueARandNB; }
        if (folderName == "dialogueB") { nbOfFiles = dialogueBRandNB; }
        if (folderName == "dialogueC") { nbOfFiles = dialogueCRandNB; }
        if (folderName == "dialogueD") { nbOfFiles = dialogueDRandNB; }
        if (folderName == "dialogueE") { nbOfFiles = dialogueERandNB; }


        int fileNumber = Random.Range(1, nbOfFiles + 1);
        string fileName = folderName + "Rand" + fileNumber;

        return Path.Combine(PathRand, fileName);

    }
}
