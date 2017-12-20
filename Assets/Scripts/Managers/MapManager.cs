using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    private static MapManager instance;
    public static MapManager Instance
    {
        get { return instance; }
    }

    private Dictionary<int, Hexagon> mappedHexagons = new Dictionary<int, Hexagon>();
    private int totalGroundSprites = 0;

    [Header("Components")]
    public MapGenerator mapGenerator;

    [Header("Attributes")]
    public EMapSize mapSize;

    [Header("Sprites")]    
    public Sprite plainSprite;
    public Sprite fogSprite;
    public Sprite borderFogSprite;

    [Header("Sprite Categories")]
    public SpriteCategory[] groundSprites;

    private Hexagon hexRef = null;
    private Hexagon neighborHexagonRef = null;

    private Dictionary<EMapSize, MapStructure> gameMapDictionary = null;

    void Awake()
    {
        instance = this;

        gameMapDictionary = new Dictionary<EMapSize, MapStructure>()
        {
            { EMapSize.SMALL, new MapStructure(250, 2) },
            { EMapSize.MEDIUM, new MapStructure(500, 3) },
            { EMapSize.LARGE, new MapStructure(750, 4) },
            { EMapSize.GIANT, new MapStructure(1000, 4) }
        };

        if (groundSprites != null)
        {
            totalGroundSprites = groundSprites.Length;
        }

        if (mapGenerator != null)
        {
            mapGenerator.CreateMap(GetMapSize());
        }
    }

    public Dictionary<int, Hexagon> GetMappedHexagons()
    {
        return mappedHexagons;
    }

    public Hexagon RandomizeHexagon()
    {
        int rand = Random.Range(0, mappedHexagons.Count);

        return mappedHexagons[rand];
    }

    public void AddHexagon(Hexagon hexagon)
    {
        if(hexagon == null)
        {
            return;
        }

        if(mappedHexagons == null)
        {
            mappedHexagons = new Dictionary<int, Hexagon>();
        }

        if(hexagon != null)
        {
            hexagon.ChangeToFoggedState();
        }

        mappedHexagons.Add(hexagon.id, hexagon);
    }

    public Sprite GetRandomLandSprite()
    {
        if (groundSprites != null)
        {
            SpriteCategory category = groundSprites[Random.Range(0, totalGroundSprites)];
            return category.sprites[Random.Range(0, category.TotalSprites)];
        }

        return null;
    }
    
    public Sprite GetFogSprite()
    {
        if(fogSprite != null)
        {
            return fogSprite;
        }

        return null;
    }

    public Sprite GetBorderFogSprite()
    {
        if(borderFogSprite != null)
        {
            return borderFogSprite;
        }

        return null;
    }

    public Hexagon TryGetHexagonByIndex(int index)
    {
        if (index == -1 || mappedHexagons == null)
        {
            return null;
        }

        mappedHexagons.TryGetValue(index, out hexRef);

        return hexRef;
    }

    public Hexagon GetHexagonByMapSizeAndIndex(EMapSize size, int index)
    {
        MapStructure map = gameMapDictionary[size];
        int max = map.max;
        int players = map.players;

        return mappedHexagons[index];
    }

    public void RevealNeighbors(Hexagon refHexagon)
    {
        if (refHexagon != null && refHexagon.neighborStructure != null && mappedHexagons != null)
        {
            ValidateNeighborHexagonAndRevealIt(refHexagon.neighborStructure.left);
            ValidateNeighborHexagonAndRevealIt(refHexagon.neighborStructure.topLeft);
            ValidateNeighborHexagonAndRevealIt(refHexagon.neighborStructure.topRight);
            ValidateNeighborHexagonAndRevealIt(refHexagon.neighborStructure.right);
            ValidateNeighborHexagonAndRevealIt(refHexagon.neighborStructure.bottomRight);
            ValidateNeighborHexagonAndRevealIt(refHexagon.neighborStructure.bottomLeft);
        }
    }

    private void ValidateNeighborHexagonAndRevealIt(int index)
    {
        if (index > -1)
        {
            neighborHexagonRef = TryGetHexagonByIndex(index);

            if (neighborHexagonRef != null)
            {
                neighborHexagonRef.ChangeToVisibleState();
            }
        }
    }

    private int GetMapSize()
    {
        if (gameMapDictionary != null)
        {
            return gameMapDictionary[GameSetup.mapSize].max;
        }

        return 0;
    }
}