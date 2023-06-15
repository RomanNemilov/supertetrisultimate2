using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Board : MonoBehaviour 
{
    public GameObject cube;
    public Vector3 WorldPosition { get; private set; }
    public float WorldCellSize { get; private set; }
    public Vector2Int Size { get; private set; }
    public Piece Piece { get; private set; }
    System.Random rnd;
    private int[,] _grid;

    // Start is called before the first frame update
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {

    }

    private void Awake()
    {
        Size = new Vector2Int(10, 20);
        _grid = new int[10, 20];
        WorldPosition = Vector3.zero;
        WorldCellSize = 1;
        //cube = Resources.Load("Cube") as GameObject;
        //Piece = gameObject.AddComponent<Piece>();
        Piece = gameObject.GetComponent<Piece>();
        rnd = new System.Random();
    }

    public void StartGame()
    {
        SpawnPiece();
        

        //Instantiate(cube, new Vector3(0, 0, 0), Quaternion.identity);



        Debug.Log("Game started");
        Debug.Log("First piece: +" + (int)Piece.Tetromino);
    }

    private void SpawnPiece()
    {
        Vector2Int position = new Vector2Int(4, 17);
        Tetromino tetromino = (Tetromino)rnd.Next(7);
        Piece.Initialize(this, position, tetromino);
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
                    return false;
                }
                // Ñheking if tile intersects with another tile
                if (_grid[tilePosition.x, tilePosition.y] == 1)
                {
                    return false;
                }
            }
        }
        return true;
    }
}
