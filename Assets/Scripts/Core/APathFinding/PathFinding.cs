using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace SuperPathFinding
{
    public class PathFinding
    {
        Grid grid;

        public void Init(Grid _grid)
        {
            grid = _grid;
        }

        public void FindPath(Vector3 startPos, Vector3 targetPos, Action<Stack<Vector3>> cb)
        {
            if (grid == null)
                return;
            Node startNode = grid.GetNodeByWorldPoint(startPos);
            Node targetNode = grid.GetNodeByWorldPoint(targetPos);

            Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet.RemoveFirst();

                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    RetracePath(startNode, targetNode, cb);
                    return;
                }

                foreach (Node neighbour in grid.GetNeighbours(currentNode))
                {
                    if (!neighbour.walkable || closedSet.Contains(neighbour))
                        continue;

                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                        else
                            openSet.UpdateItem(neighbour);
                    }
                }
            }

            cb?.Invoke(null);
        }

        void RetracePath(Node startNode, Node endNode, Action<Stack<Vector3>> cb)
        {
            Stack<Vector3> path = new Stack<Vector3>();
            Node currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Push(currentNode.worldPosition);

                if (currentNode.gridX <= 0 && currentNode.parent.gridX >= grid.gridSizeX - 1)
                {
                    path.Push(currentNode.worldPosition + Vector3.right * AStarManager.Inst.gridSize.x);
                }
                else if (currentNode.gridX >= grid.gridSizeX - 1 && currentNode.parent.gridX <= 0)
                {
                    path.Push(currentNode.worldPosition + Vector3.left * AStarManager.Inst.gridSize.x);
                }
                if (currentNode.gridY <= 0 && currentNode.parent.gridY >= grid.gridSizeY - 1)
                {
                    path.Push(currentNode.worldPosition + Vector3.forward * AStarManager.Inst.gridSize.y);
                }
                else if (currentNode.gridY >= grid.gridSizeX - 1 && currentNode.parent.gridY <= 0)
                {
                    path.Push(currentNode.worldPosition + Vector3.back * AStarManager.Inst.gridSize.y);
                }

                currentNode = currentNode.parent;
            }

            if (path.Count > 0)
                cb?.Invoke(path);
        }

        int GetDistance(Node nodeA, Node nodeB)
        {
            int dstXCross = 0;
            if (nodeA.gridX < nodeB.gridX)
                dstXCross = nodeA.gridX + grid.gridSizeX - nodeB.gridX;
            else
                dstXCross = nodeB.gridX + grid.gridSizeX - nodeA.gridX;
            int dstYCross = 0;
            if (nodeA.gridY < nodeB.gridY)
                dstYCross = nodeA.gridY + grid.gridSizeY - nodeB.gridY;
            else
                dstYCross = nodeB.gridY + grid.gridSizeY - nodeA.gridY;


            int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
            int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

            dstX = dstXCross > dstX ? dstX : dstXCross;
            dstY = dstYCross > dstY ? dstY : dstYCross;

            if (dstX > dstY)
                return 14 * dstY + 10 * (dstX - dstY);
            return 14 * dstX + 10 * (dstY - dstX);
        }
    }
}