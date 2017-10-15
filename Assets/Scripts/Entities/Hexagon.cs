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

    private Color playerColor = new Color(0.69f, 1f, 0.69f);
    private Color hoverPlayerColor = new Color(0.15f, 1f, 0.27f);
    private Color environmentColor = new Color(0.69f, 0.76f, 1f);
    private Color hoverEnvironmentColor = new Color(0.45f, 0.55f, 1f);
    private Color enemyColor = new Color(1f, 0.69f, 0.69f);
    private Color hoverEnemyColor = new Color(1f, 0.69f, 0.69f);

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

        ChangeColor(environmentColor);
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

        ChangeColor(playerColor);
    }
    
    public void OnRayCastHit()
    {
        if (isPlayer)
        {
            ChangeColor(hoverPlayerColor);
        }
        else if (isEnemy)
        {
            ChangeColor(hoverEnemyColor);
        }
        else
        {
            ChangeColor(hoverEnvironmentColor);
        }
    }

    public void OnRayCastExit()
    {
        if (isPlayer)
        {
            ChangeColor(playerColor);
        }
        else if (isEnemy)
        {
            ChangeColor(enemyColor);
        }
        else
        {
            ChangeColor(environmentColor);
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
