using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelGenerator : MonoBehaviour {
	enum gridSpace {empty, floor, wall};
	gridSpace[,] grid;
	int roomHeight, roomWidth;
	Vector2 roomSizeWorldUnits = new Vector2(60,60);
	float worldUnitsInOneGridCell = 1f;
	struct walker{
		public Vector2 dir;
		public Vector2 pos;
	}
	public Tilemap waterMap;
	public Tilemap floorMap;
	public Tilemap wallMap;
	public Tile floorTile;
	public Tile waterTile;
	public Tile wallTile;

	List<walker> walkers;
	float chanceWalkerChangeDir = 0.5f, chanceWalkerSpawn = 0.05f;
	float changeWalkerDestroy = 0.05f;
	int maxWalkers = 10;
	float percentToFill = 0.5f; 
	public GameObject waterObj, floorObj, wallObj;
	//for enemy generation
	public int enemyCount = 12;
	public GameObject enemy;
	[Range(1, 25)]
	public int spawnChance;
	Task task;
	//---

	bool ready = false;
	void Start () {
		Setup();
		CreateFloors();
		CreateWalls();
		RemoveSingleWalls();
		TwoGap();
		SpawnLevel();
        Nav.LoadNav();
        SpawnEnemies();

    }
	void Setup(){
		//find grid size
		roomHeight = Mathf.RoundToInt(roomSizeWorldUnits.x / worldUnitsInOneGridCell);
		roomWidth = Mathf.RoundToInt(roomSizeWorldUnits.y / worldUnitsInOneGridCell);
		//create grid
		grid = new gridSpace[roomWidth,roomHeight];
		//set grid's default state
		for (int x = 0; x < roomWidth-1; x++){
			for (int y = 0; y < roomHeight-1; y++){
				//make every cell "empty"
				grid[x,y] = gridSpace.empty;
			}
		}
		//set first walker
		//init list
		walkers = new List<walker>();
		//create a walker 
		walker newWalker = new walker();
		newWalker.dir = RandomDirection();
		//find center of grid
		Vector2 spawnPos = new Vector2(Mathf.RoundToInt(roomWidth/ 2.0f),
										Mathf.RoundToInt(roomHeight/ 2.0f));
		newWalker.pos = spawnPos;
		//add walker to list
		walkers.Add(newWalker);
	}
	void CreateFloors(){
		int iterations = 0;//loop will not run forever
		do{
			//create floor at position of every walker
			foreach (walker myWalker in walkers){
				grid[(int)myWalker.pos.x,(int)myWalker.pos.y] = gridSpace.floor;
			}
			//chance: destroy walker
			int numberChecks = walkers.Count; //might modify count while in this loop
			for (int i = 0; i < numberChecks; i++){
				//only if its not the only one, and at a low chance
				if (Random.value < changeWalkerDestroy && walkers.Count > 1){
					walkers.RemoveAt(i);
					break; //only destroy one per iteration
				}
			}
			//chance: walker pick new direction
			for (int i = 0; i < walkers.Count; i++){
				if (Random.value < chanceWalkerChangeDir){
					walker thisWalker = walkers[i];
					thisWalker.dir = RandomDirection();
					walkers[i] = thisWalker;
				}
			}
			//chance: spawn new walker
			numberChecks = walkers.Count; //might modify count while in this loop
			for (int i = 0; i < numberChecks; i++){
				//only if # of walkers < max, and at a low chance
				if (Random.value < chanceWalkerSpawn && walkers.Count < maxWalkers){
					//create a walker 
					walker newWalker = new walker();
					newWalker.dir = RandomDirection();
					newWalker.pos = walkers[i].pos;
					walkers.Add(newWalker);
				}
			}
			//move walkers
			for (int i = 0; i < walkers.Count; i++){
				walker thisWalker = walkers[i];
				thisWalker.pos += thisWalker.dir;
				walkers[i] = thisWalker;
			}
			//avoid boarder of grid
			for (int i =0; i < walkers.Count; i++){
				walker thisWalker = walkers[i];
				//clamp x,y to leave a 1 space boarder: leave room for walls
				thisWalker.pos.x = Mathf.Clamp(thisWalker.pos.x, 1, roomWidth-2);
				thisWalker.pos.y = Mathf.Clamp(thisWalker.pos.y, 1, roomHeight-2);
				walkers[i] = thisWalker;
			}
			//check to exit loop
			if ((float)NumberOfFloors() / (float)grid.Length > percentToFill){
				break;
			}
			iterations++;
		}while(iterations < 100000);
	}
	void CreateWalls(){
		//loop though every grid space
		for (int x = 0; x < roomWidth-1; x++){
			for (int y = 0; y < roomHeight-1; y++){
				//if theres a floor, check the spaces around it
				if (grid[x,y] == gridSpace.floor){
					//if any surrounding spaces are empty, place a wall
					if (grid[x,y+1] == gridSpace.empty){
						grid[x,y+1] = gridSpace.wall;
					}
					if (grid[x,y-1] == gridSpace.empty){
						grid[x,y-1] = gridSpace.wall;
					}
					if (grid[x+1,y] == gridSpace.empty){
						grid[x+1,y] = gridSpace.wall;
					}
					if (grid[x-1,y] == gridSpace.empty){
						grid[x-1,y] = gridSpace.wall;
					}
				}
			}
		}
	}

	//Broaden the "halls" to make the game more open
	void TwoGap()
	{
		for (int x = 3; x < roomWidth - 3; x++)
		{
			for (int y = 3; y < roomHeight - 3; y++)
			{
				//if theres a floor, check the spaces around it
				if (grid[x, y] == gridSpace.floor)
				{
					//make sure there is more than 1 floor tile
					if (grid[x, y + 1] == gridSpace.wall && grid[x, y - 1] == gridSpace.wall)
					{
						grid[x, y + 1] = gridSpace.floor;
						grid[x, y - 1] = gridSpace.floor;

						if (grid[x, y + 3] == gridSpace.empty)
						{
							grid[x, y + 2] = gridSpace.wall;
						}

						if (grid[x, y - 3] == gridSpace.empty)
						{
							grid[x, y - 2] = gridSpace.wall;
						}
					}

					if (grid[x + 1, y] == gridSpace.wall && grid[x - 1, y] == gridSpace.wall)
					{
						grid[x + 1, y] = gridSpace.floor;
						grid[x - 1, y] = gridSpace.floor;

						if (grid[x + 3, y] == gridSpace.empty)
						{
							grid[x + 2, y ] = gridSpace.wall;
						}

						if (grid[x - 3, y] == gridSpace.empty)
						{
							grid[x - 2, y] = gridSpace.wall;
						}
					}
				}
			}
		}
	}
	void RemoveSingleWalls(){
		//loop though every grid space
		for (int x = 0; x < roomWidth-1; x++){
			for (int y = 0; y < roomHeight-1; y++){
				//if theres a wall, check the spaces around it
				if (grid[x,y] == gridSpace.wall){
					//assume all space around wall are floors
					bool allFloors = true;
					//check each side to see if they are all floors
					for (int checkX = -1; checkX <= 1 ; checkX++){
						for (int checkY = -1; checkY <= 1; checkY++){
							if (x + checkX < 0 || x + checkX > roomWidth - 1 || 
								y + checkY < 0 || y + checkY > roomHeight - 1){
								//skip checks that are out of range
								continue;
							}
							if ((checkX != 0 && checkY != 0) || (checkX == 0 && checkY == 0)){
								//skip corners and center
								continue;
							}
							if (grid[x + checkX,y+checkY] != gridSpace.floor){
								allFloors = false;
							}
						}
					}
					if (allFloors){
						grid[x,y] = gridSpace.floor;
					}
				}
			}
		}
	}
	void SpawnLevel(){
		for (int x = 0; x < roomWidth; x++){
			for (int y = 0; y < roomHeight; y++){
				switch(grid[x,y]){
					case gridSpace.empty:
						Spawn(x, y, wallObj);
						break;
					case gridSpace.floor:
						Spawn(x,y,floorObj);
						break;
					case gridSpace.wall:
						Spawn(x,y,waterObj);
						break;
				}
			}
		}
	}
    void SpawnEnemies(){
    	for (int x = 0; x < roomWidth; x++){
    		for (int y = 0; y < roomHeight; y++){
    			switch(grid[x,y]){
    				case gridSpace.floor:
                        SpawnSingleEnemy(x,y);
    					break;
    			}
    		}
    	}
    }
    
    void SpawnSingleEnemy(int x, int y)
    {
        Vector2 offset = roomSizeWorldUnits / 2.0f;
        Vector2 spawnPos = new Vector2(x, y) * worldUnitsInOneGridCell - offset;
    
        int rand = Mathf.FloorToInt(Random.value * 100.00f);
        if (enemyCount > 0 && rand < spawnChance && grid[x + 1, y] != gridSpace.wall && grid[x - 1, y] != gridSpace.wall && grid[x, y + 1] != gridSpace.wall && grid[x, y - 1] != gridSpace.wall)
        {
            Vector3 spawnVec = Vector3Int.FloorToInt(new Vector2(x, y) * worldUnitsInOneGridCell - offset);
            Instantiate(enemy, spawnVec, Quaternion.identity);
            --enemyCount;
        }
    }
	Vector2 RandomDirection(){
		//pick random int between 0 and 3
		int choice = Mathf.FloorToInt(Random.value * 3.99f);
		//use that int to chose a direction
		switch (choice){
			case 0:
				return Vector2.down;
			case 1:
				return Vector2.left;
			case 2:
				return Vector2.up;
			default:
				return Vector2.right;
		}
	}
	int NumberOfFloors(){
		int count = 0;
		foreach (gridSpace space in grid){
			if (space == gridSpace.floor){
				count++;
			}
		}
		return count;
	}
	void Spawn(int x, int y, GameObject toSpawn){
		//find the position to spawn
		
		Vector2 offset = roomSizeWorldUnits / 2.0f;
		Vector2 spawnPos = new Vector2(x,y) * worldUnitsInOneGridCell - offset;
		//spawn object
		if (toSpawn == floorObj)
		{
			Vector3Int floorVec = Vector3Int.FloorToInt(new Vector2(x, y) * worldUnitsInOneGridCell - offset);
			floorMap.SetTile(floorVec, floorTile);
		}

		if (toSpawn == waterObj)
		{
			Vector3Int waterVec = Vector3Int.FloorToInt(new Vector2(x, y) * worldUnitsInOneGridCell - offset);
			waterMap.SetTile(waterVec, waterTile);
		}
		
		if (toSpawn == wallObj)
		{
			Vector3Int wallVec = Vector3Int.FloorToInt(new Vector2(x, y) * worldUnitsInOneGridCell - offset);
			wallMap.SetTile(wallVec, wallTile);
		}
        //int rand = Mathf.FloorToInt(Random.value * 100.00f);

        //if (toSpawn == floorObj && enemyCount > 0)
        //    if (rand < spawnChance && grid[x + 1, y] != gridSpace.wall && grid[x - 1, y] != gridSpace.wall && grid[x, y + 1] != gridSpace.wall && grid[x, y - 1] != gridSpace.wall)
        //    {
        //        Vector3 spawnVec = Vector3Int.FloorToInt(new Vector2(x, y) * worldUnitsInOneGridCell - offset);
        //        Instantiate(enemy, spawnVec, Quaternion.identity);
        //        --enemyCount;
        //    }
    }
}
