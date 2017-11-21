using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILobbyManager : MonoBehaviour {

    public TableWidget gameTable;
    
    int counter;

    public void CreateGameBtnClick()
    {
        counter++;

        gameTable.AddRow("Game" + counter, EMapSize.Large, 1);
    }
}
