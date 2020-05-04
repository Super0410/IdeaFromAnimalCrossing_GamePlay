using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperCamera
{
    public class CameraController : MonoBehaviour
    {
        private void LateUpdate()
        {
            transform.position = GameManager.Inst.playerGraphicTrans.position;
        }
    }
}