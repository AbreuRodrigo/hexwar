using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TableRow : MonoBehaviour {

    public int id;

    public Text gameName;
    public Text mapSize;
    public Text totalPlayers;
    public Image bgImage;

    private GameTemplate gameTemplate;
    
    public void SetInfo(int id, string gameName, EMapSize mapSize, int totalPlayers, Color bgColor)
    {
        if(gameTemplate == null)
        {
            gameTemplate = new GameTemplate();
        }

        gameTemplate.name = gameName;
        gameTemplate.size = mapSize;
        gameTemplate.currenPlayers = 0;
        gameTemplate.totalPlayers = totalPlayers;

        this.id = id;
        this.bgImage.color = bgColor;

        UpdateUI(gameTemplate);
    }

    private void UpdateUI(GameTemplate template)
    {
        gameName.text = template.name;
        mapSize.text = template.size.ToString();
        totalPlayers.text = template.currenPlayers + " / " + template.totalPlayers;
    }
}