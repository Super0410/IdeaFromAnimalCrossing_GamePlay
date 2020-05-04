using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperGround;

namespace SuperPlayer
{
    [RequireComponent(typeof(CharacterController))]
    internal class PlayerMovement : MonoBehaviour, ITileLogicChild
    {
        CharacterController cc;
        Plane groundPlane;
        Camera cam;
        Vector3 playerLastPos;

        public TileCoord tilePos { get; set; }

        public void DoCross()
        {
            playerLastPos = transform.position;
        }

        internal void Init()
        {
            cc = GetComponent<CharacterController>();
            groundPlane = new Plane(Vector3.up, Vector3.zero);
            cam = Camera.main;
            playerLastPos = transform.position;
        }

        internal void Move(float speed)
        {
            LogicMove(speed);
            GraphicMove();
        }

        internal void LookAt(Vector3 mouseInput)
        {
            GraphicLookAt(mouseInput);
            LogicLookAt();
        }

        private void LogicMove(float speed)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            Vector3 inputDir = new Vector3(h, 0, v).normalized;
            Vector3 moveSpeed = inputDir * Time.deltaTime * speed;

            cc.Move(moveSpeed);
        }

        private void GraphicMove()
        {
            GroundManager.Inst.graphic.Move(playerLastPos - transform.position);
            playerLastPos = transform.position;

            GroundManager.Inst.logic.TryResetMapChild(this);
        }

        private void GraphicLookAt(Vector3 mouseInput)
        {
            float rayDst;
            Ray mouseRay = cam.ScreenPointToRay(mouseInput);
            groundPlane.Raycast(mouseRay, out rayDst);
            Vector3 groundPos = mouseRay.GetPoint(rayDst);

            Transform graphicTrans = GameManager.Inst.playerGraphicTrans;
            groundPos.y = graphicTrans.position.y;
            graphicTrans.LookAt(groundPos);
        }

        private void LogicLookAt()
        {
            Transform logicTrans = transform;
            logicTrans.eulerAngles = GameManager.Inst.playerGraphicTrans.eulerAngles;
        }
    }
}
