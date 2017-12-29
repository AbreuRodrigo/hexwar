/*===============================================================
Product:    Hexwar
Developer:  Abreu
Company:    TerraNix Studios - https://www.terranix.com
Date:       28/12/2017 03:22
================================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hexwar
{
    public class LandManager : WeakSingleton<LandManager>
    {
        public List<Land> lands = new List<Land>();

        private Dictionary<ETerrainType, Land> landsDictionary = new Dictionary<ETerrainType, Land>();

        void Start()
        {
            foreach (Land land in lands)
            {
                landsDictionary.Add(land.terrainType, land);
            }
        }

        public Sprite GetLandSpriteVariation(ETerrainType terrainType)
        {
            Land land = null;

            if(landsDictionary.TryGetValue(terrainType, out land))
            {
                if(land != null)
                {
                    int amount = land.variations.Count;
                    int index = Random.Range(0, amount);

                    return land.variations[index];
                }
            }

            return null;
        }

        public Sprite GetPlains()
        {
            return GetLandSpriteVariation(ETerrainType.Plains);
        }

        public Sprite GetForest()
        {
            return GetLandSpriteVariation(ETerrainType.Forest);
        }

        public Sprite GetPineForest()
        {
            return GetLandSpriteVariation(ETerrainType.PineForest);
        }

        public Sprite GetSwamps()
        {
            return GetLandSpriteVariation(ETerrainType.Swamps);
        }

        public Sprite GetHills()
        {
            return GetLandSpriteVariation(ETerrainType.Hills);
        }

        public Sprite GetGoldMine()
        {
            return GetLandSpriteVariation(ETerrainType.GoldMine);
        }

        public Sprite GetFarms()
        {
            return GetLandSpriteVariation(ETerrainType.Farms);
        }

        public Sprite GetSmallVillage()
        {
            return GetLandSpriteVariation(ETerrainType.SmallVillage);
        }

        public Sprite GetLumberjackForest()
        {
            return GetLandSpriteVariation(ETerrainType.LumberJackForest);
        }

        public Sprite GetLumberjackPineForest()
        {
            return GetLandSpriteVariation(ETerrainType.LumberJackPineForest);
        }
	}
}