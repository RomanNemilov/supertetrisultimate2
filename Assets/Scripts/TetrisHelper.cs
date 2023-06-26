using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public static class TetrisHelper
{
    /// <summary>
    /// Returns all 7 tetrominos in random order
    /// </summary>
    public static List<Tetromino> Get7Tetrominos()
    {
        System.Random rnd = new System.Random();
        List<Tetromino> tetrominos = new List<Tetromino> {Tetromino.I, Tetromino.O,
        Tetromino.T, Tetromino.J, Tetromino.L, Tetromino.S, Tetromino.Z};
        List<Tetromino> bag = new List<Tetromino>();
        while (tetrominos.Count > 0)
        {
            int i = rnd.Next(tetrominos.Count);
            bag.Add(tetrominos[i]);
            tetrominos.RemoveAt(i);
        }
        return bag;
    }

    public static void SaveHighScores(List<HighScore> highScores)
    {
        HighScores highScoresObject = new HighScores() { highScores = highScores };
        string json = JsonUtility.ToJson(highScoresObject);
        PlayerPrefs.SetString("highScoreTable", json);
    }

    public static List<HighScore> GetHighScores() 
    {
        List<HighScore> highScores;
        string json = PlayerPrefs.GetString("highScoreTable");
        if (json.NullIfEmpty() == null)
        {
            highScores = new List<HighScore>();
            PlayerPrefs.SetString("highScoreTable", json);
        }
        else
        {
            highScores = JsonUtility.FromJson<HighScores>(json).highScores;
        }
        return highScores;
    }

    public static int[,] RotateMatrixClockwise(int[,] matrix)
    {
        int[,] resultMatrix = (int[,])matrix.Clone();
        int n = resultMatrix.GetLength(0); // assume matrix is square
        int layers = n / 2;
        layers.Equals(n);

        for (int layer = 0; layer < layers; layer++)
        {
            int first = layer;
            int last = n - 1 - layer;

            for (int i = first; i < last; i++)
            {
                int offset = i - first;

                // save top
                int top = resultMatrix[first, i];

                // left -> top
                resultMatrix[first, i] = resultMatrix[last - offset, first];

                // bottom -> left
                resultMatrix[last - offset, first] = resultMatrix[last, last - offset];

                // right -> bottom
                resultMatrix[last, last - offset] = resultMatrix[i, last];

                // top -> right
                resultMatrix[i, last] = top;
            }
        }
        //if (resultMatrix.Equals(matrix)) { Debug.Log("matixes are equal!"); }
        return resultMatrix;
    }
    public static int[,] RotateMatrixCounterclockwise(int[,] matrix)
    {
        int[,] resultMatrix = (int[,])matrix.Clone();
        int n = resultMatrix.GetLength(0); // assume matrix is square
        int layers = n / 2;

        for (int layer = 0; layer < layers; layer++)
        {
            int first = layer;
            int last = n - 1 - layer;

            for (int i = first; i < last; i++)
            {
                int offset = i - first;

                // save top
                int top = resultMatrix[first, i];

                // right -> top
                resultMatrix[first, i] = resultMatrix[i, last];

                // bottom -> right
                resultMatrix[i, last] = resultMatrix[last, last - offset];

                // left -> bottom
                resultMatrix[last, last - offset] = resultMatrix[last - offset, first];

                // top -> left
                resultMatrix[last - offset, first] = top;
            }
        }
        return resultMatrix;
    }

    public static int[,] GetTetrominoGrid(Tetromino tetromino)
    {
        int[,] tetrominoGrid;
        switch (tetromino)
        {
            case Tetromino.I:
                tetrominoGrid = new int[4, 4] {
                    { 0, 0, 1, 0 },
                    { 0, 0, 1, 0 },
                    { 0, 0, 1, 0 },
                    { 0, 0, 1, 0 }
                };
                break;
            case Tetromino.O:
                tetrominoGrid = new int[2, 2] {
                    {2, 2},
                    {2, 2},
                };
                break;
            case Tetromino.T:
                tetrominoGrid = new int[3, 3] {
                    { 0, 3, 0},
                    { 0, 3, 3},
                    { 0, 3, 0}
                };
                break;
            case Tetromino.J:
                tetrominoGrid = new int[3, 3] {
                    { 0, 4, 4},
                    { 0, 4, 0},
                    { 0, 4, 0}
                };
                break;
            case Tetromino.L:
                tetrominoGrid = new int[3, 3] {
                    { 0, 5, 0},
                    { 0, 5, 0},
                    { 0, 5, 5}
                };
                break;
            case Tetromino.S:
                tetrominoGrid = new int[3, 3] {
                    { 0, 6, 0},
                    { 0, 6, 6},
                    { 0, 0, 6}
                };
                break;
            case Tetromino.Z:
                tetrominoGrid = new int[3, 3] {
                    { 0, 0, 7},
                    { 0, 7, 7},
                    { 0, 7, 0}
                };
                break;
            default:
                tetrominoGrid = new int[4, 4];
                break;

        }
        return tetrominoGrid;
    }
}
