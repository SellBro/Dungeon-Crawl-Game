using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Pathfinding;
using RPG.units;
using UnityEngine;

using RPG.Units;

namespace RPG.Core
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private float turnDelay = 0.1f;


        public BlockManager blockManager;
        public List<SingleNodeBlocker> obstacles;

        BlockManager.TraversalProvider traversalProvider;
        
        
        
        public static GameManager Instance = null;
        
        public bool playerTurn = true;

        private List<EnemyController> _units;
        private bool _unitsMoving;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if(Instance != this)
                Destroy(gameObject);
            
            DontDestroyOnLoad(gameObject);
            
            traversalProvider = new BlockManager.TraversalProvider(blockManager, BlockManager.BlockMode.OnlySelector, obstacles);
            
            _units = new List<EnemyController>();
            InitGame();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) playerTurn = false;
            
            if (playerTurn || _unitsMoving) return;

            StartCoroutine(MoveUnits());
        }

        public void AddUnitToList(EnemyController unit)
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
            
            _units.Sort();
            
            foreach(EnemyController unit in _units)
            {
                Debug.Log(unit.name);
                unit.Act();
                yield return new WaitForSeconds(turnDelay/10);
            }

            playerTurn = true;
            _unitsMoving = false;
        }
        
        
        public Path ConstuctPath(Transform position, Transform target)
        {
            var path = ABPath.Construct(position.position, target.position, null);
            traversalProvider = new BlockManager.TraversalProvider(blockManager, BlockManager.BlockMode.OnlySelector, obstacles);
            
            // Make the path use a specific traversal provider
            path.traversalProvider = traversalProvider;
            
            // Calculate the path synchronously
            AstarPath.StartPath(path);
            path.BlockUntilCalculated();
            
            if (path.error) 
            {
                Debug.Log("No path was found");
            } 

            return path;
        }
        
    }
}
