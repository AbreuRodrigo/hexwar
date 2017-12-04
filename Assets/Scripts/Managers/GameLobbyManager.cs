using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLobbyManager : MonoBehaviour
{
    private void Awake()
    {
        Application.runInBackground = true;
    }

    [System.Obsolete]
    public void OnCreateGameButtonClick()
    {
        //Deprecated the functionality
    }
}