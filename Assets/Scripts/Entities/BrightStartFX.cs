/*===============================================================
Product:    Hexwar
Developer:  Abreu
Company:    TerraNix Studios - https://www.terranix.com
Date:       26/12/2017 23:26
================================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hexwar
{
	public class BrightStartFX : MonoBehaviour 
	{
        public RectTransform myRectTransform;
        public float timer = 0.5f;

        public void Play()
        {
            LeanTween.scale(myRectTransform, Vector3.one, timer).setLoopPingPong(1).setOnComplete(ResetFX);
        }

        private void ResetFX()
        {
            myRectTransform.localScale = Vector3.zero;
        }
	}
}