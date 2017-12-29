/*===============================================================
Product:    Hexwar
Developer:  Abreu
Company:    TerraNix Studios - https://www.terranix.com
Date:       28/12/2017 03:33
================================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hexwar
{
    [System.Serializable]
	public class Land
	{
        public ETerrainType terrainType;
        public int level;
        public List<Sprite> variations = new List<Sprite>();
	}
}