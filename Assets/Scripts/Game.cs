using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public delegate void gameEventHandler();
    public event gameEventHandler scoreChanged;
    public int Level {  get; private set; }
    public int LinesClearedCount { get; private set; }
    public int Score 
    { 
        get 
        {
            return _score;
        } 
        private
        set
        {
            _score = value;
            scoreChanged?.Invoke();
            Debug.Log($"Score changed. New score: {Score}");
        }
    }
    private int _score;
    public Board Board { get; private set; }
    public Piece Piece { get; private set; }

    private void Awake()
    {
        Board = GetComponent<Board>();
        Piece = GetComponent<Piece>();
        Board.LinesCleared += LinesCleared;
        Piece.HardDrop += HardDrop;
        Piece.SoftDrop += SoftDrop;
    }

    public void StartGame()
    {
        Level = 1;
        LinesClearedCount = 0;
        Score = 0;
        Board.ClearGrid();
        Board.ClearFuturePieces();
        Board.SpawnPiece();
        Debug.Log("Game started");
    }

    public void LinesCleared(int lines)
    {
        int points;
        switch (lines)
        {
            case 2:
                points = 300; 
                break;
            case 3:
                points = 500;
                break;
            case 4:
                points = 800;
                break;
            default:
                points = lines * 100;
                break;
        }
        Score += Level * points;
    }
    public void HardDrop(int lines)
    {
        int points = lines * 2;
        Score += points;
    }
    public void SoftDrop(int lines)
    {
        int points = lines * 1;
        Score += points;
    }
}
