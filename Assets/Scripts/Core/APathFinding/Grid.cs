using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperPathFinding
{
    public class Grid
    {
        private Node[,] nodeArr;

        internal int gridSizeX { get; private set; }
        internal int gridSizeY { get; private set; }
        internal int MaxSize
        {
            get
            {
                return gridSizeX * gridSizeY;
            }
        }

        internal void Init()
        {
            CreateGrid();
        }

        internal List<Node> GetNeighbours(Node node)
        {
            List<Node> neighboursList = new List<Node>();
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                        continue;

                    int checkX = node.gridX + x;
                    int checkY = node.gridY + y;

                    if (checkX < 0)
                        checkX = gridSizeX - 1;
                    else if (checkX >= gridSizeX)
                        checkX = 0;
                    if (checkY < 0)
                        checkY = gridSizeY - 1;
                    else if (checkY >= gridSizeY)
                        checkY = 0;

                    //if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                    //{
                    neighboursList.Add(nodeArr[checkX, checkY]);
                    //}
                }
            }
            return neighboursList;
        }

        internal Node GetNodeByWorldPoint(Vector3 worldPosition)
        {
            float percentX = (worldPosition.x + (float)nodeArr.GetLength(0) / 2) / nodeArr.GetLength(0);
            float percentY = (worldPosition.z + (float)nodeArr.GetLength(1) / 2) / nodeArr.GetLength(1);
            percentX = Mathf.Clamp01(percentX);
            percentY = Mathf.Clamp01(percentY);

            float perX = 1 / (float)gridSizeX;
            float perY = 1 / (float)gridSizeY;

            int x = (int)(percentX / perX);
            int y = (int)(percentY / perY);

            return nodeArr[x, y];
        }

        internal void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            if (nodeArr != null)
            {
                foreach (Node oneNode in nodeArr)
                {
                    if (oneNode.walkable)
                        continue;
                    Gizmos.DrawCube(oneNode.worldPosition, Vector3.one * (GridDefine.GridSize - 0.1f));
                }
            }
        }

        private void CreateGrid()
        {
            Vector3[,] nodePosArr = GetNodePosArr(AStarManager.Inst.gridSize);
            gridSizeX = nodePosArr.GetLength(0);
            gridSizeY = nodePosArr.GetLength(1);
            nodeArr = new Node[gridSizeX, gridSizeY];
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    Vector3 worldPos = nodePosArr[x, y];
                    bool walkable = GridHelper.CheckWalkable(worldPos);
                    nodeArr[x, y] = new Node(walkable, worldPos, x, y);
                }
            }
        }

        private Vector3[,] GetNodePosArr(Vector2 gridSize)
        {
            float nodeDiameter = GridDefine.GridSize;
            float nodeRadius = nodeDiameter / 2;
            int gridX = Mathf.FloorToInt(gridSize.x);
            int gridY = Mathf.FloorToInt(gridSize.y);

            Vector3[,] nodePosArr = new Vector3[gridX, gridY];

            Vector3 worldBottomLeft = -Vector3.right * gridX / 2 - Vector3.forward * gridY / 2;
            for (int x = 0; x < gridX; x++)
            {
                for (int y = 0; y < gridY; y++)
                {
                    Vector3 worldPoint = worldBottomLeft
                        + Vector3.right * (x * nodeDiameter + nodeRadius)
                        + Vector3.forward * (y * nodeDiameter + nodeRadius);

                    nodePosArr[x, y] = worldPoint;
                }
            }

            return nodePosArr;
        }
    }
}