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
    public TileCell TileCell { get; private set; }
    
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
    public void Spawn(TileCell tileCell)
    {
        // If this cell was already assigned, set it to null
        if (this.TileCell != null) 
            this.TileCell.Tile = null;
        
        // Set the cell and make this tile a part of it
        this.TileCell = tileCell;
        this.TileCell.Tile = this;

        // Make them the same position
        transform.position = tileCell.transform.position;
    }

    // Move to a given cell
    public void Move(TileCell cell)
    {
        // If this cell was already occupied, reset it
        if (this.TileCell != null) 
            this.TileCell.Tile = null;
        
        // Make a new cell with this one
        this.TileCell = cell;
        this.TileCell.Tile = this;

        // Animate and move the tile
        StartCoroutine(Animate(cell.transform.position));
    }

    // Animate the tile movement to a new position
    private IEnumerator Animate(Vector3 newPos)
    {
        // Duration and elapsed time of the animation
        float elapsed = 0f;
        float duration = 0.2f;
        
        // Store the current position
        Vector3 pos = transform.position;

        // While animation lasts
        while (elapsed < duration)
        {
            // Increase smoothly the position
            transform.position = Vector3.Lerp(pos, newPos, elapsed / duration);
            // Update elapsed time
            elapsed += Time.deltaTime;
            
            // Suspend the function till the next frame
            yield return null;
        }
        
        // Manually set the position in case it wasn't set precisely
        transform.position = newPos;
    }
}
