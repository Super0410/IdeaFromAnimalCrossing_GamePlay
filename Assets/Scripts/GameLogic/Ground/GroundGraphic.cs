using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperGround {

    [System.Serializable]
    public class GroundGraphic
    {
        public Transform graphicHolder;
        public bool editMode = false;
        public RollType rollType = RollType.XZ;
        [Range(0, 0.1f)] public float rollStrength = 0.01f;
        [SerializeField] private Vector3 tileBoundaryX_Fwd_Z = new Vector3(45, 20, 45);
        [SerializeField] private Transform tileHolder;
        [SerializeField] internal Transform gobjHolder;
        [SerializeField] private GroundTile[,] tileArr;

        public void Move(Vector3 moveSpeed)
        {
            foreach(GroundTile tile in tileArr)
            {
                Transform tileTrans = tile.gobj.transform;
                tileTrans.Translate(moveSpeed, Space.World);

                Vector3 fixedPos;
                if (GroundHelper.Graphic.TileTryTouchBoundary(tile, GroundManager.Inst.tileSize, tileBoundaryX_Fwd_Z, graphicHolder.position, out fixedPos))
                    tileTrans.position = fixedPos;
            }
        }

        public void SetIntoTile(Transform trans, TileCoord tilePos, Vector3 localPos)
        {
            GroundTile tile = GroundHelper.Graphic.GetTileByRowColumn(tilePos);
            trans.parent = tile.gobj.transform;
            trans.localPosition = localPos / GroundManager.Inst.tileSize;
        }

        internal void SetShaderProperties()
        {
            if (rollType == RollType.Z)
                Shader.EnableKeyword("GroundRollOnlyZ");
            else
                Shader.DisableKeyword("GroundRollOnlyZ");

            if (editMode && !Application.isPlaying)
            {
                Shader.SetGlobalFloat("_RollStrength", 0);
                return;
            }
            if (GameManager.Inst == null)
                return;
            if (GameManager.Inst.playerGraphicTrans == null)
                return;
            Shader.SetGlobalVector("_PlayerPos", GameManager.Inst.playerGraphicTrans.position);
            Shader.SetGlobalFloat("_RollStrength", rollStrength);
        }

        internal void RelocateTile()
        {
            if (tileArr == null || tileArr.Length < 1 || tileArr[0, 0] == null)
            {
                InitTileArr();
                RelocateTile();
                return;
            }
            foreach(GroundTile tile in tileArr)
            {
                GameObject tileGObj = tile.gobj;
                if (tileGObj == null)
                {
                    InitTileArr();
                    RelocateTile();
                    return;
                }

                Transform tileTrans = tileGObj.transform;
                tileTrans.localScale = Vector3.one * GroundManager.Inst.tileSize;
                tileTrans.position = GroundHelper.Common.GetTilePos(graphicHolder.position, GroundManager.Inst.tileSize, tile.tileCoord);
            }
        }

        internal void Init()
        {
            InitTileArr();
            AutoSetWorldGObjParentTile();
        }

        internal void OnDrawGizmos()
        {
            Vector3 boundaryCenter = graphicHolder.position + Vector3.forward * tileBoundaryX_Fwd_Z.y;
            Gizmos.DrawWireCube(boundaryCenter, new Vector3(tileBoundaryX_Fwd_Z.x, 0, tileBoundaryX_Fwd_Z.z) * 2);
        }

        private void InitTileArr()
        {
            if (tileHolder == null)
                return;
            tileArr = new GroundTile[3, 3];
            for (int i = 0; i < 9; i++)
            {
                TileCoord itileCoord;
                GroundHelper.Graphic.GetRowColumnByIndex(i, out itileCoord);
                Vector3 iLocate = GroundHelper.Common.GetTilePos(graphicHolder.position, GroundManager.Inst.tileSize, itileCoord);
                GameObject iGobj = tileHolder.GetChild(i).gameObject;
                iGobj.transform.position = iLocate;
                iGobj.name = itileCoord.row + "_" + itileCoord.column;

                tileArr[itileCoord.row, itileCoord.column] = new GroundTile()
                {
                    index = i,
                    tileCoord = itileCoord,
                    locatePos = iLocate,
                    gobj = iGobj
                };
            }

            GroundHelper.Graphic.tileArr = tileArr;
        }

        private void AutoSetWorldGObjParentTile()
        {
            if (gobjHolder == null)
                return;
            while (gobjHolder.childCount > 0)
            {
                Transform gobjTrans = gobjHolder.GetChild(0);
                gobjTrans.SetParent(GroundHelper.Graphic.GetTileByPos(gobjTrans.position).gobj.transform, true);
            }
        }
    }
}