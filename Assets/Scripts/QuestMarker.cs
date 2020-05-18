using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestMarker : MonoBehaviour
{

    public string questName;
    public bool isComplete;
    public bool markOnEnter;
    private bool canMarkQuest;
    public bool deactiveOnMarking;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canMarkQuest && Input.GetButtonDown("Fire1")) {
            canMarkQuest = false;
            MarkQuest();
        }
    }

    public void MarkQuest()
    {
        QuestManager.instance.SetQuestCompletion(questName, isComplete);
        gameObject.SetActive(!deactiveOnMarking);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {

            if (markOnEnter) {
                MarkQuest();
            } else {
                canMarkQuest = true;
            }
            
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player") {
            canMarkQuest = false;
        }
    }
}
