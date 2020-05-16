using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestObjectActivator : MonoBehaviour
{
    public GameObject objectToActivate;
    public string questName;
    public bool activeIfComplete = false;

    private bool isInitialised = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInitialised) {
            isInitialised = true;
            CheckCompletion();
        }
    }

    public void CheckCompletion()
    {
        if (QuestManager.instance.IsQuestComplete(questName)) {
            objectToActivate.SetActive(activeIfComplete);
        }
    }
}
