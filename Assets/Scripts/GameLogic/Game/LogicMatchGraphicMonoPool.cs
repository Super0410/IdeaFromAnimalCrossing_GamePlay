using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperGround;
using SuperPool;

public class LogicMatchGraphicMonoPool : PoolObject, ITileLogicChild
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
        if (graphicInstance == null)
            graphicInstance = PoolManager.Inst.ReuseObject(graphicPrefab, transform.position, transform.rotation);
        else
        {
            graphicInstance.SetActive(true);
            graphicInstance.transform.position = transform.position;
            graphicInstance.transform.rotation = transform.rotation;
        }
    }
}
