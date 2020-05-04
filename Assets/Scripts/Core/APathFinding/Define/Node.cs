using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperPathFinding
{
    public class Node : IHeapItem<Node>
    {
        public bool walkable;
        public Vector3 worldPosition;
        public int gridX;
        public int gridY;

        public int gCost;
        public int hCost;
        public int fCost
        {
            get
            {
                return gCost + hCost;
            }
        }
        public Node parent;
        public int HeapIndex { get; set; }

        public int CompareTo(Node nodeToCompare)
        {
            int compare = fCost.CompareTo(nodeToCompare.fCost);
            if (compare == 0)
            {
                compare = hCost.CompareTo(nodeToCompare.hCost);
            }
            return -compare;
        }

        public Node(bool _walkable, Vector3 _worldPosition, int _gridX, int _gridY)
        {
            walkable = _walkable;
            worldPosition = _worldPosition;
            gridX = _gridX;
            gridY = _gridY;
        }
    }


    public class NodeSphere
    {
        public bool walkable;
        public Vector3 worldPosition;
        public List<NodeSphere> neighbourNodeList;
        public NodeSphere parent;

        public float gCost;
        public float hCost;
        public float fCost
        {
            get
            {
                return gCost + hCost;
            }
        }
        public NodeSphere(bool _walkable, Vector3 _worldPosition)
        {
            walkable = _walkable;
            worldPosition = _worldPosition;
            neighbourNodeList = new List<NodeSphere>();
        }
        public NodeSphere(bool _walkable, Vector3 _worldPosition, List<NodeSphere> _neighbourNodeList)
        {
            walkable = _walkable;
            worldPosition = _worldPosition;
            neighbourNodeList = _neighbourNodeList;
        }
    }
}
