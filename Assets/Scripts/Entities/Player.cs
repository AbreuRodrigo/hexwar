using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Dictionary<int, Hexagon> hexagons = new Dictionary<int, Hexagon>();

    public string playerName;
    public int troop = 8;
    public int totalHexLands = 0;
    public int level;
    public Color playerColor;

    public void AddHexLand(Hexagon newHexLand)
    {
        if(newHexLand != null)
        {
            hexagons.Add(newHexLand.index, newHexLand);
            totalHexLands++;
        }
    }
}