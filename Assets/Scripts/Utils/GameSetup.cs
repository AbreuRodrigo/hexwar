using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetup : MonoBehaviour
{
    public static long mapSeed = 1512341350967;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}