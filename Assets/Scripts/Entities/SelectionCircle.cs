using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionCircle : MonoBehaviour {

	void OnEnable ()
    {
        StartSpinning();
	}

    private void StartSpinning()
    {
        Vector3 intialScale = gameObject.transform.localScale;

        gameObject.transform.localScale = new Vector3(0, 0, 1);

        LeanTween.scale(gameObject, new Vector3(0.7f, 0.7f, 1), 0.5f)
                 .setEaseInOutElastic()
                 .setOnComplete(()=> {
                     gameObject.transform.localScale = intialScale;

                     LeanTween.scale(gameObject, new Vector3(0.75f, 0.75f, 1), 0.7f)
                              .setLoopPingPong();
                 });

        LeanTween.rotateZ(gameObject, 180, 5)
                 .setLoopCount(-1);
    }
}