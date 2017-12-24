using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hexwar
{
    [System.Serializable]
    public class NeighborStructure
    {
        private Dictionary<ENeighborPosition, int> neighbors = new Dictionary<ENeighborPosition, int>(6);

        [Header("Neighbors")]
        public int left = -1;
        public int topLeft = -1;
        public int topRight = -1;
        public int right = -1;
        public int bottomRight = -1;
        public int bottomLeft = -1;

        public bool HasTopLeftNeighbor()
        {
            return topLeft != -1;
        }

        public bool HasLeftNeighbor()
        {
            return left != -1;
        }

        public bool HasTopRightNeighbor()
        {
            return topRight != -1;
        }

        public bool HasBottomRightNeighbor()
        {
            return bottomRight != -1;
        }

        public bool HasRightNeighbor()
        {
            return right != -1;
        }

        public bool HasBottomLeftNeighbor()
        {
            return bottomLeft != -1;
        }

        public void AddTopLeftNeighbor(int index)
        {
            AddIfNull(ENeighborPosition.TopLeft, index, out topLeft);
        }

        public void AddLeftNeighbor(int index)
        {
            AddIfNull(ENeighborPosition.Left, index, out left);
        }

        public void AddTopRightNeighbor(int index)
        {
            AddIfNull(ENeighborPosition.TopRight, index, out topRight);
        }

        public void AddBottomRightNeighbor(int index)
        {
            AddIfNull(ENeighborPosition.BottomRight, index, out bottomRight);
        }

        public void AddRightNeighbor(int index)
        {
            AddIfNull(ENeighborPosition.Right, index, out right);
        }

        public void AddBottomLeftNeighbor(int index)
        {
            AddIfNull(ENeighborPosition.BottomLeft, index, out bottomLeft);
        }

        private void AddIfNull(ENeighborPosition position, int id, out int neighborId)
        {
            if (neighbors != null)
            {
                int outId;
                neighbors.TryGetValue(position, out outId);
                neighbors.Add(position, id);
                neighborId = id;

                return;
            }

            neighborId = -1;
        }
    }
}