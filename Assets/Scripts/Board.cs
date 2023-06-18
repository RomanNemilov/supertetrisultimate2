using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements;

public class Board : MonoBehaviour 
{
    public GameObject[] cubes;
    public Vector3 WorldPosition { get; private set; }
    public float WorldCellSize { get; private set; }
    public Vector2Int Size { get; private set; }
    public Game Game { get; private set; }
    public Piece Piece { get; private set; }
    System.Random rnd;
    private List<GameObject> _renderCubes;
    private int[,] _grid;

    // Update is called once per frame
    private void Update()
    {

    }

    private void Awake()
    {
        Size = new Vector2Int(10, 40);
        _grid = new int[Size.x, Size.y];
        WorldPosition = Vector3.zero;
        WorldCellSize = 1;
        cubes = new GameObject[14];
        cubes[0] = Resources.Load("Cube_I") as GameObject;
        cubes[1] = Resources.Load("Cube_O") as GameObject;
        cubes[2] = Resources.Load("Cube_T") as GameObject;
        cubes[3] = Resources.Load("Cube_J") as GameObject;
        cubes[4] = Resources.Load("Cube_L") as GameObject;
        cubes[5] = Resources.Load("Cube_S") as GameObject;
        cubes[6] = Resources.Load("Cube_Z") as GameObject;
        cubes[7] = Resources.Load("Cube_I_T") as GameObject;
        cubes[8] = Resources.Load("Cube_O_T") as GameObject;
        cubes[9] = Resources.Load("Cube_T_T") as GameObject;
        cubes[10] = Resources.Load("Cube_J_T") as GameObject;
        cubes[11] = Resources.Load("Cube_L_T") as GameObject;
        cubes[12] = Resources.Load("Cube_S_T") as GameObject;
        cubes[13] = Resources.Load("Cube_Z_T") as GameObject;
        Piece = gameObject.GetComponent<Piece>();
        Game = gameObject.GetComponent<Game>();
        rnd = new System.Random();
        _renderCubes = new List<GameObject>();
    }

    public void Set(Piece piece)
    {
        for (int x = 0; x < Piece.Grid.GetLength(0); x++)
        {
            for (int y = 0; y < Piece.Grid.GetLength(1); y++)
            {
                if (Piece.Grid[x, y] == 0)
                {
                    continue;
                }
                _grid[Piece.Position.x + x, Piece.Position.y + y] = Piece.Grid[x, y];
            }
        }
        ClearLines();
        Render();
    }

    private void ClearLines()
    {
        for (int y = 0; y < _grid.GetLength(1);)
        {
            if (IsLineFull(y))
            {
                LineClear(y);
            }
            else
            {
                y++;
            }
        }
    }

    private bool IsLineFull(int y)
    {
        for (int x = 0; x < _grid.GetLength(0); x++)
        {
            if (_grid[x, y] == 0)
            {
                return false;
            }
        }
        return true;
    }

    private void LineClear(int y)
    {
        // Clearing the highest row
        int highestRow = _grid.GetLength(1) - 1;
        for (int x = 0; x < _grid.GetLength(0); x++)
        {
            _grid[x, highestRow] = 0;
        }
        // Moving all upper lines to the line to be cleared
        for (;y < _grid.GetLength(1) - 1; y++)
        {
            for (int x = 0; x < _grid.GetLength(0); x++)
            {
                _grid[x, y] = _grid[x, y + 1]; 
            }
        }
    }

    public void ClearGrid()
    {
        _grid = new int[Size.x, Size.y];
        Render();
    }

    public void Render()
    {
        ClearRendered();
        for (int x = 0; x < _grid.GetLength(0); x++)
        {
            for (int y = 0; y < _grid.GetLength(1); y++)
            {
                int value = _grid[x, y];
                if (value == 0)
                {
                    continue;
                }
                Vector3 worldPosition = new Vector3(x * WorldCellSize,
                    y * WorldCellSize, 0)
                    + WorldPosition;
                //Vector3 worldPosition = Vector3.zero;
                _renderCubes.Add(Instantiate(cubes[value - 1], worldPosition, Quaternion.identity));
            }
        }
    }
    public void ClearRendered()
    {
        foreach (GameObject cube in _renderCubes)
        {
            Destroy(cube);
        }
        _renderCubes.Clear();
    }

    public void SpawnPiece()
    {
        Tetromino tetromino = (Tetromino)rnd.Next(7);
        Vector2Int position = new Vector2Int(4, 18);
        Piece.Initialize(this, position, tetromino);
        if (!Piece.Move(Vector2Int.down))
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        //Piece.Grid = null;
    }

    public bool IsValidPosition(int[,] grid, Vector2Int position)
    {
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                if (grid[x, y] == 0)
                {
                    continue;
                }
                Vector2Int tilePosition = new Vector2Int(x, y) + position;
                // Ñheking if tile is out of bounds
                if (tilePosition.x >= Size.x || tilePosition.y >= Size.y ||
                    tilePosition.x < 0 || tilePosition.y < 0)
                {
                    Debug.Log("Out of bounds");
                    return false;
                }
                // Ñheking if tile intersects with another tile
                if (_grid[tilePosition.x, tilePosition.y] != 0)
                {
                    Debug.Log("Intersection");
                    return false;
                }
            }
        }
        return true;
    }
}
