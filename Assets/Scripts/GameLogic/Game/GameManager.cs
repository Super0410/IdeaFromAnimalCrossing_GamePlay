using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Transform playerLogicTrans;
    public Transform playerGraphicTrans;

    private void Awake()
    {
        StartCoroutine(LoadGame());
    }

    IEnumerator LoadGame()
    {
        SuperGround.GroundManager.Inst.Init();
        yield return new WaitForEndOfFrame();
        SuperPathFinding.AStarManager.Inst.Init();
    }
}
