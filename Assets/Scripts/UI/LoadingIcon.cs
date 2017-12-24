using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hexwar
{
    public class LoadingIcon : MonoBehaviour
    {

        public Image blockBakaground;
        public Image loadingIcon;

        void Start()
        {
            LeanTween.rotateZ(gameObject, 180, 1).setLoopType(LeanTweenType.linear);
        }

        public void Show()
        {
            blockBakaground.gameObject.SetActive(true);
            loadingIcon.enabled = true;
        }

        public void Hide()
        {
            blockBakaground.gameObject.SetActive(false);
            loadingIcon.enabled = false;
        }
    }
}