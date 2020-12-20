using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using RPG.DungeonGenerator;
using RPG.Player;
using RPG.units;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RPG.Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance = null;
        
        public BlockManager blockManager;
        public List<SingleNodeBlocker> obstacles;
        
        public bool playerTurn = true;
        public int level = 1;
        
        
        public GameObject player;
        [SerializeField] private float turnDelay = 0.1f;

        //private AstarPath _astarPath;
        private BlockManager.TraversalProvider traversalProvider;
        private List<EnemyController> _units;
        private bool _unitsMoving;

        public Vector2 spawn;
        
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if(Instance != this)
                Destroy(gameObject);
            
            DontDestroyOnLoad(gameObject);

            AstarPath.active = GetComponent<AstarPath>();
            blockManager = GetComponent<BlockManager>();

            _units = new List<EnemyController>();
            InitGame();
        }


        private void Start()
        {
            StartCoroutine(GenerateDungeon());
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) playerTurn = false;
            
            if (playerTurn || _unitsMoving) return;

            StartCoroutine(MoveUnits());
        }

        private IEnumerator GenerateDungeon()
        {
            yield return StartCoroutine(LevelGeneration.Instance.GenerateLevel());
            SpawnPlayer();
            yield return null;
            
            yield return StartCoroutine(LevelGeneration.Instance.FillRooms());

            AstarPath.active.Scan();
            
            foreach (var unit in _units)
            {
                SingleNodeBlocker unitNode = unit.GetComponent<SingleNodeBlocker>();
                obstacles.Add(unitNode);
            }
            
            traversalProvider = new BlockManager.TraversalProvider(blockManager, BlockManager.BlockMode.OnlySelector, obstacles);
        }

        private void SpawnPlayer()
        {
            int x = Random.Range(1, 16);
            int y = Random.Range(1, 15);
            int num = Random.Range(0, LevelGeneration.DungeonRooms.Count);
            while (!LevelGeneration.DungeonRooms[num].IsTileEmpty(x, y))
            {
                x = Random.Range(1, 16);
                y = Random.Range(1, 15);
            }

            spawn = LevelGeneration.DungeonRooms[num].position;
            player = Instantiate(player, new Vector2(spawn.x + x + 0.5f, spawn.y + y + 0.5f), Quaternion.identity);
            LevelGeneration.DungeonRooms[num].hasSpawnedPlayer = true;
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
                unit.Act();
                yield return null;
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
