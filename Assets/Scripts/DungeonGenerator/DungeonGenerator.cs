using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Tile = UnityEngine.WSA.Tile;

namespace Roguelike.DungeonGenerator
{
    public class DungeonGenerator : MonoBehaviour
    {
        public const int MIN_ROOM_DELTA = 2;

        public int dungeonSize;

        [SerializeField] private int sight;

        [Range(1, 6)] 
        [SerializeField] private int numberOfIterations;
        
        [Range(1, 4)] 
        [SerializeField] private int corridorThickness;

        [SerializeField] private bool shouldDebugDrawBsp;
        [SerializeField] private Tilemap map;
        [SerializeField] private TileBase wallTile;

        
        [SerializeField] private Tilemap obstacleMap;
        [SerializeField] private Tilemap fogOfWarMap;
        [SerializeField] private GameObject player;

        private BspTree tree;
        private bool[,] dungeon;
        private AstarPath _astarPath;

        [Header("Tiles")] 
        [SerializeField] private TileBase[] floorTile;
        [SerializeField] private TileBase tlTile;
        [SerializeField] private TileBase tmTile;
        [SerializeField] private TileBase trTile;
        [SerializeField] private TileBase mlTile;
        [SerializeField] private TileBase mmTile;
        [SerializeField] private TileBase mrTile;
        [SerializeField] private TileBase blTile;
        [SerializeField] private TileBase bmTile;
        [SerializeField] private TileBase brTile;
        [Header("Corner Tiles")]
        //[SerializeField] private TileBase leftUp;
        [SerializeField] private TileBase leftDown;
        //[SerializeField] private TileBase rightUp;
        [SerializeField] private TileBase rightDown;
        
        [SerializeField] private TileBase fogOfWarTile;


        private void Start()
        {
            _astarPath = GetComponent<AstarPath>();
            dungeon = new bool[dungeonSize,dungeonSize];
            GenerateDungeon();
            CalculatePath();
            CreateFogOfWar();
            SpawnPlayer();
        }

        private void Update()
        {
            UpdateFogOfWar();
        }

        private void UpdateFogOfWar()
        {
            for (int i = -sight + (int)player.transform.position.x; i < sight + (int)player.transform.position.x; i++) {
                for (int j = -sight + (int)player.transform.position.y; j < sight + (int)player.transform.position.y; j++) {
                    fogOfWarMap.SetTile(new Vector3Int(i, j, 0), null);
                }
            }
        }

        public void CalculatePath()
        {
            _astarPath.Scan();
        }
        
        public void SpawnPlayer()
        {
            for (int i = 0; i < dungeonSize; i++) {
                for (int j = 0; j < dungeonSize; j++) {
                    var tile = GetTileByNeihbors (i, j);
                    if (tile == mmTile)
                    {
                        GameObject pl = Instantiate(player);
                        pl.transform.position = new Vector3(i + 0.5f, j + 0.5f);
                        player = pl;
                        return;
                    }
                }
            }
        }

        public void CreateFogOfWar()
        {
            for (int i = 0; i < dungeonSize; i++) 
            {
                for (int j = 0; j < dungeonSize; j++) 
                {
                    fogOfWarMap.SetTile(new Vector3Int(i, j, 0), fogOfWarTile);
                }
            }
        }

        public void GenerateDungeon()
        {
            InitReferences ();
            GenerateContainersUsingBsp ();
            GenerateRoomsInsideContainers ();
            GenerateCorridors ();
            FillRoomsOnTilemap ();
            PaintTilesAccordingToTheirNeighbors ();
        }
        
        private void InitReferences () {
            map.ClearAllTiles();
        }
        
        private void GenerateContainersUsingBsp () {
            tree = BspTree.Split (numberOfIterations, new RectInt (0, 0, dungeonSize, dungeonSize));
        }
        
        private void GenerateRoomsInsideContainers () {
            BspTree.GenerateRoomsInsideContainersNode (tree);
        }
        
        private void GenerateCorridors () {
            // For each parent
            // Find their center
            // Find a direction and connect these centers
            GenerateCorridorsNode (tree);
        }

