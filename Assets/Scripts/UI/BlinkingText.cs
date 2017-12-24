using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hexwar
{
    public class BlinkingText : MonoBehaviour
    {
        public Image blockBakaground;
        public Text text;

        void Start()
        {
            LeanTween.textColor(text.rectTransform, new Color(0.25f, 0.25f, 0.25f), 1).setLoopPingPong();
        }

        public void Show()
        {
            blockBakaground.gameObject.SetActive(true);
            text.enabled = true;
        }

        public void Hide()
        {
            blockBakaground.gameObject.SetActive(false);
            text.enabled = false;
        }
    }
}