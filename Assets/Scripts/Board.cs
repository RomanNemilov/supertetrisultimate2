using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements;
using static Game;

public class Board : MonoBehaviour 
{
    public delegate void scoreEventHandler(int lines);
    public delegate void gameEventHandler();
    public event scoreEventHandler LinesCleared;
    public event gameEventHandler GameOver;
    private System.Random rnd;
    private Queue<Tetromino> _futurePieces;
    private List<GameObject> _renderCubes;
    private int[,] _grid;
    public GameObject[] cubes;
    public Vector3 WorldPosition { get; private set; }
    public float WorldCellSize { get; private set; }
    public Vector2Int Size { get; private set; }
    public Vector2Int VisibleSize { get; private set; }
    public Game Game { get; private set; }
    public Piece Piece { get; private set; }
    private PiecePreview _piecePreview;
    public Tetromino NextPiece
    {
        get { return _futurePieces.Peek(); }
    }

    private void Awake()
    {
        Size = new Vector2Int(10, 40);
        VisibleSize = new Vector2Int(10, 20);
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
        Game = gameObject.GetComponent<Game>();
        Piece = gameObject.GetComponent<Piece>();
        _piecePreview = gameObject.GetComponent<PiecePreview>();
        rnd = new System.Random();
        _renderCubes = new List<GameObject>();
        _futurePieces = new Queue<Tetromino>();
        AddFuturePieces();
    }

    public bool Set(Piece piece)
    {
        bool visible = false;
        for (int x = 0; x < Piece.Grid.GetLength(0); x++)
        {
            for (int y = 0; y < Piece.Grid.GetLength(1); y++)
            {
                if (Piece.Grid[x, y] == 0)
                {
                    continue;
                }
                _grid[Piece.Position.x + x, Piece.Position.y + y] = Piece.Grid[x, y];
                if (Piece.Position.x + x < VisibleSize.x && Piece.Position.y + y < VisibleSize.y)
                {
                    visible = true;
                }
            }
        }
        ClearLines();
        Render();
        return visible;
    }

    private void ClearLines()
    {
        int linesCleared = 0;
        for (int y = 0; y < _grid.GetLength(1);)
        {
            if (IsLineFull(y))
            {
                LineClear(y);
                linesCleared++;
            }
            else
            {
                y++;
            }
        }
        if (linesCleared > 0)
        {
            LinesCleared?.Invoke(linesCleared);
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
        for (int x = 0; x < VisibleSize.x; x++)
        {
            for (int y = 0; y < VisibleSize.y; y++)
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
        if (_futurePieces.Count <= 7) 
        {
            AddFuturePieces();
        }
        Tetromino tetromino = _futurePieces.Dequeue();
        Vector2Int position;
        switch (tetromino)
        {
            case Tetromino.I:
                position = new Vector2Int(3, 18);
                break;
            case Tetromino.O:
                position = new Vector2Int(4, 20);
                break;
            case Tetromino.T:
            case Tetromino.J:
            case Tetromino.L:
            case Tetromino.S:
            case Tetromino.Z:
                position = new Vector2Int(3, 19);
                break;
            default:
                position = new Vector2Int(0,0);
                break;
        }
        Piece.Initialize(this, position, tetromino);
        if (!IsValidPosition(Piece.Grid, Piece.Position))
        {
            StopGame();
        }
        _piecePreview.Render(NextPiece);
        Debug.Log("Spawned piece: " +  Piece.Tetromino + " Next piece: " + NextPiece);
    }

    public void StopGame()
    {
        Piece.Stop();
        GameOver?.Invoke();
        Debug.Log("Game over");
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
                    //Debug.Log("Out of bounds");
                    return false;
                }
                // Ñheking if tile intersects with another tile
                if (_grid[tilePosition.x, tilePosition.y] != 0)
                {
                    //Debug.Log("Intersection");
                    return false;
                }
            }
        }
        return true;
    }

    public void AddFuturePieces()
    {
        foreach (Tetromino piece in TetrisHelper.Get7Tetrominos())
        {
            _futurePieces.Enqueue(piece);
        }
    }

    public void ClearFuturePieces()
    {
        _futurePieces.Clear();
    }
}
