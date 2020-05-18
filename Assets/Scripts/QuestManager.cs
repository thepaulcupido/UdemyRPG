using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{

    // This should really be populated by some tool and stored i na database
    public string[] questMarkerNames;
    public bool[] questMarkerComplete;

    public static QuestManager instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        questMarkerComplete = new bool[questMarkerNames.Length];
    }

    // Update is called once per frame
    void Update()
    {
        if (questMarkerComplete == null) {
            questMarkerComplete = new bool[questMarkerNames.Length];
        }

        if (Input.GetKeyUp(KeyCode.Q)) {

            if (IsQuestComplete("Quest test")) {
                SetQuestCompletion("Quest test", false);
            } else {
                SetQuestCompletion("Quest test", true);
            }
        }

        if (Input.GetKeyUp(KeyCode.O)) {
            SaveQuestData();
        }

        if (Input.GetKeyUp(KeyCode.P)) {
            LoadQuestData();
        }
    }

    public int GetQuestIndex(string questName)
    {

        for (int i = 0; i < questMarkerNames.Length; i ++) {
            if (questMarkerNames[i] == questName) {
                return i;
            }
        }

        Debug.Log("Quest: " + questName + " does not exist");

        return -1;
    }

    public bool IsQuestComplete(string questName)
    {
        int questIndex = GetQuestIndex(questName);

        if (questIndex >= 0) {
            return questMarkerComplete[questIndex];
        }

        return false;
    }


    // These two methods could easily be merged into a single SetQuestCompletion(string questName, bool isComplete)

    public void SetQuestCompletion(string questName, bool isComplete)
    {
        int questIndex = GetQuestIndex(questName);

        if (questIndex >= 0) {
            questMarkerComplete[questIndex] = isComplete;

            UpdateLocalQuestObjects();
        }
    }

    public void CompleteQuest(string questName)
    {
        SetQuestCompletion(questName, true);
    }

    public void SetQuestActive(string questName)
    {
        SetQuestCompletion(questName, false);
    }

    public void UpdateLocalQuestObjects()
    {
        // Get all QuestObjectActivators
        QuestObjectActivator[] questObjects = FindObjectsOfType<QuestObjectActivator>();

        for (int i =0 ; i < questObjects.Length; i++) {
            questObjects[i].CheckCompletion();
        }
    }

    public void SaveQuestData()
    {
        for (int i = 0; i < questMarkerNames.Length; i++) {
            PlayerPrefs.SetInt("QuestMarker_" + questMarkerNames[i], (questMarkerComplete[i] ? 1 : 0));
        }
    }

    public void LoadQuestData()
    {
        int value;
        for (int i = 0; i < questMarkerNames.Length; i++) {
            value = 0;

            if (PlayerPrefs.HasKey("QuestMarker_" + questMarkerNames[i])) {
                value = PlayerPrefs.GetInt("QuestMarker_" + questMarkerNames[i]);
            }

            questMarkerComplete[i] = (value != 0);
        }

    }
}
