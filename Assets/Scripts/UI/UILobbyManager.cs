using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILobbyManager : MonoBehaviour {

    public TableWidget gameTable;
    public CreateGamePopUp createGamePopUp;
    public JoinConfirmPopUp joinConfirmPopUp;
    public Dropdown gameMapDropdown;
    public LoadingIcon loading;

    private bool isSearchingGame = false;

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

    public void ShowLoading()
    {
        if(loading != null)
        {
            loading.Show();
        }
    }

    public void HideLoading()
    {
        if (loading != null)
        {
            loading.Hide();
        }
    }

    public void SearchGame()
    {
        if(gameMapDropdown != null && !isSearchingGame)
        {
            ShowLoading();

            isSearchingGame = true;
            int selectedOption = gameMapDropdown.value;

            NetworkManager.Instance.SearchGame(selectedOption);            
        }
    }

    [System.Obsolete]
    public void CreateGameBtnClick()
    {
        if(createGamePopUp != null)
        {
            createGamePopUp.gameObject.SetActive(true);
        }
    }

    [System.Obsolete]
    public void JoinGameBtnClick()
    {
        if(joinConfirmPopUp != null)
        {
            joinConfirmPopUp.gameObject.SetActive(true);
        }
    }

    [System.Obsolete]
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

    public void StartLoadingScreen()
    {
        StartCoroutine(StartLoadingScreenRoutine());
    }

    IEnumerator StartLoadingScreenRoutine()
    {
        UILobbyManager.Instance.ShowLoading();

        while (NetworkManager.Instance.isLoading)
        {
            yield return null;
        }

        UILobbyManager.Instance.HideLoading();
    }
}