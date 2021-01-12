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
        [SerializeField] private Camera cam;
        
        [Header("Managing Components")]
        public static GameManager Instance = null;
        public BlockManager blockManager;
        public GameObject unitsManager;

        [Header("Lists")]
        public List<SingleNodeBlocker> obstacles;
        public List<EnemyController> _units;
        
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
            if (shouldGenerateLevel)
            {
                StartCoroutine(GenerateDungeon());
            }
            else
            {
                GenerateTestScene();
            }
            
            cam.GetComponentInChildren<CinemachineVirtualCamera>().Follow = player.transform;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) playerTurn = false;
            
            if (playerTurn || _unitsMoving) return;

            StartCoroutine(MoveUnits());
        }

        private void LateUpdate()
        {
            //Camera.main.transform.position = new Vector3(player.transform.position.x,player.transform.position.y,-10);
            cam.GetComponentInChildren<CinemachineVirtualCamera>().Follow = player.transform;
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

        private void GenerateTestScene()
        {
            SpawnTestPlayer();
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

        private void SpawnTestPlayer()
        {
            player = Instantiate(player, new Vector2( 0.5f, 0.5f), Quaternion.identity);
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
