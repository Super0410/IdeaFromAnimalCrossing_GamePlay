using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperPathFinding;
using SuperEvent;
using UnityEngine.UI;

public class Enemy : LogicMatchGraphicMonoPool, IHitable
{
    public int health = 1;
    public float speed = 10;
    public float findPathThreshold = 0.3f;
    private float nextTimeFindPath = 0;
    private Stack<Vector3> pathStack;
    Vector3 targetPos;

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy();
        }
    }

    public override void OnReuseObject()
    {
        base.OnReuseObject();
        base.CloneGraphic();

        EventSystem.Inst.AddListener(Event_UpdatePathFinding.eventId, UpdatePathFinding);
    }

    protected override void Destroy()
    {
        gameObject.SetActive(false);
        graphicInstance.SetActive(false);

        EventSystem.Inst.RemoveListener(Event_UpdatePathFinding.eventId, UpdatePathFinding);
    }

    private void Move()
    {
        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            if (pathStack == null || pathStack.Count <= 0)
                return;
            targetPos = pathStack.Pop();
        }

        Vector3 moveDir = (targetPos - transform.position).normalized;
        transform.Translate(moveDir * speed * Time.deltaTime, Space.World);
        graphicInstance.transform.LookAt(graphicInstance.transform.position + moveDir);
    }

    private void FindPath(bool force = false)
    {
        if (force || nextTimeFindPath< Time.time)
        {
            nextTimeFindPath = Time.time + findPathThreshold;
            AStarManager.Inst.FindPath(new PathRequest(transform, GameManager.Inst.playerLogicTrans, FindPathCallback));
        }
    }

    private void FindPathCallback(Stack<Vector3> _pathStack)
    {
        pathStack = _pathStack;
        if (pathStack == null || pathStack.Count < 1)
            return;
        targetPos = pathStack.Pop();
    }

    private void UpdatePathFinding(EventBase obj)
    {
        FindPath(true);
    }

    private void Update()
    {
        Move();
        FindPath();
        base.TryResetPos();
    }

    private void OnDrawGizmos()
    {
        if (pathStack != null)
        {
            foreach(Vector3 pos in pathStack)
            {
                Gizmos.DrawCube(pos, Vector3.one * (GridDefine.GridSize - 0.1f));
            }
        }
    }
}
