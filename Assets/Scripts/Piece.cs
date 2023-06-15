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
    private List<GameObject> _renderCubes = new List<GameObject>();


    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (Grid == null) return;

        if (Input.GetKeyDown(KeyCode.A))
        {
            Move(Vector2Int.left);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Move(Vector2Int.right);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Rotate(-1);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            Rotate(1);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            Move(Vector2Int.up);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Move(Vector2Int.down);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            HardDrop();
        }
    }

    private bool Move(Vector2Int translation)
    {
        Vector2Int newPositon = Position + translation;
        bool valid = Board.IsValidPosition(this.Grid, newPositon);
        if (valid)
        {
            Position = newPositon;
            Render();
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
        bool value = Board.IsValidPosition(newGrid, Position);
        if (value) 
        {
            Grid = newGrid;
            Render();
        }
        return value;
    }


    private void HardDrop()
    {
        while (Move(Vector2Int.down)) { }
    }

    public void Initialize(Board board, Vector2Int position, Tetromino tetromino)
    {
        Board = board;
        Position = position;
        Tetromino = tetromino;
        Grid = TetrisHelper.GetTetrominoGrid(tetromino);
        Render();
    }

    public void Render()
    {
        ClearRendered();
        for (int x = 0;  x < Grid.GetLength(0); x++)
        {
            for (int y = 0;  y < Grid.GetLength(1); y++)
            {
                if (Grid[x, y] == 0)
                {
                    continue;
                }
                Vector3 worldPosition = new Vector3((x + Position.x) * Board.WorldCellSize, 
                    (y + Position.y) * Board.WorldCellSize, 0) 
                    + Board.WorldPosition;
                //Vector3 worldPosition = Vector3.zero;
                _renderCubes.Add(Instantiate(Board.cube, worldPosition, Quaternion.identity));
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
