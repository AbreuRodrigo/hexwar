using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Components")]
    public ScreenFader screenFader;

    [Header("Elements")]
    public Text playerActionsText;
    public Text playerTroopText;
    public Text playerLevelText;
    public Text currentTurnText;
    public Text turnTimer;

    [Header("Buttons")]
    public UIButton turnActionBtn;
    public Button moveMapBtn;

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

    public void UpdateTopUI(Player player, int currentTurn)
    {
        if(player != null)
        {
            SetPlayerActionsUI(player.actions.ToString());
            SetPlayerTroopUI(player.troop.ToString());
            SetPlayerLevelUI(player.level.ToString());
            SetCurrentTurnUI(currentTurn.ToString());
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
    
    public void UpdateUiForMaintenancePhase()
    {
        screenBlocker.gameObject.SetActive(false);
        turnActionBtn.gameObject.SetActive(true);
        turnActionBtn.SetInteractivity(true);
        turnActionBtn.ChangeToSkipState();
    }

    public void UpdateUiForCombatOrExplorationPhase()
    {
        screenBlocker.gameObject.SetActive(false);
        turnActionBtn.gameObject.SetActive(true);
        turnActionBtn.SetInteractivity(true);
        turnActionBtn.ChangeToSkipState();
        turnTimer.gameObject.SetActive(true);
        moveMapBtn.gameObject.SetActive(true);
    }

    public void UpdateUiForClearPhase()
    {
        turnActionBtn.ChangeToEndState();
    }

    public void UpdateUiForWaitPhase()
    {
        turnActionBtn.gameObject.SetActive(false);
        screenBlocker.gameObject.SetActive(true);
        turnTimer.gameObject.SetActive(false);
        moveMapBtn.gameObject.SetActive(false);
    }

    public void SetTurnTimer(int timer)
    {
        string timerFormat1 = "00:0{0}";
        string timerFormat2 = "00:{0}";
        string format = timer >= 10 ? timerFormat2 : timerFormat1;

        turnTimer.text = string.Format(format, timer);
    }

    private void SetTextUIValue(Text textUI, string label, string value)
    {
        if(textUI != null)
        {
            textUI.text = label + value;
        }
    }
}