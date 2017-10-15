using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Dictionary<string, Hexagon> hexagons = new Dictionary<string, Hexagon>();

    public int troop = 8;
    public Color playerColor;
}