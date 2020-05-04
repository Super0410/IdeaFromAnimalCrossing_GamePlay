using SuperWeapon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperPlayer
{
    internal class PlayerBuild : MonoBehaviour
    {
        public Transform buildPos;
        public GameObject buildPrefab;

        internal void Init()
        {

        }

        internal void Build()
        {
            GameObject buildInst = Instantiate(buildPrefab, buildPos.position, buildPos.rotation);
        }
    }
}
