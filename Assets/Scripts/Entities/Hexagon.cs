using System;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hexagon : MonoBehaviour
{
    public SpriteRenderer landSpriteRenderer;
    public SpriteRenderer fogSpriteRenderer;
    public int id;
    public float width;
    public float height;
    public bool isPlayer;
    public bool isEnemy;
    public int troop = 0;
    public NeighborStructure neighborStructure;
    public HexagonHUD hud;
    public ELandState state;

	void Awake ()
    {
        neighborStructure = new NeighborStructure();

        if (landSpriteRenderer == null)
        {
            landSpriteRenderer = GetComponent<SpriteRenderer>();
        }

        width = landSpriteRenderer.size.x;
        height = landSpriteRenderer.size.y;
    }
        
    public void ChangeColor(Color color)
    {
        landSpriteRenderer.color = color;
    }

    public void SetAsPlayer(Player player, int troop)
    {
        if (player != null)
        {
            isPlayer = true;

            if (hud != null)
            {
                hud.gameObject.SetActive(true);
                hud.SetTroop(troop);
                this.troop = troop;
            }

            ChangeColor(player.playerColor);

            player.AddHexLand(this);
        }
    }

    public void SetLandSprite(Sprite sprite)
    {
        if(landSpriteRenderer != null)
        {
            landSpriteRenderer.sprite = sprite;
        }
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
        float x = transform.position.x;
        float y = transform.position.y;       

        Hexagon neighbor = null;

        //Defining TopLeft
        Vector3 direction = new Vector3(x - width * 0.75f, y + height * 0.5f, 0);
        neighbor = MathHelper.DetectNeighbor(this, direction);

        if(neighbor != null)
        {
            this.neighborStructure.AddTopLeftNeighbor(neighbor.id);
        }

        //Defining TopMiddle
        direction = new Vector3(x, y + height, 0);
        neighbor = MathHelper.DetectNeighbor(this, direction);

        if (neighbor != null)
        {
            this.neighborStructure.AddTopMiddleNeighbor(neighbor.id);
        }
        
        //Defining TopRight
        direction = new Vector3(x + width * 0.75f, y + height * 0.5f, 0);
        neighbor = MathHelper.DetectNeighbor(this, direction);
        
        if (neighbor != null)
        {
            this.neighborStructure.AddTopRightNeighbor(neighbor.id);
        }

        //Defining BottomRight
        direction = new Vector3(x + width * 0.75f, y - height * 0.5f, 0);
        neighbor = MathHelper.DetectNeighbor(this, direction);

        if (neighbor != null)
        {
            this.neighborStructure.AddBottomRightNeighbor(neighbor.id);
        }

        //Defining BottomMiddle
        direction = new Vector3(x, y - height, 0);
        neighbor = MathHelper.DetectNeighbor(this, direction);

        if (neighbor != null)
        {
            this.neighborStructure.AddBottomMiddleNeighbor(neighbor.id);
        }

        //Defining BottomLeft
        direction = new Vector3(x - width * 0.75f, y - height * 0.5f, 0);
        neighbor = MathHelper.DetectNeighbor(this, direction);

        if (neighbor != null)
        {
            this.neighborStructure.AddBottomLeftNeighbor(neighbor.id);
        }
    }

    public void AddToTopLeft(int index)
    {
        if (neighborStructure != null)
        {
            neighborStructure.AddTopLeftNeighbor(index);
        }
    }

    public void AddToTopMiddle(int index)
    {
        if (neighborStructure != null)
        {
            neighborStructure.AddTopMiddleNeighbor(index);
        }
    }

    public void AddToTopRight(int index)
    {
        if (neighborStructure != null)
        {
            neighborStructure.AddTopRightNeighbor(index);
        }
    }

    public void AddToBottomRight(int index)
    {
        if (neighborStructure != null)
        {
            neighborStructure.AddBottomRightNeighbor(index);
        }
    }

    public void AddToBottomMiddle(int index)
    {
        if (neighborStructure != null)
        {
            neighborStructure.AddBottomMiddleNeighbor(index);
        }
    }

    public void AddToBottomLeft(int index)
    {
        if (neighborStructure != null)
        {
            neighborStructure.AddBottomLeftNeighbor(index);
        }
    }

    public void ChangeToVisibleState()
    {
        ChangeState(ELandState.Visible);
        ActivateLandSprite();
    }

    public void ChangeToFoggedState()
    {
        ChangeState(ELandState.Fogged);
        ActivateFogSprite();
    }

    public ENeighborPosition GetNeighborPositionInRelationTo(Hexagon currentPlayerHexagon)
    {
        if(currentPlayerHexagon != null)
        {
            if(currentPlayerHexagon.id == this.neighborStructure.bottomLeft)
            {
                return ENeighborPosition.TopRight;
            }
            if (currentPlayerHexagon.id == this.neighborStructure.bottomMiddle)
            {
                return ENeighborPosition.TopMiddle;
            }
            if (currentPlayerHexagon.id == this.neighborStructure.bottomRight)
            {
                return ENeighborPosition.TopLeft;
            }
            if (currentPlayerHexagon.id == this.neighborStructure.topLeft)
            {
                return ENeighborPosition.BottomRight;
            }
            if (currentPlayerHexagon.id == this.neighborStructure.topMiddle)
            {
                return ENeighborPosition.BottomMiddle;
            }
            if (currentPlayerHexagon.id == this.neighborStructure.topRight)
            {
                return ENeighborPosition.BottomLeft;
            }
        }

        return ENeighborPosition.None;
    }

    public void ChangeToBorderFoggedState()
    {
        ChangeState(ELandState.BorderFooged);
        ActivateBorderFogSprite();
    }

    private void ChangeState(ELandState state)
    {
        this.state = state;
    }

    private void ActivateLandSprite()
    {
        if (landSpriteRenderer != null)
        {
            if (landSpriteRenderer.sprite == null && MapManager.Instance != null)
            {
                landSpriteRenderer.sprite = MapManager.Instance.GetRandomLandSprite();
            }

            landSpriteRenderer.enabled = true;
            DeactivateFogSprite();
        }
    }

    private void DeactivateLandSprite()
    {
        if (landSpriteRenderer != null)
        {
            landSpriteRenderer.enabled = false;
        }
    }

    private void ActivateFogSprite()
    {
        if (fogSpriteRenderer != null)
        {
            if (fogSpriteRenderer.sprite == null && MapManager.Instance != null)
            {
                fogSpriteRenderer.sprite = MapManager.Instance.GetFogSprite();
            }

            fogSpriteRenderer.enabled = true;
            DeactivateLandSprite();
        }
    }

    private void DeactivateFogSprite()
    {
        if(fogSpriteRenderer != null)
        {            
            fogSpriteRenderer.enabled = false;
            fogSpriteRenderer.gameObject.SetActive(false);
        }
    }

    private void ActivateBorderFogSprite()
    {
        if (fogSpriteRenderer != null && landSpriteRenderer != null && MapManager.Instance != null)
        {
            if (fogSpriteRenderer.sprite == null)
            {
                fogSpriteRenderer.sprite = MapManager.Instance.GetBorderFogSprite();
            }

            if(landSpriteRenderer.sprite == null)
            {
                landSpriteRenderer.sprite = MapManager.Instance.GetRandomLandSprite();
            }

            landSpriteRenderer.enabled = true;

            fogSpriteRenderer.gameObject.SetActive(true);
            fogSpriteRenderer.enabled = true;
        }
    }
}