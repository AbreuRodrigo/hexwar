using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TableWidget : MonoBehaviour {

    [Header("Prefabs")]
    public GameObject tableRowPrefab;

    [Header("Components")]
    public RectTransform tableContent;

    [Header("Rows Colors")]
    public Color evenRowColor;
    public Color oddRowColor;

    private bool createNewGameRow = false;
    private GameTemplate gameTemplateRef = new GameTemplate();

    private Queue<GameTemplate> gameQueue = new Queue<GameTemplate>();
    private bool dequeuing = false;

    private void Start()
    {
        StartCoroutine(CheckCreateNewGameRow());
        StartCoroutine(DequeueRows());
    }

    public void AddRow(string gameName, EMapSize mapSize, int currentPlayers, int totalPlayers, bool createdByLocalPlayer)
    {
        gameTemplateRef.name = gameName;
        gameTemplateRef.size = mapSize;
        gameTemplateRef.currenPlayers = currentPlayers;
        gameTemplateRef.totalPlayers = totalPlayers;
        gameTemplateRef.createdByLocalPlayer = createdByLocalPlayer;

        createNewGameRow = true;
    }

    public void EnqueueRowItem(string gameName, EMapSize mapSize, int currentPlayers, int totalPlayers, bool createdByLocalPlayer)
    {
        GameTemplate gameTemplate = new GameTemplate();
        gameTemplate.name = gameName;
        gameTemplate.size = mapSize;
        gameTemplate.currenPlayers = currentPlayers;
        gameTemplate.totalPlayers = totalPlayers;
        gameTemplate.createdByLocalPlayer = createdByLocalPlayer;

        gameQueue.Enqueue(gameTemplate);
    }
    
    private IEnumerator DequeueRows()
    {
        if(!dequeuing)
        {
            dequeuing = true;
            int rows = 0;

            while (true)
            {
                if (gameQueue.Count > 0)
                {
                    GameTemplate gt = gameQueue.Dequeue();
                    GameObject newRow = Instantiate(tableRowPrefab, tableContent);
                    TableRow row = newRow.GetComponent<TableRow>();
                    row.SetInfo(rows, gt.name, gt.size, gt.currenPlayers,
                        gt.totalPlayers, rows % 2 == 0 ? evenRowColor : oddRowColor, gt.createdByLocalPlayer);

                    UILobbyManager.Instance.AddOnClickLogicsToTableRow(row);

                    rows++;
                }

                yield return null;
            }
        }
    }

    private IEnumerator CheckCreateNewGameRow()
    {
        int rows = 0;

        while (true)
        {
            if (createNewGameRow)
            {                
                GameObject newRow = Instantiate(tableRowPrefab, tableContent);
                TableRow row = newRow.GetComponent<TableRow>();
                row.SetInfo(rows, gameTemplateRef.name, gameTemplateRef.size, gameTemplateRef.currenPlayers,
                    gameTemplateRef.totalPlayers, rows % 2 == 0 ? evenRowColor : oddRowColor, gameTemplateRef.createdByLocalPlayer);

                UILobbyManager.Instance.AddOnClickLogicsToTableRow(row);

                rows++;

                createNewGameRow = false;
            }

            yield return null;
        }        
    }
}