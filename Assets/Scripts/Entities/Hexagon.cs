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
    public HexagonHUD debug;
    public ELandState state;
    public int spawningTurn = 0;

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

    void Start()
    {
        if (GameSetup.Instance.showHexagonsId)
        {
            debug.gameObject.SetActive(true);
            debug.SetValue(id);
        }
        else
        {
            debug.gameObject.SetActive(false);
        }
    }
        
    public void ChangeColor(Color color)
    {
        landSpriteRenderer.color = color;
    }

    public void SetAsPlayer(Player player)
    { 
        isPlayer = true;

        if (hud != null)
        {
            hud.gameObject.SetActive(true);
            hud.SetValue(troop);
        }

        ChangeColor(player.playerColor);
        player.AddHexLand(this);
    }

    public void SetAsEnemy(Player opponent, int units = 0)
    {
        isEnemy = true;
                        
        if (hud != null && GameSetup.Instance.showEnemies)
        {
            hud.gameObject.SetActive(true);

            if (units != 0)
            {
                hud.SetValue(units + troop);
            }
            else
            {
                hud.SetValue(troop);
            }
        }

        ChangeColor(opponent.playerColor);
        opponent.AddHexLand(this);
    }

    public void SetLandSprite(Sprite sprite)
    {
        if(landSpriteRenderer != null)
        {
            landSpriteRenderer.sprite = sprite;
        }
    }

    public void EnableNeighbours()
    {
        MapManager.Instance.RevealNeighbors(this);
    }
    
    public void OnRayCastHit()
    {
        if (isPlayer)
        {
            ChangeColor(GameSetup.playerRealColor);
        }
        else if (isEnemy)
        {
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
            ChangeColor(GameSetup.playerRealColor);
        }
        else if (isEnemy)
        {
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

    public bool TestIsNeighbour(Hexagon other)
    {
        if(neighborStructure != null && other != null)
        {
            if(other.id == neighborStructure.topLeft)
            {
                return true;
            }
            if (other.id == neighborStructure.topMiddle)
            {
                return true;
            }
            if (other.id == neighborStructure.topRight)
            {
                return true;
            }
            if (other.id == neighborStructure.bottomLeft)
            {
                return true;
            }
            if (other.id == neighborStructure.bottomMiddle)
            {
                return true;
            }
            if (other.id == neighborStructure.bottomRight)
            {
                return true;
            }
        }

        return false;
    }

    public void ChangeToBorderFoggedState()
    {
        ChangeState(ELandState.BorderFooged);
        ActivateBorderFogSprite();
    }

    public void AddOneUnitToTroop()
    {
        troop++;
        hud.SetValue(troop);
    }

    public void DeductUnits(int deduction)
    {
        troop -= deduction;
        hud.SetValue(troop);
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