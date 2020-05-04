using UnityEngine;

namespace SuperGround
{
    [System.Serializable]
    public struct TileCoord
    {
        public int row;
        public int column;

        public TileCoord (int _row, int _column)
        {
            row = _row;
            column = _column;
        }
    }

    [System.Serializable]
    public enum RollType
    {
        Z,
        XZ
    }

    [System.Serializable]
    internal class GroundTile
    {
        public int index;
        public TileCoord tileCoord;
        public Vector3 locatePos;
        public GameObject gobj;
    }

    public interface ITileGraphicChild
    {
        Transform transform{ get; }

        Vector2 tileRowColumn { get; set; }

        void DoCross();
    }

    public interface ITileLogicChild
    {
        Transform transform { get; }

        TileCoord tilePos { get; set; }

        void DoCross();
    }
}
