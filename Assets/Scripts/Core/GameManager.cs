using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Pathfinding;
using SellBro.DungeonGenerator;
using SellBro.Units;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SellBro.Core
{
    public class GameManager : MonoBehaviour
    {
        [Header("Game Manager Settings")] 
        [SerializeField] private bool shouldGenerateLevel = true;
        [SerializeField] private GameObject cam;
        
        [Header("Managing Components")]
        public static GameManager Instance = null;
        public BlockManager blockManager;
        public GameObject unitsParent;

        [Header("Lists")]
        public List<SingleNodeBlocker> obstacles;
        public List<EnemyController> units;
        
        [Header("Level settings")]
        public bool playerTurn = true;
        public int level = 1;
        public Vector2 spawn;
        
        [Header("Player")]
        public GameObject player;
        
        [Header("Game Settings")]
        [SerializeField] private float turnDelay = 0.1f;
        
        private BlockManager.TraversalProvider traversalProvider;
        private bool _unitsMoving;

        private CinemachineVirtualCamera _camera;


        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if(Instance != this)
                Destroy(gameObject);
            
            DontDestroyOnLoad(gameObject);

            AstarPath.active = GetComponent<AstarPath>();
            blockManager = GetComponent<BlockManager>();
            units = new List<EnemyController>();

            InitGame();
        }


        private void Start()
        {
            if (shouldGenerateLevel)
            {
                StartCoroutine(GenerateDungeon());
            }
            else
            {
                GenerateTestScene();
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) playerTurn = false;
            
            if (playerTurn || _unitsMoving) return;

            StartCoroutine(MoveUnits());
        }

        private void InitGame()
        {
            units.Clear();
        }

        private IEnumerator GenerateDungeon()
        {
            // Generate rooms
            yield return StartCoroutine(LevelGeneration.Instance.GenerateLevel());
            // Spawn player
            SpawnPlayer();
            yield return null;
            
            // Generate room interior and units
            yield return StartCoroutine(LevelGeneration.Instance.FillRooms());

            // Create A*
            AstarPath.active.Scan();
            
            // Block Nodes under Units
            foreach (var unit in units)
            {
                SingleNodeBlocker unitNode = unit.GetComponent<SingleNodeBlocker>();
                obstacles.Add(unitNode);
            }
            
            // A* var
            traversalProvider = new BlockManager.TraversalProvider(blockManager, BlockManager.BlockMode.OnlySelector, obstacles);
        }

        private void GenerateTestScene()
        {
            SpawnTestPlayer();
            AstarPath.active.Scan();
                
            foreach (var unit in units)
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
            
            Vector3 movePos = new Vector3(spawn.x + x + 0.5f, spawn.y + y + 0.5f,-1);
            player.transform.position = movePos;
            
            MoveCamera(movePos);
            
            LevelGeneration.DungeonRooms[num].hasSpawnedPlayer = true;
        }

        private void SpawnTestPlayer()
        {
            Vector3 spawnPos = new Vector3(0.5f, 0.5f,-1);
            player = Instantiate(player, spawnPos, Quaternion.identity);
            MoveCamera(spawnPos);
        }

        private void MoveCamera(Vector3 pos)
        {
            _camera = Instantiate(cam, new Vector3(pos.x, pos.y, -10), Quaternion.identity)
                .GetComponentInChildren<CinemachineVirtualCamera>();
            _camera.Follow = player.transform;
        }

        public void AddUnitToList(EnemyController unit)
        {
            units.Add(unit);
        }

        private IEnumerator MoveUnits()
        {
            _unitsMoving = true;
            yield return new WaitForSeconds(turnDelay);
            
            if (units.Count == 0)
            {
                yield return new WaitForSeconds(turnDelay);
            }
            
            // Priority queue based on distance to player
            units.Sort();
            
            foreach(EnemyController unit in units)
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