        private void GenerateCorridorsNode (BspTree node) {
            if (node.IsInternal()) {
                RectInt leftContainer = node.left.container;
                RectInt rightContainer = node.right.container;
                Vector2 leftCenter = leftContainer.center;
                Vector2 rightCenter = rightContainer.center;
                Vector2 direction = (rightCenter - leftCenter).normalized; // Arbitrarily choosing right as the target point
                while (Vector2.Distance (leftCenter, rightCenter) > 1) {
                    if (direction.Equals (Vector2.right)) {
                        for (int i = 0; i < corridorThickness; i++) {
                            map.SetTile (new Vector3Int ((int) leftCenter.x, (int) leftCenter.y + i, 0), wallTile);
                        }
                    } else if (direction.Equals (Vector2.up)) {
                        for (int i = 0; i < corridorThickness; i++) {
                            map.SetTile (new Vector3Int ((int) leftCenter.x + i, (int) leftCenter.y, 0), wallTile);
                        }
                    }
                    leftCenter.x += direction.x;
                    leftCenter.y += direction.y;
                }
                if (node.left != null) GenerateCorridorsNode (node.left);
                if (node.right != null) GenerateCorridorsNode (node.right);
            }
        }
        
        private void FillRoomsOnTilemap () {
            UpdateTilemapUsingTreeNode (tree);
        }

        private void UpdateTilemapUsingTreeNode (BspTree node) {
            if (node.IsLeaf()) {
                for (int i = node.room.x; i < node.room.xMax; i++) {
                    for (int j = node.room.y; j < node.room.yMax; j++) {
                        map.SetTile (new Vector3Int (i, j, 0), wallTile);
                    }
                }
            } else {
                if (node.left != null) UpdateTilemapUsingTreeNode (node.left);
                if (node.right != null) UpdateTilemapUsingTreeNode (node.right);
            }
        }
        
        private TileBase GetTileByNeihbors (int i, int j)
        {
            var mmGridTile = map.GetTile (new Vector3Int (i, j, 0));
            if (mmGridTile == null) return null; // you shouldn't repaint a n

            var blGridTile = map.GetTile (new Vector3Int (i-1, j-1, 0));
            var bmGridTile = map.GetTile (new Vector3Int (i,   j-1, 0));
            var brGridTile = map.GetTile (new Vector3Int (i+1, j-1, 0));

            var mlGridTile = map.GetTile (new Vector3Int (i-1, j, 0));
            var mrGridTile = map.GetTile (new Vector3Int (i+1, j, 0));

            var tlGridTile = map.GetTile (new Vector3Int (i-1, j+1, 0));
            var tmGridTile = map.GetTile (new Vector3Int (i,   j+1, 0));
            var trGridTile = map.GetTile (new Vector3Int (i+1, j+1, 0));

            // we have 8 + 1 cases

            // left
            if (mlGridTile == null && tmGridTile == null) return tlTile;
            if (mlGridTile == null && tmGridTile != null && bmGridTile != null) return mlTile;
            if (mlGridTile == null && bmGridTile == null && tmGridTile != null) return blTile;

            // middle
            if (mlGridTile != null && tmGridTile == null && mrGridTile != null) return tmTile;
            if (mlGridTile != null && bmGridTile == null && mrGridTile != null) return bmTile;

            // right
            if (mlGridTile != null && tmGridTile == null && mrGridTile == null) return trTile;
            if (tmGridTile != null && bmGridTile != null && mrGridTile == null) return mrTile;
            if (tmGridTile != null && bmGridTile == null && mrGridTile == null) return brTile;
            
            // corners
            if (bmGridTile != null && mrGridTile != null && tlGridTile == null) return tmTile;
            if (bmGridTile != null && mlGridTile != null && blGridTile == null) return leftDown;
            if (tmGridTile != null && mrGridTile != null && trGridTile == null) return tmTile;
            if (bmGridTile != null && mrGridTile != null && brGridTile == null) return rightDown;

            return mmTile;
        }

        private void PaintTilesAccordingToTheirNeighbors () 
        {
            for (int i = 0; i < dungeonSize; i++) 
            {
                for (int j = 0; j < dungeonSize; j++) 
                {
                    var tile = GetTileByNeihbors (i, j);
                    if (tile != null)
                    {
                        if (tile != mmTile)
                        {
                            obstacleMap.SetTile(new Vector3Int(i, j, 0), tile);
                            map.SetTile(new Vector3Int(i, j, 0), tile);
                        }
                        else
                        {
                            int rand = Random.Range(0, floorTile.Length);
                            map.SetTile(new Vector3Int(i, j, 0), floorTile[rand]);
                            dungeon[i, j] = true;
                        }
                    }
                }
            }
        }

