/*===============================================================
Product:    HexWar
Developer:  Abreu
Company:    TerraNix Studios - https://www.terranix.com
Date:       23/12/2017 17:09
================================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexWar
{
	public class MenuBG : MonoBehaviour 
	{
        [Header("Config")]
        public float delayBeforeZoom = 2;
        public float delayAfterZoom = 0;
        public float zoomDuration = 2;
        public Vector3 initialSize;
        public Vector3 finalSize;

        void Start () 
		{
            transform.localScale = initialSize;
            StartCoroutine(ZoomIn());
		}

        private IEnumerator ZoomIn()
        {
            yield return new WaitForSeconds(delayBeforeZoom);

            float elapsedTime = 0;
            Vector3 startingPos = transform.localScale;

            while (elapsedTime < zoomDuration)
            {
                transform.localScale = Vector3.Lerp(startingPos, finalSize, (elapsedTime / zoomDuration));
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            transform.localScale = finalSize;

            yield return new WaitForSeconds(delayAfterZoom);
        }
	}
}