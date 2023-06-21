using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public int Level {  get; private set; }
    public int LinesCleared { get; private set; }
    public int Score {  get; private set; }
    public Board Board { get; private set; }
    public void StartGame()
    {
        Level = 1;
        LinesCleared = 0;
        Score = 0;
        Board.ClearGrid();
        Board.ClearFuturePieces();
        Board.SpawnPiece();
        Debug.Log("Game started");
    }

    private void Awake()
    {
        Board = GetComponent<Board>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
