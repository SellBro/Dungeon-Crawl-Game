using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RPG.Units;

namespace RPG.Core
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private float turnDelay = 0.1f;
        
        public static GameManager Instance = null;
        
        public bool playerTurn = true;

        private List<Unit> _units;
        private bool _unitsMoving;
        
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if(Instance != this)
                Destroy(gameObject);
            
            DontDestroyOnLoad(gameObject);
            
            _units = new List<Unit>();
            InitGame();
        }

        private void Update()
        {
            if (playerTurn || _unitsMoving) return;

            StartCoroutine(MoveUnits());
        }

        public void AddUnitToList(Unit unit)
        {
            _units.Add(unit);
        }

        private void InitGame()
        {
            _units.Clear();
        }

        private IEnumerator MoveUnits()
        {
            _unitsMoving = true;
            yield return new WaitForSeconds(turnDelay);

            if (_units.Count == 0)
            {
                yield return new WaitForSeconds(turnDelay);
            }
            
            foreach(Unit unit in _units)
            {
                unit.MoveEnemy();
                yield return new WaitForSeconds(unit.moveTime);
            }

            playerTurn = true;
            _unitsMoving = false;
        }
    }
}
