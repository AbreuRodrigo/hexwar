using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trail : MonoBehaviour {

    void OnComplete()
    {
        TrailManager.Instance.FreeObject(this);
    }

    public void Play(Vector3 origin)
    {
        transform.position = origin;

        Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        target = new Vector3(target.x, target.y, origin.z);

        iTween.MoveBy(gameObject, iTween.Hash(
            "x", target.x,
            "y", target.y,
            "z", origin.z,
            "time", 1f,
            "easetype", "linear",
            "oncompletetarget", gameObject,
            "oncomplete", "OnComplete"
        ));
    }
}