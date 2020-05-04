using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperGround
{
    [System.Serializable]
    public class GroundLogic
    {
        [SerializeField] private Transform logicHolder;
        [SerializeField] private Transform ground;
        [SerializeField] private Material debug_gobjMat;

        public void TryResetMapChild(ITileLogicChild tileChild)
        {
            Transform tileChildTrans = tileChild.transform;

            Vector4 boundary = GroundHelper.Logic.GetMapBoundary(logicHolder.position, GroundManager.Inst.tileSize);
            Vector3 fixDir = Vector3.zero;
            if (tileChildTrans.position.x < boundary.x)
                fixDir.x = 1;
            else if (tileChildTrans.position.x > boundary.y)
                fixDir.x = -1;
            if (tileChildTrans.position.z < boundary.z)
                fixDir.z = 1;
            else if (tileChildTrans.position.z > boundary.w)
                fixDir.z = -1;

            if (fixDir.x == 0 && fixDir.z == 0)
                return;

            tileChildTrans.position += fixDir * GroundHelper.Common.GetCrossMapDst(GroundManager.Inst.tileSize);
            tileChild.DoCross();
        }

        public TileCoord GetMapTile(ITileLogicChild tileChild, out Vector3 localPos)
        {
            TileCoord tilePos = GroundHelper.Logic.GetTileCoordByPos(logicHolder.position, GroundManager.Inst.tileSize, tileChild.transform.position);
            localPos = tileChild.transform.position - GroundHelper.Common.GetTilePos(logicHolder.position, GroundManager.Inst.tileSize, tilePos);
            return tilePos;
        }

        internal void Init()
        {
            Transform gobjGroup = GameObject.Instantiate(GroundManager.Inst.graphic.gobjHolder.gameObject).transform;
            Vector3 gobjGroupScale = gobjGroup.localScale;
            gobjGroupScale.x /= GroundHelper.Logic.GroundScale.x;
            gobjGroupScale.y /= GroundHelper.Logic.GroundScale.y;
            gobjGroupScale.z /= GroundHelper.Logic.GroundScale.z;
            gobjGroup.localScale = gobjGroupScale;
            gobjGroup.SetParent(ground, false);
            for (int i = 0; i < gobjGroup.childCount; i++)
            {
                gobjGroup.GetChild(i).GetComponent<MeshRenderer>().material = debug_gobjMat;
            }
        }

        internal void OnValueChange()
        {
            ground.localScale = GroundHelper.Logic.GroundScale;
        }
    }
}
