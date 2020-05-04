using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperPool
{
	public class PoolObject : MonoBehaviour
	{
		public virtual void OnReuseObject()
		{

		}

		protected virtual void Destroy()
		{
			gameObject.SetActive(false);
		}
	}
}
