using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


// Grid of tiles
public class TileGrid : MonoBehaviour
{
    // Rows of the grid
    public TileRow[] Rows { get; private set; }
    // Cells of the grid
    public TileCell[] Cells { get; private set; }
    
    // Size of grid, determined by numbers of cells and rows

    public int Size => Cells.Length;
    public int Height => Rows.Length;
    public int Width => Size / Height;

    private void Awake()
    {
        // Get the rows and cells
        Rows = GetComponentsInChildren<TileRow>();
        Cells = GetComponentsInChildren<TileCell>();
    }
    
    // Automatically runs on the first frame when script is enabled
    private void Start()
    {
        // Go through each row
        for (int y = 0; y < Rows.Length; ++y)
        {
            // Go through each column inside the row
            for (int x = 0; x < Rows[y].cells.Length; ++x)
            {
                // Save the cell positions
                Rows[y].cells[x].Position = new Vector2Int(x, y);
            }
        }
    }

    // Get a random cell that is empty
    public TileCell GetRandomEmptyCell()
    {
        // Get random index
        int index = Random.Range(0, Cells.Length);
        // Save it
        int startIndex = index;
        
        // Loop until none occupied cell is found
        while (Cells[index].Occupied)
        {
            // Increase the cell index
            ++index;

            // If it is too high, reset it to 0
            if (index >= Cells.Length)
                index = 0;
            
            // If all cells were checked, return null
            if (index == startIndex)
                return null;
        }
        
        // Return the empty cell
        return Cells[index];
    }

    // Return a cell from the given position
    public TileCell GetCell(int x, int y)
    {
        // Validate the position, return the cell
        if ((x >= 0 && x < Width) && (y >= 0 && y < Height))
            return Rows[y].cells[x];
        // If this position wasn't proper, return null
        return null;
    }
    
    // Return a cell from the given position vector
    public TileCell GetCell(Vector2Int position)
    {
        // Validate the position
        if ((position.x >= 0 && position.x < Width) && (position.y >= 0 && position.y < Height))
            return Rows[position.y].cells[position.x];
        // If not valid return null
        return null;
    }

    // Get the adjacent cell to the given one
    public TileCell GetAdjacentCell(TileCell cell, Vector2Int direction)
    {
        // Store the position, calculate it after moving
        Vector2Int position = cell.Position;
        position.x += direction.x;
        // Subtract because of reversed index to axis positions
        position.y -= direction.y;

        return GetCell(position);
    }
}
