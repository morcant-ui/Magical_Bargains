using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Ink.Runtime;



public class DialogueLoader : MonoBehaviour
{
    [SerializeField] private int dialogueARandNB = 1;
    [SerializeField] private int dialogueBRandNB = 1;
    [SerializeField] private int dialogueCRandNB = 1;
    [SerializeField] private int dialogueDRandNB = 3;
    [SerializeField] private int dialogueERandNB = 1;
    [SerializeField] private int dialogueFRandNB = 1;
    [SerializeField] private int dialogueGRandNB = 1;
    [SerializeField] private int dialogueHRandNB = 1;
    [SerializeField] private int dialogueIRandNB = 1;


    private string srcToDialoguePath = System.IO.Path.Combine("Assets", "Resources");


    public void tryRand() {

        Debug.Log("RAND: " + Random.Range(1,4));
    }

    public TextAsset LoadDialogue(string dialogueName, string folderName, bool isClientIntro = false) {

        string PathName = System.IO.Path.Combine("Dialogues", "Clients");

        //if (isClientIntro)
        //{
        //    PathName = Path.Combine(PathName, "ClientIntro");
        //} else 
        //{
        //    PathName = Path.Combine(PathName, "ClientOutro");
        //}

        TextAsset dialogue;
        string finalPath;

        // if dialogue name could not be found - pull a random dialogue
        if (dialogueName == null)
        {
            finalPath = getPathAtRandom(folderName, PathName);
            
        } else {

            finalPath = System.IO.Path.Combine(PathName, dialogueName);
        }

        dialogue = Resources.Load<TextAsset>(finalPath);

        if (dialogue != null && DoesKnotExists(dialogue, folderName))
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

        Debug.Log("PULLING DIALOGUE AT RANDOM");

        string PathRand = System.IO.Path.Combine(pathName, "Rand", folderName);

        int nbOfFiles = 1; //Directory.GetFiles(Path.Combine(srcToDialoguePath, PathRand), "*.ink").Length;
        if (folderName == "dialogueA") { nbOfFiles = dialogueARandNB; }
        if (folderName == "dialogueB") { nbOfFiles = dialogueBRandNB; }
        if (folderName == "dialogueC") { nbOfFiles = dialogueCRandNB; }
        if (folderName == "dialogueD") { nbOfFiles = dialogueDRandNB; }
        if (folderName == "dialogueE") { nbOfFiles = dialogueERandNB; }
        if (folderName == "dialogueF") { nbOfFiles = dialogueFRandNB; }
        if (folderName == "dialogueG") { nbOfFiles = dialogueGRandNB; }
        if (folderName == "dialogueH") { nbOfFiles = dialogueHRandNB; }
        if (folderName == "dialogueI") { nbOfFiles = dialogueIRandNB; }


        int fileNumber = Random.Range(1, nbOfFiles + 1);
        string fileName = folderName + "Rand" + fileNumber;

        return System.IO.Path.Combine(PathRand, fileName);

    }


    private bool DoesKnotExists(TextAsset dialogue, string folderName) {
        Story story = new Story(dialogue.text);

        string knotName = "";

        if (folderName == "dialogueA") { knotName = "a"; }
        if (folderName == "dialogueB") { knotName = "b"; }
        if (folderName == "dialogueC") { knotName = "c"; }
        if (folderName == "dialogueD") { knotName = "d"; }
        if (folderName == "dialogueE") { knotName = "e"; }
        if (folderName == "dialogueF") { knotName = "f"; }
        if (folderName == "dialogueG") { knotName = "g"; }
        if (folderName == "dialogueH") { knotName = "h"; }
        if (folderName == "dialogueI") { knotName = "i"; }

        Debug.Log("------------------here");

        try
        {
            Debug.Log("KNOT FOUND");
            story.ChoosePathString(knotName);
            return true;
        }
        catch (StoryException)
        {
            Debug.Log("NO KNOT FOUND");
            return false;
        }
    }
}
