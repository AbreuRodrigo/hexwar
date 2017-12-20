using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpriteCategory
{
    public string categoryName;
    public Sprite[] sprites;

    public int TotalSprites
    {
        get { return sprites.Length; }
    }
}
