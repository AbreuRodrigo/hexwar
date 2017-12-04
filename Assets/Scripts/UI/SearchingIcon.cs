using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SearchingIcon : MonoBehaviour
{
    public Image blockBakaground;
    public Image icon;
 
    public void Show()
    {
        blockBakaground.gameObject.SetActive(true);
        icon.enabled = true;
        LeanTween.moveLocalY(gameObject, 45, 1).setLoopPingPong();
        LeanTween.rotateZ(gameObject, 10, 1).setLoopPingPong();
    }

    public void Hide()
    {
        blockBakaground.gameObject.SetActive(false);
        icon.enabled = false;
    }
}