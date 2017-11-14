using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    public Stack<PoolableObject> availableObjects = new Stack<PoolableObject>();
    public List<PoolableObject> usedObjects = new List<PoolableObject>();

    private int initialPoolSize;
    private PoolableObject poolableObject;
    private Transform parent;

    private PoolableObject objRef = null;

    public ObjectPool(int initialPoolSize, PoolableObject poolableObject, Transform parent)
    {
        this.initialPoolSize = initialPoolSize;
        this.poolableObject = poolableObject;
        this.parent = parent;
        
        InitializePool();
    }

    private void InitializePool()
    {
        GameObject refObj = null;
        PoolableObject poolableObj = null;

        for (int i = 0; i < initialPoolSize; i++)
        {
            refObj = GameObject.Instantiate(poolableObject.gameObject);
            refObj.transform.SetParent(parent);
            refObj.SetActive(false);

            poolableObj = refObj.GetComponent<PoolableObject>();
            poolableObj.idInPool = i;

            availableObjects.Push(poolableObj);
        }
    }

    public PoolableObject GetNextAvailable()
    {
        if(availableObjects != null && availableObjects.Count > 0)
        {
            objRef = availableObjects.Pop();
            usedObjects.Add(objRef);

            return objRef;
        }

        return null;
    }

    public void ReturnObjectToPool(PoolableObject poolableObject)
    {
        if(usedObjects != null && poolableObject != null)
        {
            usedObjects.Remove(poolableObject);
            availableObjects.Push(poolableObject);
            poolableObject.gameObject.SetActive(false);
        }
    }
}