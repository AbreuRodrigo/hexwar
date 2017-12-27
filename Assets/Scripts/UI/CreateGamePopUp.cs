using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hexwar
{
    public class CreateGamePopUp : BasePopUp
    {
        public InputField gameNameInputField;
        public Dropdown mapSizeDropDown;

        private void OnDisable()
        {
            ClosePopUp();
        }

        public string GetGameName()
        {
            string gameName = "";

            if (gameNameInputField != null)
            {
                gameName = gameNameInputField.text;
            }

            return gameName;
        }

        public string GetMapSize()
        {
            return EMapSize.GIANT.ToString();
        }
    }
}