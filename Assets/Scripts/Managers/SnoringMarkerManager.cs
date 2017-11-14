using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if(objectPool != null)
        {
            foreach(PoolableObject po  in objectPool.usedObjects)
            {
                objectPool.ReturnObjectToPool(po);
            }
        }
    }
}