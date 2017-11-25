using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingIcon : MonoBehaviour {

    public Image blockBakaground;
    public Image loadingIcon;
    
    public void Show()
    {
        blockBakaground.gameObject.SetActive(true);
        loadingIcon.enabled = true;
        LeanTween.rotateZ(gameObject, 180, 1).setLoopType(LeanTweenType.linear);
    }

    public void Hide()
    {
        blockBakaground.gameObject.SetActive(false);
        loadingIcon.enabled = false;
    }
}
