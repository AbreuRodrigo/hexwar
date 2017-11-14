using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{
    public Button button;
    public Text buttonText;
    public ETurnActionButtonState state;

    public void SetText(string text)
    {
        buttonText.text = text;
    }

    public void SetInteractivity(bool interactible)
    {
        if(button != null)
        {
            button.interactable = interactible;
        }
    }

    public void PressButtonAction()
    {
        if(ETurnActionButtonState.Skip == state)
        {
            GameManager.Instance.SkipTurn();
        }
        else if(ETurnActionButtonState.End == state)
        {
            GameManager.Instance.EndTurn();
        }
    }
}