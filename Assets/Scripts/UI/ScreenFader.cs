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
            LeanTween.value(gameObject, fadeImage.color.a, 0, 2.5f)
                     .setOnUpdate(OnFading)
                     .setOnComplete(OnCompleteFadingIn);
        }
    }

    public void FadeOut()
    {
        if (fadeImage != null)
        {
            Color target = fadeImage.color;
            target.a = 1;

            LeanTween.value(gameObject, fadeImage.color, target, 3)
                     .setOnUpdate(OnFading)
                     .setOnComplete(OnCompleteFadingOut);
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