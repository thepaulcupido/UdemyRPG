using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaExit : MonoBehaviour
{

    public string areaToLoad;

	public string areaTransitionName;
    public AreaEntrance entrance;
    public float waitToLoad = 1f;

    private bool loadAfterFade = false;

    void Start()
    {
        entrance.transitionName = this.areaTransitionName;
    }

    void OnTriggerEnter2D(Collider2D other) {
        
        if (other.tag == "Player") {
            // SceneManager.LoadScene(areaToLoad);
            loadAfterFade = true;
            GameManager.instance.inSceneTransition = true;
            // need to disable user input while this transition is happening

            UIFade.instance.FadeToBlack();
            PlayerController.instance.areaTransitionName = this.areaTransitionName;
        }

    }

    void Update()
    {
        if (loadAfterFade) {
            waitToLoad -= Time.deltaTime;
            if (waitToLoad <= 0f) {
                loadAfterFade = false;
                waitToLoad = 1f;
                SceneManager.LoadScene(areaToLoad);
            }
        }
    }
}
