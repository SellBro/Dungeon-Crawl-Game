using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Player
{
    public class PlayerController : MovingObject
    {
        protected override void Start()
        {
            // Custom Start
            base.Start();
        }

        private void Update()
        {
            if (!GameManager.Instance.playerTurn) return;

            int horizontal = Mathf.FloorToInt(Input.GetAxisRaw("Horizontal"));
            int vertical = Mathf.FloorToInt(Input.GetAxisRaw("Vertical"));
            if (horizontal != 0)
                vertical = 0;

            if (horizontal != 0 || vertical != 0)
            {
                AttemptMove<Collider2D>(horizontal,vertical);
            }
        }

        protected override void AttemptMove<T>(int xDir, int yDir)
        {
            // Custom AttemptMove
            base.AttemptMove<T>(xDir, yDir);

            RaycastHit2D hit;

            GameManager.Instance.playerTurn = false;
        }

        protected override void OnCantMove<T>(T component)
        {
            Collider2D collider = component as Collider2D;
        }
    }
}
