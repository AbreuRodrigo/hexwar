using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hexwar
{
    public class SelectionCircle : MonoBehaviour
    {
        void Start()
        {
            LeanTween.rotateZ(gameObject, 180, 5).setLoopClamp();
        }

        void OnEnable()
        {
            StartSpinning();
        }

        private void StartSpinning()
        {
            gameObject.transform.localScale = Vector3.zero;
            LeanTween.scale(gameObject, new Vector3(0.7f, 0.7f, 1), 0.5f).setEaseInOutElastic();
        }
    }
}