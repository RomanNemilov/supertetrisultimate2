using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public Board Board { get; private set; }
    public int[,] Grid { get; private set; }
    public Vector2Int Position { get; private set; }
    public Tetromino Tetromino { get; private set; }
    public int Rotation { get; private set; }
    private List<GameObject> _renderCubes;
    
    public float stepDelay = 1;
    public float lockDelay = 0;

    private float _stepTime;
    private float _lockTime;


    // Start is called before the first frame update
    private void Start()
    {
        
    }

    private void Awake()
    {
        _renderCubes = new List<GameObject>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Grid == null) return;

        _lockTime += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.A))
        {
            Move(Vector2Int.left);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Move(Vector2Int.right);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Rotate(-1);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            Rotate(1);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Move(Vector2Int.down);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            HardDrop();
        }
        if (Time.time >= _stepTime)
        {
            Step();
        }

    }

    public bool Move(Vector2Int translation)
    {
        Vector2Int newPositon = Position + translation;
        bool valid = Board.IsValidPosition(this.Grid, newPositon);
        if (valid)
        {
            Position = newPositon;
            _lockTime = 0;
            Render();
        }
        if (translation == Vector2Int.down && _lockTime > lockDelay)
        {
            Lock();
        }
        return valid;
    }

    private bool Rotate(int rotation)
    {
        int[,] newGrid;
        if (rotation == 1)
        {
            newGrid = TetrisHelper.RotateMatrixClockwise(Grid);
        }
        else
        {
            newGrid = TetrisHelper.RotateMatrixCounterclockwise(Grid);
        }
        bool valid = Board.IsValidPosition(newGrid, Position);
        if (valid) 
        {
            Grid = newGrid;
            _lockTime = 0;
            Render();
        }
        return valid;
    }


    private void HardDrop()
    {
        while (Move(Vector2Int.down)) { }

        Lock();
    }

    public void Initialize(Board board, Vector2Int position, Tetromino tetromino)
    {
        Board = board;
        Position = position;
        Tetromino = tetromino;
        Grid = TetrisHelper.GetTetrominoGrid(tetromino);
        _stepTime = Time.time + stepDelay;
        _lockTime = 0;
        Render();
    }

    private void Lock()
    {
        Board.Set(this);
        Board.SpawnPiece();
    }

    private void Step()
    {
        _stepTime = Time.time + stepDelay;
        Move(Vector2Int.down);
        if (_lockTime > lockDelay)
        {
            Lock();
        }
    }

    public void Render()
    {
        ClearRendered();
        for (int x = 0;  x < Grid.GetLength(0); x++)
        {
            for (int y = 0;  y < Grid.GetLength(1); y++)
            {
                int value = Grid[x, y];
                if (value == 0)
                {
                    continue;
                }
                Vector3 worldPosition = new Vector3((x + Position.x) * Board.WorldCellSize, 
                    (y + Position.y) * Board.WorldCellSize, 0) 
                    + Board.WorldPosition;
                //Vector3 worldPosition = Vector3.zero;
                _renderCubes.Add(Instantiate(Board.cubes[value - 1], worldPosition, Quaternion.identity));
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
}
