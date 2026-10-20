
using System.Diagnostics;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public const int JimSize = 6;
    public const int WallSize = 11;

    // 0 = empty, 1 = wall, 2 = trap, 3 = objective, 4 = corner
    [SerializeField] private int[] wallGrid = new int[WallSize * WallSize];

    public bool HasWall(int x, int y)
    {
        return TileType(x,y) == 1;
    }

    public int TileType(int x, int y)
    {
        if (x < 0 || x >= WallSize || y < 0 || y >= WallSize) return 1;
        if (x % 2 == 1 && y % 2 ==1) return 4;
        return wallGrid[y * WallSize + x];
    }

    public bool IsWallBetween(Vector2Int cell, Vector2Int direction)
    {
        int wallX = cell.x + direction.x;
        int wallY = cell.y + direction.y;

        return TileType(wallX, wallY) == 1;
    }

    
}

