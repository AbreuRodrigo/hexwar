using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NeighborStructure
{
    private Dictionary<ENeighborPosition, string> neighbors = new Dictionary<ENeighborPosition, string>(6);

    [Header("Neighbors")]
    public string topLeft;
    public string topMiddle;
    public string topRight;
    public string bottomRight;
    public string bottomMiddle;
    public string bottomLeft;

    public bool HasTopLeftNeighbor()
    {
        return topLeft != null;
    }

    public bool HasTopMiddleNeighbor()
    {
        return topMiddle != null;
    }

    public bool HasTopRightNeighbor()
    {
        return topRight != null;
    }

    public bool HasBottomRightNeighbor()
    {
        return bottomRight != null;
    }

    public bool HasBottomMiddleNeighbor()
    {
        return bottomMiddle != null;
    }

    public bool HasBottomLeftNeighbor()
    {
        return bottomLeft != null;
    }

    public void AddTopLeftNeighbor(string id)
    {
        AddIfNull(ENeighborPosition.TopLeft, id, out topLeft);
    }

    public void AddTopMiddleNeighbor(string id)
    {
        AddIfNull(ENeighborPosition.TopMiddle, id, out topMiddle);
    }

    public void AddTopRightNeighbor(string id)
    {
        AddIfNull(ENeighborPosition.TopRight, id, out topRight);
    }

    public void AddBottomRightNeighbor(string id)
    {
        AddIfNull(ENeighborPosition.BottomRight, id, out bottomRight);
    }

    public void AddBottomMiddleNeighbor(string id)
    {
        AddIfNull(ENeighborPosition.BottomMiddle, id, out bottomMiddle);
    }

    public void AddBottomLeftNeighbor(string id)
    {
        AddIfNull(ENeighborPosition.BottomLeft, id, out bottomLeft);
    }

    private void AddIfNull(ENeighborPosition position, string id, out string neighborId)
    {
        if(neighbors != null)
        {
            string outId = null;
            neighbors.TryGetValue(position, out outId);
            
            if(outId == null)
            {
                neighbors.Add(position, id);
                neighborId = id;

                return;
            }            
        }

        neighborId = null;
    }
}