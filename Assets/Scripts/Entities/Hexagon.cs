using System;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hexagon : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public string id;
    public float width;
    public float height;
    public bool isPlayer;
    public bool isEnemy;
    public NeighborStructure neighborStructure;

    public GameObject troopCounter;

	void Awake ()
    {
        neighborStructure = new NeighborStructure();

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        id = Guid.NewGuid().ToString().Substring(0, 7);
        width = spriteRenderer.size.x;
        height = spriteRenderer.size.y;
    }
        
    public void ChangeColor(Color color)
    {
        spriteRenderer.color = color;
    }

    public void SetAsPlayer()
    {
        isPlayer = true;

        if(troopCounter != null)
        {
            troopCounter.SetActive(true);            
        }

        ChangeColor(GameConfig.player1Color);
    }
    
    public void OnRayCastHit()
    {
        if (isPlayer)
        {
            ChangeColor(GameConfig.player1ColorOnHover);
        }
        else if (isEnemy)
        {
            ChangeColor(GameConfig.enemyColorOnHover);
        }
        else
        {
            ChangeColor(GameConfig.openEnvironmentColorOnHover);
        }
    }

    public void OnRayCastExit()
    {
        if (isPlayer)
        {
            ChangeColor(GameConfig.player1Color);
        }
        else if (isEnemy)
        {
            ChangeColor(GameConfig.enemyColor);
        }
        else
        {
            ChangeColor(GameConfig.openEnvironmentColor);
        }
    }

    public void DetectNeighbors()
    {
        Hexagon neighbor = MathHelper.DetectNeighbor(this, Vector3.up);

        if(neighbor != null)
        {
            if(!this.neighborStructure.HasTopMiddleNeighbor())
            {
                this.neighborStructure.AddTopMiddleNeighbor(neighbor.id);
            }
        }

        neighbor = MathHelper.DetectNeighbor(this, Vector3.down);

        if (neighbor != null)
        {
            if (!this.neighborStructure.HasBottomMiddleNeighbor())
            {
                this.neighborStructure.AddBottomMiddleNeighbor(neighbor.id);
            }
        }
    }
}
