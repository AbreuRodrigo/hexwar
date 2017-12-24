using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hexwar
{
    public class BasePopUp : MonoBehaviour
    {

        public RectTransform myRect;
        public Image blockBackground;
        public float openTimer = 0.75f;
        public float closeTimer = 0.25f;

        protected virtual void OnEnable()
        {
            blockBackground.gameObject.SetActive(true);
            LeanTween.scale(myRect, Vector3.one, openTimer)
                     .setEaseOutElastic();
        }

        public void ClosePopUp()
        {
            blockBackground.gameObject.SetActive(false);
            LeanTween.scale(myRect, Vector3.zero, closeTimer)
                     .setOnComplete(Disable);
        }

        private void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}