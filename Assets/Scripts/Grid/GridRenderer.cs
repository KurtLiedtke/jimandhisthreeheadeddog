
using NUnit.Framework;
using UnityEngine;
public class GridRenderer : MonoBehaviour
{
    [SerializeField] private GridManager grid;

    [Header("Maze Settings")]
    [SerializeField] private int gridSize = 11;
    [SerializeField] private float cellSize = 1f;
    [SerializeField] private Vector2 origin = Vector2.zero;

    [Header("Prefabs")]
    [SerializeField] private GameObject floorPrefab;
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject cornerPrefab;
    [SerializeField] private GameObject objective;
    [SerializeField] private GameObject trap;


    private void Start()
    {
        DrawMaze();
    }

    public void DrawMaze()
    {
        // Clear the old maze if we redraw it.
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                if(x % 2 == 1 && y % 2 == 1) continue;
                bool isWallPosition = x % 2 == 1 || y % 2 == 1;
                if (isWallPosition && !grid.HasWall(x,y)) continue;
                bool makeWall = isWallPosition && grid.HasWall(x,y);
                Vector3 position = new Vector3(origin.x + x * cellSize, origin.y + y * cellSize,0);

                GameObject prefab;
                switch (grid.TileType(x, y))
                {
                    default:
                    case 0:
                        prefab = floorPrefab;
                    break;
                    case 1:
                        prefab = wallPrefab;
                    break;
                    case 2:
                        prefab = trap;
                    break;
                    case 3:
                        prefab = objective;
                    break;
                }
                

                GameObject tile = Instantiate(prefab, position, Quaternion.identity, transform);
                if(makeWall && y % 2 == 1)
                {
                    tile.transform.rotation = Quaternion.Euler(0,0,90);
                }
                
            }
        }

        for (int y = 0; y < gridSize; y++)
        {
            for(int x = 0; x < gridSize; x++)
            {
                if(!(x % 2 == 1) || !(y % 2 == 1)) continue;

                int count = 0;
                if (grid.HasWall(x+1,y)) count++;
                if (grid.HasWall(x,y+1)) count++;
                if (grid.HasWall(x-1,y)) count++;
                if (grid.HasWall(x,y-1)) count++;

                if(count > 1){
                    Vector3 position = new Vector3(origin.x + x * cellSize, origin.y + y * cellSize,0);
                    GameObject tile = Instantiate(cornerPrefab, position, Quaternion.identity, transform);
                }


            }
        }
    }
}
