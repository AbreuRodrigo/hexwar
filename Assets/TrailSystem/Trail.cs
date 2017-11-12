using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trail : MonoBehaviour
{
    public void Play(Vector3 origin)
    {
        transform.position = origin;

        Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        target = new Vector3(target.x, target.y, origin.z);

        LeanTween.move(gameObject, new Vector3(target.x, target.y, origin.z), 1)
                 .setOnComplete(OnComplete);
    }

    void OnComplete()
    {
        TrailManager.Instance.FreeObject(this);
    }
}