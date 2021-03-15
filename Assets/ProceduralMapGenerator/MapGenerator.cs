using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TileType
{
    Floor,
    Void
}

public enum BaseTileSetType
{
    Dungeon,
    MagicWorld,
    Catacombs,
    Dungenon2
}

public class MapGenerator : MonoBehaviour, IGameManager
{
    [Header("Tilemaps")]
    [SerializeField] Tilemap floorTilemap;
    [SerializeField] Tilemap voidTilemap;
    [SerializeField] Tilemap wallTilemap;
    [SerializeField] Tilemap wallTopDTilemap;
    [SerializeField] Tilemap wallTopUTilemap;
    [SerializeField] Tilemap wallTopLTilemap;
    [SerializeField] Tilemap wallTopRTilemap;

    [Header("Tilebases")]
    [SerializeField] List<TileBaseSet> tileBaseSet;
    public BaseTileSetType tileBaseSetType;

    public Action OnMapGenerated;
    public ManagerStatus Status { get; private set; }
    public Vector3 PlayerSpawnPosition { get; private set; }
    public Vector3 BossSpawnPosition { get; private set; }

    [Header("Map Generation")]
    public int width;
    public int height;
    [Range(0, 100)]
    public int randomFillPercent;
    public string seed;
    public bool useRandomSeed;
    public int smoothIterations;
    public int minimumWallSize = 15;
    public int minimumRoomSize = 15;
    public int passageRadius = 2;
    [Range(0, 10)]
    public int unitSpawnPercent = 5;
    float unitSpawnRate;

    int[,] map;
    TileType[,] tileTypeMap;
    public TileType[,] TileTypeMap => tileTypeMap;

