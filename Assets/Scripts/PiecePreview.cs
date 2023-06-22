using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PiecePreview : MonoBehaviour
{

    private Board _board;
    private Vector3 _worldPosition;
    private List<GameObject> _renderCubes;

    private void Awake()
    {
        _board = GetComponent<Board>();
        _worldPosition = new Vector3(13, 17);
        _renderCubes = new List<GameObject>();
    }

    public void Render(Tetromino piece)
    {
        int[,] grid = TetrisHelper.GetTetrominoGrid(piece);
        Vector2Int offset = new Vector2Int(0, 0);
        ClearRendered();
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                int value = grid[x, y];
                if (value == 0)
                {
                    continue;
                }
                Vector3 worldPosition = new Vector3((x + offset.x) * _board.WorldCellSize,
                    (y + offset.y) * _board.WorldCellSize, 0)
                    + _worldPosition;
                _renderCubes.Add(Instantiate(_board.cubes[value - 1], worldPosition, Quaternion.identity));
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
