using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEntrance : MonoBehaviour
{

    public string transitionName;

    // todo: use a combination of enums and a config file to replace the transition names
    void Start()
    {
        if (this.transitionName == PlayerController.instance.areaTransitionName) {
            PlayerController.instance.transform.position = gameObject.transform.position;
        }

        UIFade.instance.FadeFromBlack();
        GameManager.instance.inSceneTransition = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
