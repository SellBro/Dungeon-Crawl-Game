using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using RPG.Player;
using UnityEngine;

namespace RPG.Units
{
    public class Unit : MovingObject
    {
        private Transform _target;
        private bool _skipMove;

        protected override void Start()
        {
            GameManager.Instance.AddUnitToList(this);
            
            _target = GameObject.FindGameObjectWithTag("Player").transform;
            base.Start();
        }

        protected override void AttemptMove<T>(int xDir, int yDir)
        {
            if (_skipMove)
            {
                _skipMove = false;
                return;
            }
            
            base.AttemptMove<T>(xDir, yDir);

            _skipMove = true;
        }

        public void MoveEnemy()
        {
            int xDir = 0;
            int yDir = 0;

            AttemptMove<PlayerController>(xDir,yDir);
        }

        protected override void OnCantMove<T>(T component)
        {
            PlayerController hitPlayer = component as PlayerController;
            
            
        }
    }
}
