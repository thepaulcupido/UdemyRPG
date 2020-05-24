using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public AudioSource[] sfx;
    public AudioSource[] bgMusic;

    public static AudioManager instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.T)) {
        //     PlaySFX(0);
        // }
    }

    public void PlaySFX(int sound)
    {
        if (sound < sfx.Length) {
            // sfx[sound].Play();
        }
    }

    public void PlayBackgroundMusic(int music)
    {
        if (music < bgMusic.Length || music < 0) {
            if (!bgMusic[music].isPlaying) {
                StopMusic();
                bgMusic[music].Play();
            } else {
                print("Music continues between scenes");
            }
        } else {
            Debug.Log("Error: int variable supplied lies out of soundtrack index");
        }
    }

    public void StopMusic()
    {
        for (int i = 0; i < bgMusic.Length; i ++) {
            bgMusic[i].Stop();
        }
    }
}
