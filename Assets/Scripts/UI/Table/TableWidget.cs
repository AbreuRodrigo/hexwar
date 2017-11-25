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

    private int rows = 0;

    private bool createNewGameRow = false;
    private GameTemplate gameTemplateRef = new GameTemplate();

    private void Start()
    {
        StartCoroutine(CheckCreateNewGameRow());
    }

    public void AddRow(string gameName, EMapSize mapSize, int totalPlayers, bool createdByLocalPlayer)
    {
        gameTemplateRef.name = gameName;
        gameTemplateRef.size = mapSize;
        gameTemplateRef.currenPlayers = totalPlayers;
        gameTemplateRef.createdByLocalPlayer = createdByLocalPlayer;

        createNewGameRow = true;
    }

    private IEnumerator CheckCreateNewGameRow()
    {
        while(true)
        {
            yield return new WaitForSecondsRealtime(3);

            if(createNewGameRow)
            {                
                GameObject newRow = Instantiate(tableRowPrefab, tableContent);
                TableRow row = newRow.GetComponent<TableRow>();
                row.SetInfo(rows, gameTemplateRef.name, gameTemplateRef.size, gameTemplateRef.currenPlayers, 
                    rows % 2 == 0 ? evenRowColor : oddRowColor, gameTemplateRef.createdByLocalPlayer);

                rows++;

                createNewGameRow = false;
            }
        }        
    }
}
