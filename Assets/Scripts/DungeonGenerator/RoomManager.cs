using SellBro.Core;
using SellBro.Units;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace SellBro.DungeonGenerator
{
    public class RoomManager : MonoBehaviour
    {
        public Vector2Int position;
        
        [HideInInspector] public bool hasSpawnedPlayer = false;
        
        [SerializeField] private Tilemap obstacleMap;
        [SerializeField] private Tilemap groundMap;

        private bool[,] _tiles = new bool[17,16];

        private void Start()
        {
            if (obstacleMap == null || groundMap == null)
            {
                Debug.LogError("Maps == null" + gameObject.name);
                return;
            }
            
            position = new Vector2Int((int)(transform.position.x - 8), (int)(transform.position.y - 8));

            FindEmptySpaces();
        }

        public void FillRoom()
        {
            SpawBoxes();
            SpawnEnemies();
        }

        private void FindEmptySpaces()
        {
            // Fill bool array; True - empty space
            for (int i = -8; i < 9; i++)
            {
                for (int j = -8; j < 8; j++)
                {
                    if (obstacleMap.GetTile(new Vector3Int(i, j,0)) == null &&
                        groundMap.GetTile(new Vector3Int(i, j, 0)) != null)
                    {
                        _tiles[i + 8,j + 8] = true;
                    }
                    else
                    {
                        _tiles[i + 8,j + 8] = false;
                    }
                }
            }
        }

        private void SpawBoxes()
        {
            int boxNumber = Random.Range(0, LevelGeneration.Instance.maxBoxNumberPerRoom);
            
            for(int i = 3; i < 14; i++)
            {
                for(int j = 3; j < 13; j++)
                {
                    int rand = Random.Range(0, 100);

                    if (rand <= LevelGeneration.Instance.boxGenerationChance && _tiles[i,j] && boxNumber > 0)
                    {
                        SpawBox(i,j);
                        boxNumber--;
                    }
                    
                    if (boxNumber <= 0)
                        return;
                }
            }
        }

        private void SpawBox(int x, int y)
        {
            Spaw(LevelGeneration.Instance.boxPrefab, x, y);


            bool top, bot, left, right;
            top = IsTileEmpty(x, y + 1);
            bot = IsTileEmpty(x, y - 1);
            left = IsTileEmpty(x - 1, y);
            right = IsTileEmpty(x + 1, y);

            float rand = Random.Range(0, 1f);
            if (rand > 0.5 && top)
            {
                Spaw(LevelGeneration.Instance.boxPrefab, x, y + 1);
            }
            rand = Random.Range(0, 1f);
            if (rand > 0.5 && bot)
            {
                Spaw(LevelGeneration.Instance.boxPrefab, x, y - 1);
            }
            rand = Random.Range(0, 1f);
            if (rand > 0.5 && left)
            {
                Spaw(LevelGeneration.Instance.boxPrefab, x - 1, y);
            }
            rand = Random.Range(0, 1f);
            if (rand > 0.5 && right)
            {
                Spaw(LevelGeneration.Instance.boxPrefab, x + 1, y);
            }
        }

        private void SpawnEnemies()
        {
            if (hasSpawnedPlayer) return;
            
            int enemiesType = Random.Range(0, LevelGeneration.Instance.enemies.Length);
            int enemiesNumber = Random.Range(0, LevelGeneration.Instance.maxEnemiesPerRoom);
            
            for(int i = 3; i < 14; i++)
            {
                for(int j = 3; j < 13; j++)
                {
                    int rand = Random.Range(0, 100);

                    if (rand <= LevelGeneration.Instance.boxGenerationChance && _tiles[i,j] && enemiesNumber > 0)
                    {
                        SpawnEnemy(LevelGeneration.Instance.enemies[enemiesType], i, j);
                        enemiesNumber--;
                    }
                    
                    if (enemiesNumber <= 0)
                        return;
                }
            }
        }

        private void Spaw(GameObject obj, int x, int y)
        {
            Instantiate(obj, new Vector3(position.x + x + 0.5f, position.y + y + 0.5f, 0),
                Quaternion.identity,transform.parent);
            _tiles[x, y] = false;
        }

        private void SpawnEnemy(GameObject obj, int x, int y)
        {
            var enemy = Instantiate(obj, new Vector3(position.x + x + 0.5f, position.y + y + 0.5f, 0),
                Quaternion.identity,GameManager.Instance.unitsManager.transform.parent);
            _tiles[x, y] = false;
            enemy.GetComponent<EnemyController>().SetStartingRoom(this);
        }

        public Vector2 GetRandomTile()
        {
            int x;
            int y;
            
            for (int i = 0; i < 10;i++)
            {
                x = Random.Range(1, 16);
                y = Random.Range(1, 15);
                
                if (IsTileEmpty(x, y))
                {
                    return new Vector3(position.x + x,position.y + y,0);
                }
            }
            
            return Vector2.zero;
        }
        
        public bool IsTileEmpty(int x, int y)
        {
            return _tiles[x, y];
        }
    }
}