    void Start()
    {
        //GenerateMap();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            GenerateMap();
        }
    }

    void KillAllUnits()
    {
        if (Managers.UnitGenerator.CurrentUnits.Count > 0)
            foreach (GameObject unit in Managers.UnitGenerator.CurrentUnits)
                Destroy(unit);
    }

    void DestroyAllItems()
    {
        if (Managers.ItemGenerator.CurrentItems.Count > 0)
            foreach (GameObject item in Managers.ItemGenerator.CurrentItems)
                Destroy(item);
    }

    public void Startup()
    {
        Debug.Log("Map generator starting...");
        Status = ManagerStatus.Started;
    }

    void ClearAllTilemaps()
    {
        floorTilemap.ClearAllTiles();
        voidTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();
        wallTopDTilemap.ClearAllTiles();
        wallTopUTilemap.ClearAllTiles();
        wallTopLTilemap.ClearAllTiles();
        wallTopRTilemap.ClearAllTiles();
    }

    public void GenerateMap()
    {
        unitSpawnRate = unitSpawnPercent / 100f;
        ClearAllTilemaps();
        KillAllUnits();
        DestroyAllItems();

        map = new int[width, height];
        tileTypeMap = new TileType[width, height];

        RandomFillMap();

        for (int i = 0; i < smoothIterations; i++)
        {
            SmoothMap();
        }

        ProcessMap();

        Managers.UnitGenerator.SpawnUnit(UnitType.Boss, BossSpawnPosition);

        //Invoke this event when the map is generated
        OnMapGenerated?.Invoke();
    }

    void ProcessMap()
    {
        //Fill all wall tiles below minimum wall size with floor tiles
        List<List<Coord>> wallRegions = GetRegions(1);

        foreach (List<Coord> wallRegion in wallRegions)
        {
            if (wallRegion.Count < minimumWallSize)
            {
                foreach (Coord tile in wallRegion)
                    map[tile.tileX, tile.tileY] = 0;
            }
        }

        //Fill all floor tiles
        List<List<Coord>> roomRegions = GetRegions(0);

        //New list of rooms
        List<Room> survivingRooms = new List<Room>();

        //Loop through each room region in all room regions
        foreach (List<Coord> roomRegion in roomRegions)
        {
            //If room size is less than minimus size
            if (roomRegion.Count < minimumRoomSize)
            {
                //Loop through all the tiles in this room
                foreach (Coord tile in roomRegion)
                    //Change this tile to wall tile
                    map[tile.tileX, tile.tileY] = 1;
            }
            else
                //Add region to list of rooms
                survivingRooms.Add(new Room(roomRegion, map));
        }

        //Sort rooms from largest to smallest
        survivingRooms.Sort();

        //Get the middle coord from the smallest room and convert it to world position
        PlayerSpawnPosition = CoordToWorldPosition(survivingRooms[survivingRooms.Count - 1].tiles[survivingRooms[survivingRooms.Count - 1].tiles.Count / 2]);
        //Get the middle coord from the largest room and convert it to world position
        BossSpawnPosition = CoordToWorldPosition(survivingRooms[0].tiles[survivingRooms[0].tiles.Count / 2]);

        for (int i = 0; i < survivingRooms.Count - 1; i++)
        {
            for (int j = 0; j < survivingRooms[i].tiles.Count; j++)
            {
                if (UnityEngine.Random.Range(0f, 1f) < unitSpawnRate)
                    Managers.UnitGenerator.SpawnUnit(new Vector3Int(-width / 2 + survivingRooms[i].tiles[j].tileX, -height / 2 + survivingRooms[i].tiles[j].tileY, 0) + new Vector3(0.5f, 0.5f, 0.5f));
            }
        }

        survivingRooms[0].isMainRoom = true;
        survivingRooms[0].isAccessibleFromMainRoom = true;
        ConnectClosestRooms(survivingRooms);

        //Fill tiletype map
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (map[x, y] == 0)
                    tileTypeMap[x, y] = TileType.Floor;
                else if (map[x, y] == 1)
                    tileTypeMap[x, y] = TileType.Void;
            }
        }

        FillWithTiles();
    }

    void FillWithTiles()
    {
        //Fill the map
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (tileTypeMap[x, y] == TileType.Floor)
                {
                    floorTilemap.SetTile(new Vector3Int(-width / 2 + x, -height / 2 + y, 0), tileBaseSet[(int)tileBaseSetType].floorRuleTile);
                }
                else if (tileTypeMap[x, y] == TileType.Void)
                {
                    voidTilemap.SetTile(new Vector3Int(-width / 2 + x, -height / 2 + y, 0), tileBaseSet[(int)tileBaseSetType].voidRuleTile);
                    wallTilemap.SetTile(new Vector3Int(-width / 2 + x, -height / 2 + y, 0), tileBaseSet[(int)tileBaseSetType].wallRuleTile);
                    wallTopDTilemap.SetTile(new Vector3Int(-width / 2 + x, -height / 2 + y, 0), tileBaseSet[(int)tileBaseSetType].wallTopDRuleTile);
                    wallTopUTilemap.SetTile(new Vector3Int(-width / 2 + x, -height / 2 + y, 0), tileBaseSet[(int)tileBaseSetType].wallTopURuleTile);
                    wallTopLTilemap.SetTile(new Vector3Int(-width / 2 + x, -height / 2 + y, 0), tileBaseSet[(int)tileBaseSetType].wallTopsLRuleTile);
                    wallTopRTilemap.SetTile(new Vector3Int(-width / 2 + x, -height / 2 + y, 0), tileBaseSet[(int)tileBaseSetType].wallTopsRRuleTile);
                }
            }
        }
    }

    void ConnectClosestRooms(List<Room> allRooms, bool forceAccessibilityFromMainRoom = false)
    {
        List<Room> roomListA = new List<Room>();
        List<Room> roomListB = new List<Room>();

        //Trying to connect all the rooms in list A which aren't accesible from main room to any room from ListB which are accessible from main room
        if (forceAccessibilityFromMainRoom)
        {
            foreach (Room room in allRooms)
            {
                if (room.isAccessibleFromMainRoom)
                {
                    roomListB.Add(room);
                }
                else
                    roomListA.Add(room);
            }
        }
        else
        {
            roomListA = allRooms;
            roomListB = allRooms;
        }

        int bestDistance = 0;
        Coord bestTileA = new Coord();
        Coord bestTileB = new Coord();
        Room bestRoomA = new Room();
        Room bestRoomB = new Room();
        bool possibleConnectionFound = false;

        //Loop through all rooms in list A
        foreach (Room roomA in roomListA)
        {
            if (!forceAccessibilityFromMainRoom)
            {
                possibleConnectionFound = false;

                //Skip room if we have connections
                if (roomA.connectedRooms.Count > 0)
                    continue;
            }

            //Loop through all rooms in list B
            foreach (Room roomB in roomListB)
            {
                //If it is the same room skip to the next loop || roomA is connected to B
                if (roomA == roomB || roomA.IsConnected(roomB))
                    continue;

                //Loop through all border tiles in room A
                for (int tileIndexA = 0; tileIndexA < roomA.borderTiles.Count; tileIndexA++)
                {
                    //Loop through all border tiles in room B
                    for (int tileIndexB = 0; tileIndexB < roomB.borderTiles.Count; tileIndexB++)
                    {
                        //Set current tile A
                        Coord tileA = roomA.borderTiles[tileIndexA];
                        //Set current tile B
                        Coord tileB = roomB.borderTiles[tileIndexB];

                        //Get distance between current room border tiles
                        int distanceBetweenRooms = (int)(Mathf.Pow(tileA.tileX - tileB.tileX, 2) + Mathf.Pow(tileA.tileY - tileB.tileY, 2));

                        //If this distance is less than best distance || we did not find any connection yet
                        if (distanceBetweenRooms < bestDistance || !possibleConnectionFound)
                        {
                            //Set some variables
                            bestDistance = distanceBetweenRooms;
                            possibleConnectionFound = true;
                            bestTileA = tileA;
                            bestTileB = tileB;
                            bestRoomA = roomA;
                            bestRoomB = roomB;
                        }
                    }
                }
            }

            //If there is a connection found
            if (possibleConnectionFound && !forceAccessibilityFromMainRoom)
                //Make a passage
                CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
        }

        if (possibleConnectionFound && forceAccessibilityFromMainRoom)
        {
            //Make a passage
            CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
            //Calling method again to look for any more connections  that need to be made
            ConnectClosestRooms(allRooms, true);
        }

        if (!forceAccessibilityFromMainRoom)
            ConnectClosestRooms(allRooms, true);
    }

    void CreatePassage(Room roomA, Room roomB, Coord tileA, Coord tileB)
    {
        //Connects given rooms
        Room.ConnectRooms(roomA, roomB);
        Debug.DrawLine(CoordToWorldPosition(tileA), CoordToWorldPosition(tileB), Color.green, 100f);
        List<Coord> line = GetLine(tileA, tileB);
        foreach (Coord c in line)
            DrawCircle(c, passageRadius);
    }

    //Set radius for our passage
    void DrawCircle(Coord c, int r)
    {
        for (int x = -r; x <= r; x++)
        {
            for (int y = -r; y <= r; y++)
            {
                if (x * x + y * y <= r * r)
                {
                    //actual point that we are drawing
                    int drawX = c.tileX + x;
                    int drawY = c.tileY + y;
                    if (IsInMapRange(drawX, drawY))
                    {
                        map[drawX, drawY] = 0;
                    }
                }
            }
        }
    }

    List<Coord> GetLine(Coord from, Coord to)
    {
        List<Coord> line = new List<Coord>();

        int x = from.tileX;
        int y = from.tileY;

        int dx = to.tileX - from.tileX;
        int dy = to.tileY - from.tileY;

        bool inverted = false;
        //increment x each step, positively or negatrively depens of sign of delta x
        int step = Math.Sign(dx);
        //Step where Y changes
        int gradientStep = Math.Sign(dy);

        int longest = Mathf.Abs(dx);
        int shortest = Mathf.Abs(dy);

        if (longest < shortest)
        {
            inverted = true;
            longest = Mathf.Abs(dy);
            shortest = Mathf.Abs(dx);

            step = Math.Sign(dy);
            gradientStep = Math.Sign(dx);
        }
        int gradientAccumulation = longest / 2;
        for (int i = 0; i < longest; i++)
        {
            line.Add(new Coord(x, y));

            if (inverted)
            {
                y += step;
            }
            else
            {
                x += step;
            }
            gradientAccumulation += shortest;
            if (gradientAccumulation >= longest)
            {
                if (inverted)
                {
                    x += gradientStep;
                }
                else
                {
                    y += gradientStep;
                }
                gradientAccumulation -= longest;
            }
        }
        return line;
    }

    Vector3 CoordToWorldPosition(Coord tile)
    {
        return new Vector3(-width / 2 + 0.5f + tile.tileX, -height / 2 + 0.5f + tile.tileY, 0f);
    }

    List<List<Coord>> GetRegions(int tileType)
    {
        //New list of cord list
        List<List<Coord>> regions = new List<List<Coord>>();

        //Array to know if we already visited a tile or not
        int[,] mapFlags = new int[width, height];

        //Loop through whole map
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                //If tile was not already visited && tile is correct tile type
                if (mapFlags[x, y] == 0 && map[x, y] == tileType)
                {
                    //Get a new list of coords starting from current x and y
                    List<Coord> newRegion = GetRegionTiles(x, y);

                    //Add new region to list of regions
                    regions.Add(newRegion);

                    //Flag all coordinates in new region as visited
                    foreach (Coord tile in newRegion)
                    {
                        mapFlags[tile.tileX, tile.tileY] = 1;
                    }
                }
            }
        }

        return regions;
    }

    List<Coord> GetRegionTiles(int startX, int startY)
    {
        //New list of coords
        List<Coord> tiles = new List<Coord>();

        //Array to know if we already visited a tile or not
        int[,] mapFlags = new int[width, height];

        //Type of tiles too look for
        int tileType = map[startX, startY];

        //Nowa kolejeczka coodrow
        Queue<Coord> queue = new Queue<Coord>();
        //Add starting coordinate to queue
        queue.Enqueue(new Coord(startX, startY));
        //Flag our starting point as looked at tile
        mapFlags[startX, startY] = 1;

        //While there is something left in the queue
        while (queue.Count > 0)
        {
            //Dequeue first item in queue and assign it to variable
            Coord tile = queue.Dequeue();
            //Add dequeued tile to tiles list
            tiles.Add(tile);

            //Loop through vertical and horizontal neighbours
            for (int x = tile.tileX - 1; x <= tile.tileX + 1; x++)
            {
                for (int y = tile.tileY - 1; y <= tile.tileY + 1; y++)
                {
                    //If tile is inside map bounds && tile is no diagonal to original tile
                    if (IsInMapRange(x, y) && (y == tile.tileY || x == tile.tileX))
                    {
                        //If we did not already check this tile && tile is a correct tile type
                        if (mapFlags[x, y] == 0 && map[x, y] == tileType)
                        {
                            mapFlags[x, y] = 1;
                            //Add new coordinate to queue
                            queue.Enqueue(new Coord(x, y));
                        }
                    }
                }
            }
        }

        return tiles;
    }

    bool IsInMapRange(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }

    void RandomFillMap()
    {
        if (useRandomSeed)
        {
            seed = UnityEngine.Random.Range(0, int.MaxValue).ToString();
        }

        //return unique hash code for the seed
        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                //setting walls on the edge of the map
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    map[x, y] = 1;
                }
                else
                    map[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? 1 : 0;
            }
        }
    }

    //Checks the neighbors and sets the tile based on that
    void SmoothMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighbourWallTiles = GetSurroundingWallCount(x, y);

                if (neighbourWallTiles > 4)
                {
                    map[x, y] = 1;
                }
                else if (neighbourWallTiles < 4)
                {
                    map[x, y] = 0;
                }
            }
        }
    }

    int GetSurroundingWallCount(int gridX, int gridY)
    {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                if (IsInMapRange(neighbourX, neighbourY))
                {
                    if (neighbourX != gridX || neighbourY != gridY)
                        wallCount += map[neighbourX, neighbourY];
                }
                else
                {
                    wallCount++;
                }
            }
        }
        return wallCount;
    }

    struct Coord
    {
        public int tileX;
        public int tileY;

        public Coord(int x, int y)
        {
            tileX = x;
            tileY = y;
        }
    }

    //A calss for storing room
    class Room : IComparable<Room>
    {
        public List<Coord> tiles;
        public List<Coord> borderTiles;
        public List<Room> connectedRooms;
        public int roomSize;
        public bool isAccessibleFromMainRoom;
        public bool isMainRoom;

        //Empty constructor
        public Room() { }

        public Room(List<Coord> roomTiles, int[,] map)
        {
            tiles = roomTiles;
            roomSize = tiles.Count;
            connectedRooms = new List<Room>();
            borderTiles = new List<Coord>();

            //Loop through all room tiles
            foreach (Coord tile in tiles)
            {
                //Loop through tile neighbours
                for (int x = tile.tileX - 1; x <= tile.tileX + 1; x++)
                {
                    for (int y = tile.tileY - 1; y <= tile.tileY + 1; y++)
                    {
                        //Exclude diagonal neighbours
                        //xox
                        //ooo
                        //xox
                        if (x == tile.tileX || y == tile.tileY)
                            //If tile is a wall
                            if (map[x, y] == 1)
                                //Add this tile to border tile list
                                borderTiles.Add(tile);
                    }
                }
            }
        }

        //If one of them is accessible from the main room, we want to set other one equal to accessible
        public void SetAccessibleFromMainRoom()
        {
            if (!isAccessibleFromMainRoom)
            {
                isAccessibleFromMainRoom = true;
                foreach (Room connectedRoom in connectedRooms)
                {
                    connectedRoom.SetAccessibleFromMainRoom();
                }
            }
        }

        public static void ConnectRooms(Room roomA, Room roomB)
        {
            //Update the accessibiltiy of all connected rooms when two rooms are connected
            if (roomA.isAccessibleFromMainRoom)
                roomB.SetAccessibleFromMainRoom();
            else if (roomB.isAccessibleFromMainRoom)
                roomA.SetAccessibleFromMainRoom();

            roomA.connectedRooms.Add(roomB);
            roomB.connectedRooms.Add(roomA);
        }

        public bool IsConnected(Room otherRoom)
        {
            return connectedRooms.Contains(otherRoom);
        }

        public int CompareTo(Room otherRoom)
        {
            return otherRoom.roomSize.CompareTo(roomSize);
        }
    }
}
