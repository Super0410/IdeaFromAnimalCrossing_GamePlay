using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperPlayer
{
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(PlayerAttack))]
    [RequireComponent(typeof(PlayerBuild))]
    public class PlayerController : MonoBehaviour
    {
        public float moveSpeed = 10;
        private PlayerMovement movement;
        private PlayerAttack attack;
        private PlayerBuild build; 

        private void Awake()
        {
            movement = GetComponent<PlayerMovement>();
            movement.Init();
            attack = GetComponent<PlayerAttack>();
            attack.Init();
            build = GetComponent<PlayerBuild>();
            build.Init();
        }

        private void Update()
        {
            movement.Move(moveSpeed);
            movement.LookAt(Input.mousePosition);
            if (Input.GetMouseButtonDown(0))
                attack.Attack();
            if (Input.GetMouseButtonDown(1))
                build.Build();
        }
    }
}