using SuperWeapon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperPool;

namespace SuperPlayer
{
    internal class PlayerAttack : MonoBehaviour
    {
        public Transform attackPos;
        public GameObject bulletPrefab;

        internal void Init()
        {

        }

        internal void Attack()
        {
            GameObject bulletInst = PoolManager.Inst.ReuseObject(bulletPrefab, attackPos.position, attackPos.rotation);
        }
    }
}
