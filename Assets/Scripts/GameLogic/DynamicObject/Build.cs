using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using SuperPathFinding;
using SuperEvent;

public class Build : LogicMatchGraphicMono, IHitable
{
    public Transform obstaclesHolder;

    public void TakeDamage(int damage)
    {
    }

    private void Start()
    {
        base.CloneGraphic();
        base.TryResetPos();

        AStarManager.Inst.UpdateTile(obstaclesHolder, false);
        EventSystem.Inst.Notify(Event_UpdatePathFinding.eventId, null);
    }
}
