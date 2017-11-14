using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{
    private const string TURN_ACTION_SKIP = "Skip Turn";
    private const string TURN_ACTION_END = "End Turn";

    public Button button;
    public Text buttonText;
    public ETurnActionButtonState state;

    [Header("Colors")]
    public Color skipTurnColor;
    public Color endTurnColor;

    public void ChangeToSkipState()
    {
        state = ETurnActionButtonState.Skip;
        button.image.color = skipTurnColor;
        buttonText.text = TURN_ACTION_SKIP;
    }

    public void ChangeToEndState()
    {
        state = ETurnActionButtonState.End;
        button.image.color = endTurnColor;
        buttonText.text = TURN_ACTION_END;
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