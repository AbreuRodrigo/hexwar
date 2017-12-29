using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hexwar
{
    public class MapManager : MonoBehaviour
    {
        private static MapManager instance;
        public static MapManager Instance
        {
            get { return instance; }
        }

        private Dictionary<int, Hexagon> mappedHexagons = new Dictionary<int, Hexagon>();

        [Header("Components")]
        public MapGenerator mapGenerator;

        [Header("Attributes")]
        public EMapSize mapSize;

        [Header("Sprites")]
        public Sprite fogSprite;
        public Sprite borderFogSprite;
        public Sprite PlainSprite
        {
            get
            {
                return LandManager.Instance.GetSmallVillage();
            }
        }

        private Hexagon hexRef = null;
        private Hexagon neighborHexagonRef = null;        

        private Dictionary<EMapSize, MapStructure> gameMapDictionary = null;

        void Awake()
        {
            instance = this;

            gameMapDictionary = new Dictionary<EMapSize, MapStructure>()
            {
                { EMapSize.SMALL, new MapStructure(150, 2) },
                { EMapSize.MEDIUM, new MapStructure(300, 2) },
                { EMapSize.LARGE, new MapStructure(500, 2) },
                { EMapSize.GIANT, new MapStructure(700, 2) }
            };

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
            if (hexagon == null)
            {
                return;
            }

            if (mappedHexagons == null)
            {
                mappedHexagons = new Dictionary<int, Hexagon>();
            }

            if (hexagon != null)
            {
                hexagon.ChangeToFoggedState();
            }

            mappedHexagons.Add(hexagon.id, hexagon);
        }

        public Sprite GetRandomLandSprite()
        {
            return LandManager.Instance.GetPlains();
        }

        public Sprite GetFogSprite()
        {
            if (fogSprite != null)
            {
                return fogSprite;
            }

            return null;
        }

        public Sprite GetBorderFogSprite()
        {
            if (borderFogSprite != null)
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
            Hexagon hexagon = null;
            MapStructure map = null;

            try
            {
                map = gameMapDictionary[size];
                int max = map.max;
                int players = map.players;

                hexagon = mappedHexagons[index];
            }
            catch(System.Exception e)
            {
                Debug.LogError(e.Message + " - size: " + size + " index: " + index);
            }

            return hexagon;
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
}