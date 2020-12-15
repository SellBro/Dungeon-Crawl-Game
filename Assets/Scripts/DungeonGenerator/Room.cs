using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.DungeonGenerator
{
    public class Room
    {
        public Vector2 gridPos;
        public int type;
        public bool doorTop;
        public bool doorBot;
        public bool doorLeft;
        public bool doorRight;

        public Room(Vector2 _gridPos, int _type)
        {
            gridPos = _gridPos;
            type = _type;
        }
    }
}
