using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject hexagonPrefab;

    [Header("Sprites")]
    public Sprite[] grounds;
    public Sprite plain;

    [Header("References")]
    public Transform map;

    [Header("Attributes")]
    public int size = 7;//The value may be different in the inspector

    private ENeighborPosition[] listOfNeighborPosition = null;
    private int totalGroundSprites = 0;

    private Dictionary<string, Hexagon> mappedHexagons = new Dictionary<string, Hexagon>();
    private List<Hexagon> hexagons = new List<Hexagon>();
    private Hexagon hexRef = null;

    void Awake()
    {
        listOfNeighborPosition = (ENeighborPosition[]) System.Enum.GetValues(typeof(ENeighborPosition));

        if (grounds != null)
        {
            totalGroundSprites = grounds.Length;
        }
    }
   
    void Start ()
    {
        if(hexagonPrefab != null)
        {
            Hexagon hexBase = CreateNewHexagon(0, 0, 0);
            ENeighborPosition neighborPos;
            Vector3 direction;
            bool hasNeighborInDirection = false;

            int index = 0;

            float time = Time.time;

            while (index < size-1)
            {
                neighborPos = RandomizeNeighborPosition();
                hexBase = RandomizeHexagon();
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

            Debug.Log(Time.time - time);
        }
        
        foreach(Hexagon h in hexagons)
        {
            mappedHexagons.Add(h.id, h);
            h.DetectNeighbors();
        }

        int playerIndex = Random.Range(0, hexagons.Count);

        Hexagon playerHexagon = hexagons[playerIndex];
        playerHexagon.SetAsPlayer();
        playerHexagon.spriteRenderer.sprite = plain;

        GameManager.Instance.SetPlayerHexagon(playerHexagon);

        hexagons.Clear();
	}

    private Hexagon CreateNewHexagon(Vector3 pos)
    {
        return CreateNewHexagon(pos.x, pos.y, pos.z);
    }

    private Hexagon CreateNewHexagon(float x, float y, float z)
    {
        if (hexagonPrefab != null && map != null)
        {
            hexRef = Instantiate(hexagonPrefab, new Vector3(x, y, z), Quaternion.identity, map).GetComponent<Hexagon>();

            if (grounds != null)
            {
                hexRef.spriteRenderer.sprite = grounds[Random.Range(0, totalGroundSprites)];
            }

            hexagons.Add(hexRef);
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
        int rand = Random.Range(0, listOfNeighborPosition.Length);
        return listOfNeighborPosition[rand];
    }

    private Hexagon RandomizeHexagon()
    {
        int max = hexagons.Count;
        int rand = Random.Range(0, max);

        return hexagons[rand];
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