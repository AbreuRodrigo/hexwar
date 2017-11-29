using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILobbyManager : MonoBehaviour {

    public TableWidget gameTable;
    public CreateGamePopUp createGamePopUp;

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

    public void CreateNewGame(GameTemplatePayload gameTemplate)
    {
        if (gameTemplate != null)
        {
            EMapSize mapSize = (EMapSize) System.Enum.Parse(typeof(EMapSize), gameTemplate.mapSize, true);
            gameTable.AddRow(gameTemplate.gameName, mapSize, gameTemplate.currentPlayers, gameTemplate.maxPlayers, true);
        }
    }

    public void EnqueueRowsItem(GameTemplatePayload gameTemplate)
    {
        if (gameTemplate != null)
        {
            EMapSize mapSize = (EMapSize)System.Enum.Parse(typeof(EMapSize), gameTemplate.mapSize, true);
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
}