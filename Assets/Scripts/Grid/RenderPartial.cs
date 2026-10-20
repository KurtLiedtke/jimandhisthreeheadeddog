using UnityEngine;

public class RenderPartial : MonoBehaviour
{
    [SerializeField] private GridManager grid;

    [Header("Prefabs")]
    [SerializeField] private GameObject floorPrefab;
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject objective;
    [SerializeField] private GameObject trap;
    [SerializeField] private GameObject cornerPrefab;
    
    [Header("Needed Prefabs")]
    [SerializeField] private int[] renderControl;


    [Header("Maze Settings")]
    [SerializeField] private int gridSize = 11;
    [SerializeField] private float cellSize = 1f;
    [SerializeField] private Vector2 jimOrigin = Vector2.zero;
    [SerializeField] private Vector2 timofyOrigin = Vector2.zero;
    [SerializeField] private Vector2 lacyOrigin = Vector2.zero;
    [SerializeField] private Vector2 dougingtonOrigin = Vector2.zero;

    void Start()
    {
        DrawMaze();
    }

    void DrawMaze()
    {
        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                //generate boards
                JimBoard(x,y,grid.TileType(x,y));
                TimofyBoard(x,y,grid.TileType(x,y));
                LacyBoard(x,y,grid.TileType(x,y));
                DougingtonBoard(x,y,grid.TileType(x,y));
            }
        }
    }

    void JimBoard(int x, int y, int tileType)
    {
        if (tileType == 4) return;
        if (x % 2 == 1 || y % 2 == 1) return;
        Generator(floorPrefab, x, y, jimOrigin, Quaternion.identity);
    }

    void TimofyBoard(int x, int y, int tileType)
    {
        if(tileType == 4) return;

        if((x % 2 == 1 || y % 2 == 1) && tileType == 1)
        {
            Quaternion angle = y % 2 == 1 ? Quaternion.Euler(0,0,90) : Quaternion.identity;
            Generator(wallPrefab, x, y, timofyOrigin, angle);
            return;
        }

        if((x % 2 == 1 || y % 2 == 1) && tileType != 1) return;

        Generator(floorPrefab, x, y, timofyOrigin, Quaternion.identity);
    }

    void LacyBoard(int x, int y, int tileType)
    {
        if (tileType == 4) return;
        if (x % 2 == 1 || y % 2 == 1) return;

        if (tileType == 2)
        {
            Generator(trap, x, y, lacyOrigin, Quaternion.identity);
            return;
        }

        Generator(floorPrefab, x, y, lacyOrigin, Quaternion.identity);
    }

    void DougingtonBoard(int x, int y, int tileType)
    {
        if (tileType == 4) return;
        if (x % 2 == 1 || y % 2 == 1) return;

        if (tileType == 3)
        {
            Generator(objective, x, y, dougingtonOrigin, Quaternion.identity);
            return;
        }

        Generator(floorPrefab, x, y, dougingtonOrigin, Quaternion.identity);
    }

    

    void Generator(GameObject prefab, int x, int y, Vector2 origin, Quaternion angle)
    {
        Vector3 position = new Vector3(origin.x + x * cellSize, origin.y + y * cellSize,0);
        Instantiate(prefab, position, angle, transform);
    }
}
