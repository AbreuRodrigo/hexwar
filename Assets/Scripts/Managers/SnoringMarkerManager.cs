using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hexwar
{
    public class SnoringMarkerManager : Singleton<SnoringMarkerManager>
    {
        public PoolableObject poolableObject;
        public int initialPoolSize;

        private ObjectPool objectPool = null;

        void Start()
        {
            objectPool = new ObjectPool(initialPoolSize, poolableObject, transform);
        }

        public void RequestSnoringMarker(Hexagon newPlayerHexagon)
        {
            PoolableObject obj = objectPool.GetNextAvailable();
            obj.transform.position = newPlayerHexagon.transform.position;
            obj.gameObject.SetActive(true);
        }

        public void ReturnAllObjectsInUseToPool()
        {
            if (objectPool != null)
            {
                int count = objectPool.usedObjects.Count;

                for (int i = 0; i < count; i++)
                {
                    objectPool.ReturnObjectToPool(objectPool.usedObjects.First.Value);
                }
            }
        }
    }
}