using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Piece : MonoBehaviour
{
    public Board Board { get; private set; }
    public int[,] Grid { get; private set; }
    public Vector2Int Position { get; private set; }
    private Vector2Int GhostPosition { get; set; }
    public Tetromino Tetromino { get; private set; }
    public int Rotation { get; private set; }
    public bool Active { get; private set; }
    private List<GameObject> _renderCubes;

    public float stepDelay;
    public float lockDelay;

    private float _stepTime;
    private float _lockTime;


    private void Awake()
    {
        _renderCubes = new List<GameObject>();
        stepDelay = 1;
        lockDelay = 0;
        Active = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!Active) return;

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
        Vector2Int newPosition = Position + translation;
        bool valid = Board.IsValidPosition(Grid, newPosition);
        if (valid)
        {
            Position = newPosition;
            UpdateGhostPosition();
            _lockTime = 0;
            Render();
        }
        //else Debug.Log("Failed to move to x:" + newPositon.x + " y: " + newPositon.y);
        //if (translation == Vector2Int.down && _lockTime > lockDelay)
        //{
        //    Debug.Log("Lock due to failed atempt to move down");
        //    Lock();
        //}
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
            UpdateGhostPosition();
            _lockTime = 0;
            Render();
        }
        return valid;
    }


    private void HardDrop()
    {
        while (Move(Vector2Int.down)) { }
        Debug.Log("Lock due to harddrop");
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
        UpdateGhostPosition();
        Render();
        Active = true;
    }

    public void Stop()
    {
        Active = false;
    }

    private void Lock()
    {
        _lockTime = 0;
        if (Board.Set(this))
        {
            Board.SpawnPiece();
        }
        else
        {
            //Game over due to lock out
            Board.GameOver();
        }
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

    private void UpdateGhostPosition()
    {
        Vector2Int newPosition = Position;
        while (true)
        {
            newPosition += Vector2Int.down;
            bool valid = Board.IsValidPosition(Grid, newPosition);
            if (!valid)
            {
                GhostPosition = newPosition + Vector2Int.up;
                return;
            }
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
                // Rendering main piece block
                Vector3 worldPosition = new Vector3((x + Position.x) * Board.WorldCellSize, 
                    (y + Position.y) * Board.WorldCellSize, 0) 
                    + Board.WorldPosition;
                _renderCubes.Add(Instantiate(Board.cubes[value - 1], worldPosition, Quaternion.identity));
                // Rendering ghost piece block
                Vector3 worldGhostPosition = new Vector3((x + GhostPosition.x) * Board.WorldCellSize,
                    (y + GhostPosition.y) * Board.WorldCellSize, 0)
                    + Board.WorldPosition;
                _renderCubes.Add(Instantiate(Board.cubes[value - 1 + 7], worldGhostPosition, Quaternion.identity));
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
