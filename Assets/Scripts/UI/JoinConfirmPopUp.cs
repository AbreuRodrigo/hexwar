using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinConfirmPopUp : BasePopUp
{
    public Text textBox;

    private const string TEXT_BOX_CONFIRM_TXT = "Do you really want to join {0}?";

    public void SetGameNameInTextBox(string gameName)
    {
        if(textBox != null)
        {
            textBox.text = string.Format(TEXT_BOX_CONFIRM_TXT, gameName);
        }
    }
}