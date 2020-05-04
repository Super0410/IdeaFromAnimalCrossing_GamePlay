using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperGround;

public class LogicMatchGraphicMono : MonoBehaviour, ITileLogicChild
{
    [SerializeField] private GameObject graphicPrefab;
    protected GameObject graphicInstance;
    public TileCoord tilePos { get; set; }

    public void DoCross()
    {
    }

    protected void TryResetPos()
    {
        GroundManager.Inst.logic.TryResetMapChild(this);
        Vector3 localPos;
        tilePos = GroundManager.Inst.logic.GetMapTile(this, out localPos);

        GroundManager.Inst.graphic.SetIntoTile(graphicInstance.transform, tilePos, localPos);
    }

    protected void CloneGraphic()
    {
        graphicInstance = Instantiate(graphicPrefab);
        graphicInstance.transform.eulerAngles = transform.eulerAngles;
    }
}
