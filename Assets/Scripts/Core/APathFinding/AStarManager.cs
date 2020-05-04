using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperPathFinding
{
    public class AStarManager : Singleton<AStarManager>
    {
        public Vector2 gridSize = new Vector2(90, 90);
        public LayerMask unwalkableLayer;
        public int pathFindingPerFrame = 3;
        private Grid grid;
        private PathFinding pathFinding;
        private Queue<PathRequest> requestQueue = new Queue<PathRequest>();

        public void Init()
        {
            grid = new Grid();
            grid.Init();
            pathFinding = new PathFinding();
            pathFinding.Init(grid);
        }

        public void FindPath(PathRequest request)
        {
            requestQueue.Enqueue(request);
        }

        public void UpdateTile(Transform holder, bool canWalk)
        {
            for (int i = 0; i < holder.childCount; i++)
            {
                Node centerNode = grid.GetNodeByWorldPoint(holder.GetChild(i).position);
                centerNode.walkable = canWalk;
                List<Node> aroundNodeList = grid.GetNeighbours(centerNode);
                for(int j = 0; j < aroundNodeList.Count; j++)
                {
                    Node node = aroundNodeList[j];
                    if (node.walkable == canWalk)
                        continue;
                    node.walkable = canWalk;// GridHelper.CheckWalkable(node.worldPosition);
                }
            }
        }

        private void Update()
        {
            if (pathFinding == null)
                return;

            int handleCount = 0;
            while (requestQueue.Count > 0)
            {
                PathRequest request = requestQueue.Dequeue();
                if (request.seeker == null || request.target == null)
                    continue;
                pathFinding.FindPath(request.seeker.position, request.target.position, request.callback);

                handleCount++;
                if (handleCount >= pathFindingPerFrame)
                    break;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(gridSize.x, 1, gridSize.y));

            if (grid != null)
                grid.OnDrawGizmos();
        }
    }
}
