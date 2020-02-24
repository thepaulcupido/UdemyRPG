using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFade : MonoBehaviour
{

    public static UIFade instance;
    public float fadeSpeed = 1f;
    public Image fadeScreen;

    private bool fadeToBlack = false;
    private bool fadeFromBlack = false;
    private Color fadeScreenColor;

    void Start() {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update() {
        fadeScreenColor = fadeScreen.color;

        if (fadeToBlack) {
            fadeScreen.color = new Color(fadeScreenColor.r, fadeScreenColor.g, fadeScreenColor.b, Mathf.MoveTowards(fadeScreenColor.a, 1f, fadeSpeed * Time.deltaTime));
            fadeToBlack = !(fadeScreen.color.a == 1f);
        }
        if (fadeFromBlack) {
            fadeScreen.color = new Color(fadeScreenColor.r, fadeScreenColor.g, fadeScreenColor.b, Mathf.MoveTowards(fadeScreenColor.a, 0f, fadeSpeed * Time.deltaTime));
            fadeFromBlack = !(fadeScreen.color.a == 0f);
        }
        
    }

    public void FadeToBlack() {
        this.fadeToBlack = true;
        this.fadeFromBlack = false;
    }

    public void FadeFromBlack() {
        this.fadeToBlack = false;
        this.fadeFromBlack = true;
    }
}
