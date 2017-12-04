using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Randomizing : MonoBehaviour {

    public Text text;

	// Use this for initialization
	void Start () {
        StartCoroutine(Run());
	}
	
	IEnumerator Run()
    {
        yield return new WaitForSecondsRealtime(0);

        long seed = 15123;
        Random.InitState((int)seed);

        int i = 5;

        string s = "";

        while (i > 0)
        {
            s += Random.Range(0, 10) + ", ";
            i--;
        }

        text.text = s;
    }
}
