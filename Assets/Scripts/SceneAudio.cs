using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneAudio : MonoBehaviour
{
    public int musicToPlay;
    private bool musicStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!musicStarted) {
            musicStarted = true;
            AudioManager.instance.PlayBackgroundMusic(musicToPlay);
        }        
    }
}
