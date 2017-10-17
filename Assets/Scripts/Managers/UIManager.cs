using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Components")]
    public ScreenFader screenFader;

    [Header("Elements")]
    public Text playerColorText;
    public Text playerTroopText;
    public Text playerLevelText;
    public Text currentTurnText;

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
            SetPlayerNameUI(player.playerName);
            SetPlayerTroopUI(player.troop.ToString());
            SetPlayerLevelUI(player.level.ToString());
        }
    }

    public void SetPlayerNameUI(string playerName)
    {
        SetTextUIValue(playerColorText, "Player: ", playerName);
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

    private void SetTextUIValue(Text textUI, string label, string value)
    {
        if(textUI != null)
        {
            textUI.text = label + value;
        }
    }
}
