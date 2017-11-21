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

    public void AddRow(string gameName, EMapSize mapSize, int totalPlayers)
    {
        GameObject newRow = Instantiate(tableRowPrefab, tableContent);
        TableRow row = newRow.GetComponent<TableRow>();
        row.SetInfo(rows, gameName, mapSize, totalPlayers, rows % 2 == 0 ? evenRowColor : oddRowColor);

        rows++;
    }
}
