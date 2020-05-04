using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperPathFinding
{
    public class GridHelper
    {
        public static bool CheckWalkable(Vector3 worldPos)
        {
            return !(Physics.CheckSphere(worldPos, GridDefine.GridSize * 0.5f, AStarManager.Inst.unwalkableLayer));
        }
    }

    public class GridDefine
    {
        public const int GridSize = 1;
    }

    public struct PathRequest
    {
        public Transform seeker;
        public Transform target;
        public Action<Stack<Vector3>> callback;

        public PathRequest(Transform _seeker, Transform _target, Action<Stack<Vector3>> _callback)
        {
            seeker = _seeker;
            target = _target;
            callback = _callback;
        }
    }
}