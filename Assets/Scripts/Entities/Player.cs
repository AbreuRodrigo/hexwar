using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Dictionary<int, Hexagon> hexagons = new Dictionary<int, Hexagon>();

    public string playerName;
    public int actions = 2;
    public int initialActions = 2;
    public int troop = 7;
    public int totalHexLands = 0;
    public int level;
    public long xp;
    public Color playerColor;
    public int selectedHexagonId;
    public string clientId;

    public void AddHexLand(Hexagon newHexLand)
    {
        if(newHexLand != null)
        {
            hexagons.Add(newHexLand.id, newHexLand);
            totalHexLands++;
        }
    }

    public void AddUnitToAllHexagonsPlayerHas()
    {
        foreach (KeyValuePair<int, Hexagon> hexagon in hexagons)
        {
            hexagon.Value.AddOneUnitToTroop();
            troop++;
        }
    }
}