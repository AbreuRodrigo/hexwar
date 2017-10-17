using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSelection : MonoBehaviour {

    private void OnEnable()
    {
        gameObject.transform.localScale = Vector3.zero;

        iTween.ScaleTo(gameObject, iTween.Hash(
            "scale", new Vector3(1, 1, 1),
            "time", 0.8f,
            "easetype", iTween.EaseType.easeInOutElastic
        ));
        /*iTween.MoveTo(gameObject, iTween.Hash(
            "x", 0,
            "y", 0.7f,
            "time", 0.5f,
            "easetype", iTween.EaseType.linear
        ));*/
    }
}
