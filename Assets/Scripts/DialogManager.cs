using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{

    public Text dialogText;
    public Text nameText;    
    public GameObject dialogBox;
    public GameObject nameBox;

    public string[] dialogLines;

    public int currentLine;

    public static DialogManager instance;

    private bool justStarted = false;
    private string questToMark;
    private bool markQuestComplete;
    private bool shouldMarkQuest;

    void Start()
    {
        instance = this;
    }

    public void ShouldActivateQuestAtEnd(string questName, bool markComplete)
    {
        questToMark = questName;
        shouldMarkQuest = true;
        markQuestComplete = markComplete;
    }

    void NextLine() {
        if ((currentLine + 1) < dialogLines.Length) {
            currentLine++;
            CheckIfName();
            dialogText.text = dialogLines[currentLine];
        } else {

            dialogBox.SetActive(false);
            GameManager.instance.isDialogActive = false;

            if (shouldMarkQuest) {
                shouldMarkQuest = false;
                QuestManager.instance.SetQuestCompletion(questToMark, markQuestComplete);
            }
        }
    }

    void Update()
    {

        if (dialogBox.active) {
            // That Fire1 should really be moved into a config file and replaced with a generic variable
            if (Input.GetButtonUp("Fire1")) {
                if (!justStarted) {
                    NextLine();
                } else {
                    justStarted = false;
                }
            }
        }

    }

    public void CheckIfName() {
        if (dialogLines[currentLine].StartsWith("n-")) {
            nameText.text = dialogLines[currentLine].Substring(2);
            currentLine++;    
        }
    }

    public void ShowDialog(string[] newLines, bool isPerson)
    {
        dialogLines = newLines;
        currentLine = 0;
        
        CheckIfName();
        dialogText.text = dialogLines[currentLine];

        nameBox.SetActive(isPerson);
        
        justStarted = true;
        dialogBox.SetActive(true);
        GameManager.instance.isDialogActive = true;
    }
}
