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

    void Start()
    {
        if (dialogLines.Length > 0) {
            dialogText.text = dialogLines[0];
        }

    }

    void NextLine() {
        if ((currentLine + 1) < dialogLines.Length) {
            currentLine++;
            dialogText.text = dialogLines[currentLine];
        } else {
            dialogBox.SetActive(false);
        }
    }

    void Update()
    {

        if (dialogBox.active) {
            // That Fire1 should really be moved into a config file and replaced with a generic variable
            if (Input.GetButtonUp("Fire1")) {
                NextLine();
            }
        }

    }
}
