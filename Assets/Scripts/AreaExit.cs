using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaExit : MonoBehaviour
{

    public string areaToLoad;

	public string areaTransitionName;
    public AreaEntrance entrance;
    void Start()
    {
        entrance.transitionName = this.areaTransitionName;
    }

    void OnTriggerEnter2D(Collider2D other) {
        
        if (other.tag == "Player") {
            SceneManager.LoadScene(areaToLoad);
            PlayerController.instance.areaTransitionName = this.areaTransitionName;
        }

    }

    void Update()
    {
        
    }
}
