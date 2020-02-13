using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaExit : MonoBehaviour
{

public string areaToLoad;

    void Start()
    {

    }

    void OnTriggerEnter2D(Collider2D other) {
        
        if (other.tag == "Player") {
            SceneManager.LoadScene(areaToLoad);
        }

    }

    void Update()
    {
        
    }
}
