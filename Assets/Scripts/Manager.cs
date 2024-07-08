using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Game manager class
public class Manager : MonoBehaviour
{
    public TileBoard board;

    private void Start()
    {
        // Start the game
        NewGame();
    }
    
    // Prepare a new game
    public void NewGame()
    {
        // Clear the board
        board.ClearBoard();
        
        // Create two tiles to begin
        board.CreateTile();
        board.CreateTile();
        
        // Enable the board to allow input
        board.enabled = true;
    }

    // Handle losing
    public void GameOver()
    {
        // Disable the board, prevent from input
        board.enabled = false;
    }
}
