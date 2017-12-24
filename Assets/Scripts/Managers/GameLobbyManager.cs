using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hexwar
{
    public class GameLobbyManager : MonoBehaviour
    {
        private void Awake()
        {
            Application.runInBackground = true;            
        }

        private void Start()
        {
            AudioManager.Instance.PlayMenuAmbience();
            AudioManager.Instance.PlayMainTheme();
        }

        [System.Obsolete]
        public void OnCreateGameButtonClick()
        {
            //Deprecated the functionality
        }
    }
}