using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperPool;

public class PoolObjectCreater : MonoBehaviour
{
	public CreatPoolNeeded[] poolObjArr;

	void Awake ()
	{
		for (int i = 0; i < poolObjArr.Length; i++) {
			PoolManager.Inst.CreatPool (poolObjArr [i].gameObject, poolObjArr [i].poolSize);
		}
	}

	[System.Serializable]
	public struct CreatPoolNeeded
	{
		public GameObject gameObject;
		public int poolSize;
	}
}
