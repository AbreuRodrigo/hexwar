using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetup : MonoBehaviour {

    public static long mapSeed = 0;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}