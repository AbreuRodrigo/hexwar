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
    public SearchingIcon searching;
    public BlinkingText waitingOpponents;

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

    public void ShowSearching()
    {
        if (searching != null)
        {
            searching.Show();
        }
    }

    public void HideSearching()
    {
        if(searching != null)
        {
            searching.Hide();
        }
    }

    public void ShowWaitingOpponentns()
    {
        if (waitingOpponents != null)
        {
            waitingOpponents.Show();
        }
    }

    public void HideWaitingOpponentns()
    {
        if (waitingOpponents != null)
        {
            waitingOpponents.Hide();
        }
    }

    public void SearchGame()
    {
        StartCoroutine(StartSearchingGameRoutine());
    }

    [System.Obsolete]
    public void CreateGameBtnClick()
    {
    }

    [System.Obsolete]
    public void JoinGameBtnClick()
    {
    }

    [System.Obsolete]
    public void CreateNewGame(GameTemplatePayload gameTemplate)
    {
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

    IEnumerator StartSearchingGameRoutine()
    {
        if (gameMapDropdown != null && !isSearchingGame)
        {
            ShowSearching();

            isSearchingGame = true;
            int selectedOption = gameMapDropdown.value;

            yield return new WaitForSecondsRealtime(3);

            NetworkManager.Instance.SearchGame(selectedOption);
        }
    }

    IEnumerator StartLoadingScreenRoutine()
    {
        Instance.ShowLoading();

        while (NetworkManager.Instance.isLoading)
        {
            yield return null;
        }

        Instance.HideLoading();
    }
}