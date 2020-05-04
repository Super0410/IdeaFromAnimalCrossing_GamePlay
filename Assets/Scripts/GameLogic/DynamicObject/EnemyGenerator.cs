using SuperPool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    public Vector2 randomThreshold = new Vector2(0.5f, 2);
    public Vector2 radiusMinMax = new Vector2(10, 20);
    public GameObject enemyPrefab;

    private void Start()
    {
        StartCoroutine(DelayGenerateEnemy());
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    IEnumerator DelayGenerateEnemy()
    {
        while (true)
        {
            float nextTime = Random.Range(randomThreshold.x, randomThreshold.y);
            float timeCount = 0;
            while(timeCount<nextTime)
            {
                timeCount += Time.deltaTime;
                yield return null;
            }
            Vector2 randomDir = Random.insideUnitCircle;
            Vector3 centerPos = GameManager.Inst.playerLogicTrans.position;
            Vector3 offset = new Vector3(randomDir.x, 0, randomDir.y) * radiusMinMax.y;
            if (Vector3.Distance(Vector3.zero, offset) < radiusMinMax.x)
                offset = offset.normalized * radiusMinMax.x;
            Vector3 generatePos = centerPos + offset;

            PoolManager.Inst.ReuseObject(enemyPrefab, generatePos, Quaternion.identity);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(GameManager.Inst.playerLogicTrans.position, radiusMinMax.x);
        Gizmos.DrawWireSphere(GameManager.Inst.playerLogicTrans.position, radiusMinMax.y);
    }
}
