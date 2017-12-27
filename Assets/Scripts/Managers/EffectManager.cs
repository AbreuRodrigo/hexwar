/*===============================================================
Product:    Hexwar
Developer:  Abreu
Company:    TerraNix Studios - https://www.terranix.com
Date:       26/12/2017 23:43
================================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hexwar
{
	public class EffectManager : Singleton<EffectManager>
	{
        private int brightStarsIndex = 0;
        private Dictionary<int, BrightStartFX> brightStarFXs = new Dictionary<int, BrightStartFX>();
        private bool runningBrightStars = false;
        
        void Start()
        {
            BrightStartFX[] fxs = FindObjectsOfType<BrightStartFX>();

            if (fxs != null)
            {
                runningBrightStars = true;

                foreach (BrightStartFX fx in fxs)
                {
                    RegisterBrightStartFX(fx);
                }

                StartCoroutine(InitializeBrightStarFXControl());
            }
            else if(runningBrightStars)
            {
                runningBrightStars = false;
            }
        }

        private void RegisterBrightStartFX(BrightStartFX brightStarFX)
        {
            if(brightStarFX != null)
            {
                brightStarFXs.Add(brightStarsIndex, brightStarFX);
                brightStarsIndex++;
            }
        }

        private IEnumerator InitializeBrightStarFXControl()
        {
            int nextBrightStar = 0;
            float waitingSeconds = 0;
            
            while(runningBrightStars)
            {
                waitingSeconds = Random.Range(2, 4);

                yield return new WaitForSeconds(waitingSeconds);

                BrightStartFX fx = brightStarFXs[nextBrightStar];
                fx.Play();

                nextBrightStar = Random.Range(0, brightStarsIndex);                
            }
        }
    }
}