using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceLoader : MonoBehaviour
{

    public GameObject UIScreen;
    public GameObject Player;
    public GameObject gameManager;
    public GameObject audioManager;

    // Start is called before the first frame update
    void Awake()
    {
        if (PlayerController.instance == null) {
            PlayerController clone = Instantiate(Player).GetComponent<PlayerController>();
            PlayerController.instance = clone;
        }

        if (UIFade.instance == null) {
            UIFade.instance = Instantiate(UIScreen).GetComponent<UIFade>();
        }
        if (AudioManager.instance == null) {
            AudioManager.instance = Instantiate(audioManager).GetComponent<AudioManager>();
        }

        if (GameManager.instance == null) {
            Instantiate(gameManager);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
