using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperPool
{
	public class PoolManager : Singleton<PoolManager>
	{
		private Dictionary<int, Queue<ObjectInstance>> poolDict = new Dictionary<int, Queue<ObjectInstance>>();

		public void CreatPool(GameObject prefab, int poolSize)
		{
			int poolKey = prefab.GetInstanceID();

			GameObject prefabParent = new GameObject(prefab.name + " Pool");
			prefabParent.transform.parent = transform;

			if (!poolDict.ContainsKey(poolKey))
			{
				poolDict.Add(poolKey, new Queue<ObjectInstance>());

				for (int i = 0; i < poolSize; i++)
				{
					ObjectInstance newGObj = new ObjectInstance(Instantiate(prefab) as GameObject);
					newGObj.SetParent(prefabParent.transform);

					poolDict[poolKey].Enqueue(newGObj);
				}
			}
		}

		public GameObject ReuseObject(GameObject prefab, Vector3 position, Quaternion rotation)
		{
			int poolKey = prefab.GetInstanceID();

			if (poolDict.ContainsKey(poolKey))
			{
				ObjectInstance newGObj = poolDict[poolKey].Dequeue();
				poolDict[poolKey].Enqueue(newGObj);

				newGObj.OnReuseObject(position, rotation);

				return newGObj.gameObject;
			}
			return null;
		}

		public class ObjectInstance
		{
			public GameObject gameObject;
			Transform transform;

			bool hasPoolObjectComponent;
			PoolObject pooObjectScript;

			public ObjectInstance(GameObject objectInstance)
			{
				gameObject = objectInstance;
				transform = gameObject.transform;

				gameObject.SetActive(false);

				if (gameObject.GetComponent<PoolObject>() != null)
				{
					hasPoolObjectComponent = true;
					pooObjectScript = gameObject.GetComponent<PoolObject>();
				}
			}

			public void OnReuseObject(Vector3 position, Quaternion rotation)
			{
				gameObject.SetActive(true);

				transform.position = position;
				transform.rotation = rotation;

				if (hasPoolObjectComponent)
					pooObjectScript.OnReuseObject();
			}

			public void SetParent(Transform parent)
			{
				transform.parent = parent;
			}
		}
	}
}
