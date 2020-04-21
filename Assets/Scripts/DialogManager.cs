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

    private bool justStarted = false;

    public static DialogManager instance;

    void Start()
    {
        instance = this;
    }

    void NextLine() {
        if ((currentLine + 1) < dialogLines.Length) {
            currentLine++;
            CheckIfName();
            dialogText.text = dialogLines[currentLine];
        } else {
            dialogBox.SetActive(false);
            PlayerController.instance.movementEnabled = true;
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
        PlayerController.instance.movementEnabled = false;
    }
}
