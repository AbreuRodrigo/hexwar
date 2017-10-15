using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionArrow : MonoBehaviour {

	void OnEnable ()
    {
        StartSpinning();
	}

    private void StartSpinning()
    {
        iTween.RotateBy(gameObject, iTween.Hash(
            "z", 1.0f,
            "time", 5f,
            "easetype", "linear",
            "looptype", iTween.LoopType.loop
        ));
        iTween.ScaleBy(gameObject, iTween.Hash(
            "x", 0.9f,
            "y", 0.9f,
            "time", 1f,
            "easetype", "linear",
            "looptype", iTween.LoopType.pingPong
        ));
    }
}