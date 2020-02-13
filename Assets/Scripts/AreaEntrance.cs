using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEntrance : MonoBehaviour
{

    public string transitionName;

    // Start is called before the first frame update
    void Start()
    {
        if (!string.IsNullOrEmpty(PlayerController.instance.areaTransitionName) && this.transitionName == PlayerController.instance.areaTransitionName) {
            PlayerController.instance.transform.position = gameObject.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
