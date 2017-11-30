using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILobbyManager : MonoBehaviour {

    public TableWidget gameTable;
    public CreateGamePopUp createGamePopUp;
    public JoinConfirmPopUp joinConfirmPopUp;

    private static UILobbyManager instance;
    public static UILobbyManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void CreateGameBtnClick()
    {
        if(createGamePopUp != null)
        {
            createGamePopUp.gameObject.SetActive(true);
        }
    }

    public void JoinGameBtnClick()
    {
        if(joinConfirmPopUp != null)
        {
            joinConfirmPopUp.gameObject.SetActive(true);
        }
    }

    public void CreateNewGame(GameTemplatePayload gameTemplate)
    {
        if (gameTemplate != null)
        {
            EMapSize mapSize = (EMapSize) System.Enum.Parse(typeof(EMapSize), gameTemplate.mapSize);
            gameTable.AddRow(gameTemplate.gameName, mapSize, gameTemplate.currentPlayers, gameTemplate.maxPlayers, true);
        }
    }

    public void EnqueueRowsItem(GameTemplatePayload gameTemplate)
    {
        if (gameTemplate != null)
        {
            EMapSize mapSize = (EMapSize)System.Enum.Parse(typeof(EMapSize), gameTemplate.mapSize);
            gameTable.EnqueueRowItem(gameTemplate.gameName, mapSize, gameTemplate.currentPlayers, gameTemplate.maxPlayers, true);
        }
    }
    
    public string GetGameName()
    {
        if(createGamePopUp == null)
        {
            return "";
        }

        return createGamePopUp.GetGameName();
    }

    public string GetMapSize()
    {
        return createGamePopUp.GetMapSize();
    }
    
    public void AddOnClickLogicsToTableRow(TableRow row)
    {
        row.myButtonBehaviour.onClick.RemoveAllListeners();
        row.myButtonBehaviour.onClick.AddListener(() => {
            JoinGameBtnClick();
            joinConfirmPopUp.SetGameNameInTextBox(row.GameName);
        });
    }
}