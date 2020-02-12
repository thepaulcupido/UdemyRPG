using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaExit : MonoBehaviour
{

public string areaToLoad;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void onTriggerEnter2D(Collider2D other) {
        
        if (other.tag == "Player") {
            SceneManager.LoadScene(areaToLoad);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
