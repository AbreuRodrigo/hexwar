using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    private ENeighborPosition[] listOfNeighborPosition = null;
    private Hexagon hexRef = null;
    
    private int hexGenerationCount = 0;
    private int maxIndex = 0;

    [Header("Prefabs")]
    public GameObject hexagonPrefab;

    [Header("References")]
    public Transform mapParent;

    void Awake()
    {
        listOfNeighborPosition = (ENeighborPosition[])System.Enum.GetValues(typeof(ENeighborPosition));
    }

    public void CreateMap(int size)
    {
        maxIndex = listOfNeighborPosition.Length;

        Random.InitState((int)GameSetup.mapSeed);

        if (hexagonPrefab != null && mapParent != null)
        {
            Hexagon hexBase = CreateNewHexagon(0, 0, 0);
            ENeighborPosition neighborPos;
            Vector3 direction;
            bool hasNeighborInDirection = false;

            int index = 0;
            
            while (index < size-1)
            {
                neighborPos = RandomizeNeighborPosition();
                hexBase = MapManager.Instance.RandomizeHexagon();
                direction = GetNeighborPositionByDirection(hexBase, neighborPos);
                hasNeighborInDirection = MathHelper.HasNeighborInDirection(hexBase, direction);
                
                if(!hasNeighborInDirection)
                {
                    switch (neighborPos)
                    {
                        case ENeighborPosition.TopLeft:
                            CreateInTheTopLeft(hexBase);
                            break;
                        case ENeighborPosition.TopMiddle:
                            CreateInTheTopMiddle(hexBase);
                            break;
                        case ENeighborPosition.TopRight:
                            CreateInTheTopRight(hexBase);
                            break;
                        case ENeighborPosition.BottomRight:
                            CreateInTheBottomRight(hexBase);
                            break;
                        case ENeighborPosition.BottomMiddle:
                            CreateInTheBottomMiddle(hexBase);
                            break;
                        case ENeighborPosition.BottomLeft:
                            CreateInTheBottomLeft(hexBase);
                            break;
                    }

                    index++;
                }
            }
        }
        
        foreach(var h in MapManager.Instance.GetMappedHexagons())
        {
            h.Value.DetectNeighbors();
        }

        Hexagon playerHexagon = MapManager.Instance.GetPositionIndexByMapSize(GameSetup.mapSize, GameSetup.localPlayerTurnId);

        if (playerHexagon != null)
        {
            playerHexagon.ChangeToVisibleState();
            playerHexagon.landSpriteRenderer.sprite = MapManager.Instance.plainSprite;
            playerHexagon.ChangeColor(GameSetup.playerRealColor);

            GameManager.Instance.SetPlayerInitialHexLand(playerHexagon);

            MapManager.Instance.RevealNeighbors(playerHexagon);
        }
	}

    private Hexagon CreateNewHexagon(Vector3 pos)
    {
        return CreateNewHexagon(pos.x, pos.y, pos.z);
    }

    private Hexagon CreateNewHexagon(float x, float y, float z)
    {
        if (hexagonPrefab != null && mapParent != null)
        {
            hexRef = Instantiate(hexagonPrefab, new Vector3(x, y, z), Quaternion.identity, mapParent).GetComponent<Hexagon>();
            hexRef.id = hexGenerationCount;
            hexRef.SetLandSprite(MapManager.Instance.GetRandomLandSprite());

            MapManager.Instance.AddHexagon(hexGenerationCount, hexRef);            

            hexGenerationCount++;
        }

        return hexRef;
    }

    private void CreateInTheTopLeft(Hexagon hexBase)
    {
        CreateNewHexagon(GetNeighborPositionByDirection(hexBase, ENeighborPosition.TopLeft));
    }

    private void CreateInTheTopMiddle(Hexagon hexBase)
    {
        CreateNewHexagon(GetNeighborPositionByDirection(hexBase, ENeighborPosition.TopMiddle));
    }

    private void CreateInTheTopRight(Hexagon hexBase)
    {
        CreateNewHexagon(GetNeighborPositionByDirection(hexBase, ENeighborPosition.TopRight));
    }

    private void CreateInTheBottomRight(Hexagon hexBase)
    {
        CreateNewHexagon(GetNeighborPositionByDirection(hexBase, ENeighborPosition.BottomRight));
    }

    private void CreateInTheBottomMiddle(Hexagon hexBase)
    {
        CreateNewHexagon(GetNeighborPositionByDirection(hexBase, ENeighborPosition.BottomMiddle));
    }

    private void CreateInTheBottomLeft(Hexagon hexBase)
    {
        CreateNewHexagon(GetNeighborPositionByDirection(hexBase, ENeighborPosition.BottomLeft));
    }

    private ENeighborPosition RandomizeNeighborPosition()
    {
        int rand = Random.Range(0, maxIndex);
        return listOfNeighborPosition[rand];
    }

    private Vector3 GetNeighborPositionByDirection(Hexagon hexBase, ENeighborPosition neighborPos)
    {
        Vector3 direction = Vector3.zero;
        float x = hexBase.transform.localPosition.x;
        float y = hexBase.transform.localPosition.y;

        switch (neighborPos)
        {
            case ENeighborPosition.TopLeft:
                direction = new Vector3(x - hexBase.width * 0.75f, y + hexBase.height * 0.5f, 0);
                break;
            case ENeighborPosition.TopMiddle:
                direction = new Vector3(x, y + hexBase.height, 0);
                break;
            case ENeighborPosition.TopRight:
                direction = new Vector3(x + hexBase.width * 0.75f, y + hexBase.height * 0.5f, 0);
                break;
            case ENeighborPosition.BottomRight:
                direction = new Vector3(x + hexBase.width * 0.75f, y - hexBase.height * 0.5f, 0);
                break;
            case ENeighborPosition.BottomMiddle:
                direction = new Vector3(x, y - hexBase.height, 0);
                break;
            case ENeighborPosition.BottomLeft:
                direction = new Vector3(x - hexBase.width * 0.75f, y - hexBase.height * 0.5f, 0);
                break;
        }

        return direction;
    }
}