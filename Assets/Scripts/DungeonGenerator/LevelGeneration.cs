using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RPG.DungeonGenerator
{
    public class LevelGeneration : MonoBehaviour
    {
	    public static LevelGeneration Instance = null;
	    
	    public static List<RoomManager> DungeonRooms;

	    [Header("Tiles")]
	    [SerializeField] private GameObject[] T;
	    [SerializeField] private GameObject[] B;
	    [SerializeField] private GameObject[] R;
	    [SerializeField] private GameObject[] L;
	    [SerializeField] private GameObject[] TB;
	    [SerializeField] private GameObject[] RL;
	    [SerializeField] private GameObject[] TR;
	    [SerializeField] private GameObject[] TL;
	    [SerializeField] private GameObject[] BR;
	    [SerializeField] private GameObject[] BL;
	    [SerializeField] private GameObject[] TBL;
	    [SerializeField] private GameObject[] TRL;
	    [SerializeField] private GameObject[] TBR;
	    [SerializeField] private GameObject[] BRL;
	    [SerializeField] private GameObject[] TBRL;

	    [Header("Settings")] 
	    [Range(1, 10)] 
	    [SerializeField] private int worldX = 3;
	    [Range(1, 10)] 
	    [SerializeField] private int worldY = 3;
	    [SerializeField] private Transform mapRoot;
	    [SerializeField] private int roomSizeX = 16;
	    [SerializeField] private int roomSizeY = 15;
	    
	    [Header("InRoom Generation")]
	    [Range(1,100)]
	    public int boxGenerationChance = 2;
	    public int maxBoxNumberPerRoom = 2;
	    public GameObject boxPrefab;
	    
	    [Header("Enemies")] 
	    public GameObject[] enemies;
	    public int maxEnemiesPerRoom = 8;
	    
	    
	    private Vector2 _worldSize = new Vector2(3,3);
        private Room[,] _rooms;
        private List<Vector2> _takenPositions = new List<Vector2>();
        private int _gridSizeX;
        private int _gridSizeY;
        private int _numberOfRooms = 20;

        private void Awake()
        {
	        Instance = this;
        }

        public IEnumerator GenerateLevel()
        {
	        DungeonRooms = new List<RoomManager>();
	        
	        _worldSize = new Vector2(worldX,worldY);
	        // Make sure we don't try to make more rooms than can fit in our grid
	        if (_numberOfRooms >= (_worldSize.x * 2) * (_worldSize.y * 2))
	        { 
		        _numberOfRooms = Mathf.RoundToInt((_worldSize.x * 2) * (_worldSize.y * 2));
	        }
	        // Define grid size
	        _gridSizeX = Mathf.RoundToInt(_worldSize.x);
	        _gridSizeY = Mathf.RoundToInt(_worldSize.y);
			
	        // Lays out the actual map
	        CreateRooms(); 
	        // Assigns the doors where rooms would connect
	        SetRoomDoors();
	        // Instantiates objects to make up a map
	        DrawMap();
	        yield return null;
        }

        public IEnumerator FillRooms()
        {
	        foreach (RoomManager room in DungeonRooms)
	        {
		        room.FillRoom();
		        yield return null;
	        }
        }
        

        void CreateRooms(){
			// Setup
			_rooms = new Room[_gridSizeX * 2,_gridSizeY * 2];
			_rooms[_gridSizeX,_gridSizeY] = new Room(Vector2.zero, 1);
			
			_takenPositions.Insert(0,Vector2.zero);
			Vector2 checkPos = Vector2.zero;
			
			// Magic numbers
			float randomCompare = 0.2f;
			float randomCompareStart = 0.2f;
			float randomCompareEnd = 0.01f;
			
			// Add rooms
			for (int i =0; i < _numberOfRooms -1; i++)
			{
				float randomPerc = ((float) i) / (((float)_numberOfRooms - 1));
				randomCompare = Mathf.Lerp(randomCompareStart, randomCompareEnd, randomPerc);
				
				// Grab new position
				checkPos = NewPosition();
				
				// Test new position
				if (NumberOfNeighbors(checkPos, _takenPositions) > 1 && Random.value > randomCompare)
				{
					int iterations = 0;
					
					do
					{
						checkPos = SelectiveNewPosition();
						iterations++;
					}
					while(NumberOfNeighbors(checkPos, _takenPositions) > 1 && iterations < 100);
					
					//if (iterations >= 50)
						//print("error: could not create with fewer neighbors than : " + NumberOfNeighbors(checkPos, _takenPositions));
					
				}
				
				// Finalize position
				_rooms[(int) checkPos.x + _gridSizeX, (int) checkPos.y + _gridSizeY] = new Room(checkPos, 0);
				_takenPositions.Insert(0,checkPos);
			}	
		}
		
		
		Vector2 NewPosition()
		{
			int x = 0;
			int y = 0;
			
			Vector2 checkingPos = Vector2.zero;
			
			do
			{
				// Pick a random room
				int index = Mathf.RoundToInt(Random.value * (_takenPositions.Count - 1)); 
				x = (int) _takenPositions[index].x;
				y = (int) _takenPositions[index].y;
				
				// Randomly pick wether to look on hor or vert axis
				bool UpDown = (Random.value < 0.5f);
				// Pick whether to be positive or negative on that axis
				bool positive = (Random.value < 0.5f);
				
				if (UpDown){
					if (positive){
						y += 1;
					}else{
						y -= 1;
					}
				}else{
					if (positive){
						x += 1;
					}else{
						x -= 1;
					}
				}
				
				checkingPos = new Vector2(x,y);
				
			} //make sure the position is valid
			while (_takenPositions.Contains(checkingPos) || x >= _gridSizeX || x < -_gridSizeX || y >= _gridSizeY || y < -_gridSizeY); 
			
			return checkingPos;
		}
		
		
		/// Method differs from the above in the two commented ways
		Vector2 SelectiveNewPosition()
		{
			int index = 0;
			int inc = 0;
			int x = 0;
			int y =0;
			Vector2 checkingPos = Vector2.zero;
			
			do{
				inc = 0;
				
				do
				{ 
					/* Instead of getting a room to find an adject empty space, we start with one that only 
					   has one neighbor. This will make it more likely that it returns a room that branches out */
					index = Mathf.RoundToInt(Random.value * (_takenPositions.Count - 1));
					inc++;
				}
				while (NumberOfNeighbors(_takenPositions[index], _takenPositions) > 1 && inc < 100);
				
				x = (int) _takenPositions[index].x;
				y = (int) _takenPositions[index].y;
				
				bool UpDown = (Random.value < 0.5f);
				bool positive = (Random.value < 0.5f);
				
				if (UpDown)
				{
					if (positive)
					{
						y += 1;
					}else
					{
						y -= 1;
					}
				}
				else
				{
					if (positive)
					{
						x += 1;
					}else
					{
						x -= 1;
					}
				}
				
				checkingPos = new Vector2(x,y);
				
			}
			while (_takenPositions.Contains(checkingPos) || x >= _gridSizeX || x < -_gridSizeX || y >= _gridSizeY || y < -_gridSizeY);
			// Break loop if it takes too long: this loop isn't guaranteed to find solution, which is fine for this
			if (inc >= 100)
			{ 
				//print("Error: could not find position with only one neighbor");
			}
			return checkingPos;
		}
		
		
		int NumberOfNeighbors(Vector2 checkingPos, List<Vector2> usedPositions)
		{
			// Start at zero, add 1 for each side there is already a room
			int ret = 0; 
			
			if (usedPositions.Contains(checkingPos + Vector2.right))
			{ 
				// Using Vector.[direction] as short hands, for simplicity
				ret++;
			}
			if (usedPositions.Contains(checkingPos + Vector2.left))
			{
				ret++;
			}
			if (usedPositions.Contains(checkingPos + Vector2.up))
			{
				ret++;
			}
			if (usedPositions.Contains(checkingPos + Vector2.down))
			{
				ret++;
			}
			
			return ret;
		}
		
		
		void DrawMap()
		{
			foreach (Room room in _rooms)
			{
				// Skip where there is no room
				if (room == null)
				{
					continue; 
				}
				
				Vector2 drawPos = room.gridPos;
				
				// Aspect ratio of map sprite
				drawPos.x *= roomSizeX;
				drawPos.y *= roomSizeY;
				
				// Create map obj and assign its variables
				var r = Instantiate(PickObject(room), drawPos, Quaternion.identity);
				r.gameObject.transform.parent = mapRoot;
				RoomManager manager = r.gameObject.GetComponent<RoomManager>();
				DungeonRooms.Add(manager);
			}
		}
		
		
		void SetRoomDoors()
		{
			for (int x = 0; x < _gridSizeX * 2; x++)
			{
				for (int y = 0; y < _gridSizeY * 2; y++)
				{
					if (_rooms[x,y] == null)
					{
						continue;
					}
					
					Vector2 gridPosition = new Vector2(x,y);

					if (y - 1 < 0)
					{ 
						// Check above
						_rooms[x,y].doorBot = false;
					}
					else
					{
						_rooms[x,y].doorBot = (_rooms[x,y-1] != null);
					}
					
					if (y + 1 >= _gridSizeY * 2)
					{ 
						// Check bellow
						_rooms[x,y].doorTop = false;
					}
					else
					{
						_rooms[x,y].doorTop = (_rooms[x,y+1] != null);
					}
					
					if (x - 1 < 0)
					{ 
						// Check left
						_rooms[x,y].doorLeft = false;
					}
					else
					{
						_rooms[x,y].doorLeft = (_rooms[x - 1,y] != null);
					}
					
					if (x + 1 >= _gridSizeX * 2)
					{ 
						// Check right
						_rooms[x,y].doorRight = false;
					}
					else
					{
						_rooms[x,y].doorRight = (_rooms[x+1,y] != null);
					}
				}
			}
		}
	
		
		GameObject PickObject(Room room)
		{ 
			// Picks correct sprite based on the four door bools
			if (room.doorTop)
			{
				if (room.doorBot)
				{
					if (room.doorRight)
					{
						if (room.doorLeft)
						{
							return TBRL[Random.Range(0,TBRL.Length)];
						}
						else
						{
							return TBR[Random.Range(0,TBR.Length)];
						}
					}
					else if (room.doorLeft)
					{
						return TBL[Random.Range(0,TBL.Length)];
					}
					else
					{
						return TB[Random.Range(0,TB.Length)];
					}
				}
				else
				{
					if (room.doorRight)
					{
						if (room.doorLeft)
						{
							return TRL[Random.Range(0,TRL.Length)];
						}
						else
						{
							return TR[Random.Range(0,TR.Length)];
						}
					}
					else if (room.doorLeft)
					{
						return TL[Random.Range(0,TL.Length)];
					}
					else
					{
						return T[Random.Range(0,T.Length)];
					}
				}
				return null;
			}
			
			if (room.doorBot)
			{
				if (room.doorRight)
				{
					if(room.doorLeft)
					{
						return BRL[Random.Range(0,BRL.Length)];
					}
					else
					{
						return BR[Random.Range(0,BR.Length)];
					}
				}
				else if (room.doorLeft){
					
					return BL[Random.Range(0,BL.Length)];
				}
				else
				{
					return B[Random.Range(0,B.Length)];
				}
				return null;
			}
			
			if (room.doorRight){
				if (room.doorLeft)
				{
					return RL[Random.Range(0,RL.Length)];
				}else
				{
					return R[Random.Range(0,R.Length)];
				}
			}
			else
			{
				return L[Random.Range(0,L.Length)];
			}
		}
		
		
    }
}
