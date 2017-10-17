using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    public Image fadeImage;

    public void FadeIn()
    {
        if (fadeImage != null)
        {
            iTween.ValueTo(gameObject, iTween.Hash(
                "from", fadeImage.color.a,
                "to", 0,
                "time", 3f,
                "easetype", iTween.EaseType.linear,
                "onupdatetarget", gameObject,
                "onupdate", "OnFading",
                "oncompletetarget", gameObject,
                "oncomplete", "OnCompleteFadingIn"
            ));
        }
    }

    public void FadeOut()
    {
        if (fadeImage != null)
        {
            iTween.ValueTo(gameObject, iTween.Hash(
                "from", fadeImage.color.a,
                "to", 1,
                "time", 3f,
                "easetype", iTween.EaseType.linear,
                "onupdatetarget", gameObject,
                "onupdate", "OnFading",
                "oncompletetarget", gameObject,
                "oncomplete", "OnCompleteFadingOut"
            ));
        }
    }

    private void OnFading(float a)
    {
        Color c = fadeImage.color;
        c.a = a;

        fadeImage.color = c;
    }

    private void OnCompleteFadingIn()
    {
        if (fadeImage != null)
        {
            fadeImage.enabled = false;
        }
    }

    private void OnCompleteFadingOut()
    {
        
    }
}