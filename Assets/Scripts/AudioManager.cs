using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    // public variables
    public AudioSource[] sfx;
    public AudioSource[] bgMusic;
    public static AudioManager instance;

    void Start()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void PlaySFX(int sound)
    {
        if (sound < sfx.Length) {
            sfx[sound].Play();
        }
    }

    public void PlayBackgroundMusic(int music)
    {
        if (music < bgMusic.Length || music < 0) {
            if (!bgMusic[music].isPlaying) {
                StopMusic();
                bgMusic[music].Play();
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
