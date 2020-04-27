using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public CharacterStats[] playerStats;
    public bool gameMenuOpen, isDialogActive, inSceneTransition;

    void Start()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameMenuOpen || isDialogActive || inSceneTransition) {
            PlayerController.instance.movementEnabled = false;
        } else {
            PlayerController.instance.movementEnabled = true;
        }

        //PlayerController.instance.movementEnabled = !(gameMenuOpen || isDialogActive || inSceneTransition);
    }
}
