using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private const string TURN_ACTION_SKIP = "Skip Turn";
    private const string TURN_ACTION_END = "End Turn";

    [Header("Components")]
    public ScreenFader screenFader;

    [Header("Elements")]
    public Text playerActionsText;
    public Text playerTroopText;
    public Text playerLevelText;
    public Text currentTurnText;

    [Header("Buttons")]
    public UIButton turnActionBtn;

    [Header("Images")]
    public Image screenBlocker;

    void Start()
    {
        if(screenFader != null)
        {
            screenFader.gameObject.SetActive(true);
            screenFader.FadeIn();
        }
    }

    public void UpdatePlayerUI(Player player)
    {
        if(player != null)
        {
            SetPlayerActionsUI(player.actions.ToString());
            SetPlayerTroopUI(player.troop.ToString());
            SetPlayerLevelUI(player.level.ToString());
        }
    }

    public void SetPlayerActionsUI(string playerActions)
    {
        SetTextUIValue(playerActionsText, "Actions: ", playerActions);
    }

    public void SetPlayerTroopUI(string troop)
    {
        SetTextUIValue(playerTroopText, "Troop: ", troop);
    }

    public void SetPlayerLevelUI(string level)
    {
        SetTextUIValue(playerLevelText, "Level: ", level);
    }

    public void SetCurrentTurnUI(string turn)
    {
        SetTextUIValue(currentTurnText, "Turn: ", turn);
    }

    public void UpdateInterfaceByGamePhase(EGamePhase phase)
    {
        switch (phase)
        {                
            case EGamePhase.MaintenancePhase:
                screenBlocker.gameObject.SetActive(false);
                turnActionBtn.gameObject.SetActive(true);
                turnActionBtn.SetInteractivity(true);
                turnActionBtn.SetText(TURN_ACTION_SKIP);
                break;
            case EGamePhase.CombatOrExplorationPhase:
                screenBlocker.gameObject.SetActive(false);
                turnActionBtn.gameObject.SetActive(true);
                turnActionBtn.SetInteractivity(true);
                turnActionBtn.SetText(TURN_ACTION_SKIP);
                break;
            case EGamePhase.ClearPhase:
                turnActionBtn.SetText(TURN_ACTION_END);
                break;
            case EGamePhase.WaitPhase:
                turnActionBtn.gameObject.SetActive(false);
                screenBlocker.gameObject.SetActive(true);
                break;
        }
    }

    private void SetTextUIValue(Text textUI, string label, string value)
    {
        if(textUI != null)
        {
            textUI.text = label + value;
        }
    }
}