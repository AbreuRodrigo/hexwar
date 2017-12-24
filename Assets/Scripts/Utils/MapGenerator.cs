using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hexwar
{
    public class MapGenerator : MonoBehaviour
    {
        private ENeighborPosition[] listOfNeighborPosition = null;

        private int maxIndex = 0;
        private static int idRef = 0;

        [Header("Prefabs")]
        public GameObject hexagonPrefab;

        [Header("References")]
        public Transform mapParent;

        void Awake()
        {
            idRef = 0;
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

                while (index < size - 1)
                {
                    hexBase = MapManager.Instance.RandomizeHexagon();
                    neighborPos = RandomizeNeighborPosition();
                    direction = GetNeighborPositionByDirection(hexBase, neighborPos);
                    hasNeighborInDirection = MathHelper.HasNeighborInDirection(direction);

                    if (!hasNeighborInDirection && neighborPos != ENeighborPosition.None)
                    {
                        switch (neighborPos)
                        {
                            case ENeighborPosition.Left:
                                CreateInTheLeft(hexBase);
                                break;
                            case ENeighborPosition.TopLeft:
                                CreateInTheTopLeft(hexBase);
                                break;
                            case ENeighborPosition.TopRight:
                                CreateInTheTopRight(hexBase);
                                break;
                            case ENeighborPosition.Right:
                                CreateInTheRight(hexBase);
                                break;
                            case ENeighborPosition.BottomRight:
                                CreateInTheBottomRight(hexBase);
                                break;
                            case ENeighborPosition.BottomLeft:
                                CreateInTheBottomLeft(hexBase);
                                break;
                        }

                        index++;
                    }
                }
            }

            foreach (var h in MapManager.Instance.GetMappedHexagons())
            {
                h.Value.DetectNeighbors();
            }
        }

        private Hexagon CreateNewHexagon(Vector3 pos)
        {
            return CreateNewHexagon(pos.x, pos.y, pos.z);
        }

        private Hexagon CreateNewHexagon(float x, float y, float z)
        {
            Hexagon hexRef = null;

            if (hexagonPrefab != null && mapParent != null)
            {
                hexRef = Instantiate(hexagonPrefab, new Vector3(x, y - 0.64f, z), Quaternion.identity, mapParent).GetComponent<Hexagon>();
                hexRef.id = idRef;
                hexRef.SetLandSprite(MapManager.Instance.GetRandomLandSprite());
                idRef++;

                MapManager.Instance.AddHexagon(hexRef);
            }

            return hexRef;
        }

        private void CreateInTheTopLeft(Hexagon hexBase)
        {
            CreateNewHexagon(GetNeighborPositionByDirection(hexBase, ENeighborPosition.TopLeft));
        }

        private void CreateInTheLeft(Hexagon hexBase)
        {
            CreateNewHexagon(GetNeighborPositionByDirection(hexBase, ENeighborPosition.Left));
        }

        private void CreateInTheTopRight(Hexagon hexBase)
        {
            CreateNewHexagon(GetNeighborPositionByDirection(hexBase, ENeighborPosition.TopRight));
        }

        private void CreateInTheBottomRight(Hexagon hexBase)
        {
            CreateNewHexagon(GetNeighborPositionByDirection(hexBase, ENeighborPosition.BottomRight));
        }

        private void CreateInTheRight(Hexagon hexBase)
        {
            CreateNewHexagon(GetNeighborPositionByDirection(hexBase, ENeighborPosition.Right));
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
            float b = 0.32f;
            float h = b * 3;
            float w = b * 2;
            float sw = b * 4;

            y += w;

            switch (neighborPos)
            {
                case ENeighborPosition.Left:
                    direction = new Vector3(x - sw, y, 0);
                    break;
                case ENeighborPosition.TopLeft:
                    direction = new Vector3(x - w, y + h, 0);
                    break;
                case ENeighborPosition.TopRight:
                    direction = new Vector3(x + w, y + h, 0);
                    break;
                case ENeighborPosition.Right:
                    direction = new Vector3(x + sw, y, 0);
                    break;
                case ENeighborPosition.BottomRight:
                    direction = new Vector3(x + w, y - h, 0);
                    break;
                case ENeighborPosition.BottomLeft:
                    direction = new Vector3(x - w, y - h, 0);
                    break;
            }

            return direction;
        }
    }
}