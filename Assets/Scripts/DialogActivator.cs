using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogActivator : MonoBehaviour
{

    public bool isPerson = true;
    public string[] lines;
    public bool shouldActivateQuest;
    public string questToMark;
    public bool markComplete;

    private bool canActivate = false;


    void Start()
    {
        
    }

    void Update()
    {
        if (canActivate && Input.GetButtonDown("Fire1") && !DialogManager.instance.dialogBox.active) {
            // activate dialog box and update text
            DialogManager.instance.ShowDialog(lines, this.isPerson);
        }

        if (shouldActivateQuest) {
            DialogManager.instance.ShouldActivateQuestAtEnd(questToMark, markComplete);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            canActivate = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
         if (other.tag == "Player") {
            canActivate = false;
        }   
    }
}