        #region Dubug

        void OnDrawGizmos () {
            AttemptDebugDrawBsp ();
        }

        private void OnDrawGizmosSelected () {
            AttemptDebugDrawBsp ();
        }

        void AttemptDebugDrawBsp () {
            if (shouldDebugDrawBsp) {
                DebugDrawBsp ();
            }
        }

        public void DebugDrawBsp () {
            if (tree == null) return; // hasn't been generated yet

            DebugDrawBspNode (tree); // recursive call
        }

        public void DebugDrawBspNode (BspTree node) {
            // Container
            Gizmos.color = Color.green;
            // top
            Gizmos.DrawLine (new Vector3 (node.container.x, node.container.y, 0), new Vector3Int (node.container.xMax, node.container.y, 0));
            // right
            Gizmos.DrawLine (new Vector3 (node.container.xMax, node.container.y, 0), new Vector3Int (node.container.xMax, node.container.yMax, 0));
            // bottom
            Gizmos.DrawLine (new Vector3 (node.container.x, node.container.yMax, 0), new Vector3Int (node.container.xMax, node.container.yMax, 0));
            // left
            Gizmos.DrawLine (new Vector3 (node.container.x, node.container.y, 0), new Vector3Int (node.container.x, node.container.yMax, 0));

            // children
            if (node.left != null) DebugDrawBspNode (node.left);
            if (node.right != null) DebugDrawBspNode (node.right);
        }

        #endregion
        
    }
        
    public class BspTree {
        public RectInt container;
        public RectInt room;
        
        public BspTree left;
        public BspTree right;

        BspTree(RectInt container)
        {
            this.container = container;
        }
        
        internal static BspTree Split (int numberOfIterations, RectInt container) {
            
            var node = new BspTree (container);
            if (numberOfIterations == 0) return node;

            var splittedContainers = SplitContainer (container,numberOfIterations);
            node.left = Split (numberOfIterations - 1, splittedContainers[0]);
            node.right = Split (numberOfIterations - 1, splittedContainers[1]);

            return node;
        }

        private static RectInt[] SplitContainer(RectInt container, int num)
        {
            RectInt c1, c2;
            // UnityEngine.Random.Range(0f, 1f) > 0.5f
            if (num % 2 == 0)
            {
                // Vertical
                c1 = new RectInt(container.x, container.y, container.width,
                    (int) UnityEngine.Random.Range(container.height * 0.3f, container.height * 0.5f));
                c2 = new RectInt (container.x, container.y + c1.height,
                    container.width, container.height - c1.height);
            }
            else
            {
                // Horizontal
                c1 = new RectInt (container.x, container.y,
                    (int) UnityEngine.Random.Range (container.width * 0.3f, container.width * 0.5f), container.height);
                c2 = new RectInt (container.x + c1.width, container.y,
                    container.width - c1.width, container.height);
            }
            
            return new RectInt[]{c1,c2};
        }
        
        public bool IsLeaf () {
            return left == null && right == null;
        }

        public bool IsInternal() { // De morgan's
            return left != null || right != null;
        }

        public static void GenerateRoomsInsideContainersNode(BspTree node)
        {
            // Should create rooms for Leafs
            if (node.left == null && node.right == null) {
                var randomX = UnityEngine.Random.Range(DungeonGenerator.MIN_ROOM_DELTA, node.container.width / 4);
                var randomY = UnityEngine.Random.Range(DungeonGenerator.MIN_ROOM_DELTA, node.container.height / 4);
                int roomX = node.container.x + randomX - 2;
                int roomY = node.container.y + randomY - 2;
                int roomW = node.container.width - (int) (randomX * UnityEngine.Random.Range(1f, 2f)) + 2;
                int roomH = node.container.height - (int) (randomY * UnityEngine.Random.Range(1f, 2f)) + 2;
                node.room = new RectInt(roomX, roomY, roomW, roomH);
            } else {
                if (node.left != null) GenerateRoomsInsideContainersNode(node.left);
                if (node.right != null) GenerateRoomsInsideContainersNode(node.right);
            }
        }
    }
}
