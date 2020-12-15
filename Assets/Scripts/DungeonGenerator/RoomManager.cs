using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RPG.DungeonGenerator
{
    public class RoomManager : MonoBehaviour
    {
        [SerializeField] private Tilemap obstacleMap;
        [SerializeField] private Tilemap groundMap;

        private bool[,] _tiles = new bool[16,15];

        private void Start()
        {
            if (obstacleMap == null || groundMap == null)
            {
                Debug.LogError("Maps == null" + gameObject.name);
                return;
            }

            // Fill bool array; True - empty space
            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    if (obstacleMap.GetTile(new Vector3Int(i, j, 0)) == null &&
                        groundMap.GetTile(new Vector3Int(i, j, 0)) != null)
                    {
                        _tiles[i,j] = true;
                    }
                    else
                    {
                        _tiles[i,j] = false;
                    }
                }
            }
        }
    }
}
