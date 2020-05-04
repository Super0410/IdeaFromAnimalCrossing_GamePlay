using System.ComponentModel;
using UnityEngine;

namespace SuperGround
{
    internal static class GroundHelper
    {
        internal const int CenterRow = 1;
        internal const int CenterColumn = 1;
        internal const int centerIndex = 5;
        // 由于使用的是plane，后续处理时会在tileSize单位的基础上，乘plane的长宽10单位
        internal const int tileMeshSize = 10;

        internal static class Common
        {
            internal static float GetCrossMapDst(float tileSize)
            {
                return tileSize * tileMeshSize * 3;
            }

            internal static Vector3 GetTilePos(Vector3 centerPos, float tileSize, TileCoord tileCoord)
            {
                int offsetRow = tileCoord.row - CenterRow;
                int offsetColumn = tileCoord.column - CenterColumn;
                Vector3 pos = centerPos + new Vector3(offsetColumn, 0, offsetRow) * tileSize * tileMeshSize;
                return pos;
            }
        }


        internal static class Logic
        {
            internal static Vector3 GroundScale
            {
                get
                {
                    return Vector3.one * GroundManager.Inst.tileSize * 3;
                }
            }

            internal static Vector4 GetMapBoundary(Vector3 centerPos, float tileSize)
            {
                float mapSize = Common.GetCrossMapDst(tileSize);
                Vector4 boundary = Vector4.zero;
                boundary.x = centerPos.x - mapSize;
                boundary.y = centerPos.x + mapSize;
                boundary.z = centerPos.z - mapSize;
                boundary.w = centerPos.z + mapSize;

                return boundary * 0.5f;
            }

            internal static TileCoord GetTileCoordByPos(Vector3 centerPos, float tileSize, Vector3 pos)
            {
                Vector4 boundary = GetMapBoundary(centerPos, tileSize);
                int column = Mathf.FloorToInt((pos.x - boundary.x) * 3 / (boundary.y - boundary.x));
                int row = Mathf.FloorToInt((pos.z - boundary.z) * 3 / (boundary.w - boundary.z));

                return new TileCoord(row, column);
            }
        }

        internal static class Graphic
        {
            internal static GroundTile[,] tileArr;

            internal static GroundTile GetTileByPos(Vector3 pos)
            {
                float minDst = float.MaxValue;
                TileCoord tilePos = new TileCoord();
                foreach(GroundTile tile in tileArr)
                {
                    Transform tempTileTrans = tile.gobj.transform;
                    float dst = Mathf.Pow(tempTileTrans.position.x - pos.x, 2)
                        + Mathf.Pow(tempTileTrans.position.z - pos.z, 2);
                    if (dst > minDst)
                        continue;
                    minDst = dst;
                    tilePos = tile.tileCoord;
                }

                return tileArr[tilePos.row, tilePos.column];
            }

            internal static GroundTile GetTileByRowColumn(TileCoord tileCoord)
            {
                tileCoord.row = tileCoord.row % 3;
                tileCoord.column = tileCoord.column % 3;
                if (tileCoord.row < 0)
                    tileCoord.row += 3;
                if (tileCoord.column < 0)
                    tileCoord.column += 3;
                return tileArr[tileCoord.row, tileCoord.column];
            }
            
            internal static Vector4 GetMapBoundaryXZMinMax()
            {
                Vector4 boundary = new Vector4(float.MaxValue, float.MinValue, float.MaxValue, float.MinValue);
                foreach (GroundTile tile in tileArr)
                {
                    Transform tileTrans = tile.gobj.transform;
                    if (tileTrans.position.x < boundary.x)
                        boundary.x = tileTrans.position.x;
                    else if (tileTrans.position.x > boundary.y)
                        boundary.y = tileTrans.position.x;
                    if (tileTrans.position.z < boundary.z)
                        boundary.z = tileTrans.position.z;
                    else if (tileTrans.position.z > boundary.w)
                        boundary.w = tileTrans.position.z;
                }
                return boundary;
            }

            internal static bool TileTryTouchBoundary(GroundTile tile, float tileSize, Vector3 tileBoundaryX_Fwd_Z, Vector3 centerPos, out Vector3 fixedPos)
            {
                Transform tileTrans = tile.gobj.transform;
                fixedPos = tileTrans.position;

                float mapDst =  Common.GetCrossMapDst(tileSize);

                if (tileTrans.position.x < centerPos.x - tileBoundaryX_Fwd_Z.x)
                {
                    fixedPos.x += mapDst;
                    return true;
                }
                if (tileTrans.position.x > centerPos.x + tileBoundaryX_Fwd_Z.x)
                {
                    fixedPos.x -= mapDst;
                    return true;
                }
                if (tileTrans.position.z < centerPos.z + tileBoundaryX_Fwd_Z.y - tileBoundaryX_Fwd_Z.z)
                {
                    fixedPos.z += mapDst;
                    return true;
                }
                if (tileTrans.position.z > centerPos.z + tileBoundaryX_Fwd_Z.y + tileBoundaryX_Fwd_Z.z)
                {
                    fixedPos.z -= mapDst;
                    return true;
                }

                return false;
            }

            internal static Vector3 CalculateTilePos(int index, float tileSize, Vector3 centerPos)
            {
                TileCoord tileCoord;
                GetRowColumnByIndex(index, out tileCoord);

                return Common.GetTilePos(centerPos, tileSize, tileCoord);
            }

            internal static void GetRowColumnByIndex(int index, out TileCoord tileCoord)
            {
                tileCoord = new TileCoord();
                tileCoord.row = index / 3;
                tileCoord.column = Mathf.Max(0, index - tileCoord.row * 3);
            }
        }
    }
}
