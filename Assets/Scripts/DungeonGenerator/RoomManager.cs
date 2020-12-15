using UnityEngine;
using UnityEngine.Tilemaps;

namespace RPG.DungeonGenerator
{
    public class RoomManager : MonoBehaviour
    {
        public Vector2Int position;
        
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

        public bool IsTileEmpty(int x, int y)
        {
            return _tiles[x, y];
        }
    }
}
