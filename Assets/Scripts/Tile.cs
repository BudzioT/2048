using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


// A tile, the main part of the game
public class Tile : MonoBehaviour
{
    // State for quick state change
    public TileState State { get; private set; }
    // Current cell
    public Cell Cell { get; private set; }
    
    // Current number that is on the tile
    public int Number { get; private set; }
    // Background of a tile
    private Image _background;
    // Its text
    private TextMeshProUGUI _text;

    private void Awake()
    {
        // Initialize the components
        _background = GetComponent<Image>();
        _text = GetComponentInChildren<TextMeshProUGUI>();
    }


    // Set the state of a tile
    public void SetState(TileState state, int number)
    {
        // Set the state and number
        this.State = state;
        this.Number = number;

        // Set the colors
        _background.color = state.backgroundColor;
        _text.color = state.textColor;
        // Set number as a text
        _text.text = number.ToString();
    }

    // Spawn the cell with a tile
    public void Spawn(Cell cell)
    {
        // If this cell was already assigned, set it to null
        if (this.Cell != null) 
            this.Cell.Tile = null;
        
        // Set the cell and make this tile a part of it
        this.Cell = cell;
        this.Cell.Tile = this;

        // Make them the same position
        transform.position = cell.transform.position;
    }
}
