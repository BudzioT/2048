using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


// Grid of tiles
public class Grid : MonoBehaviour
{
    // Rows of the grid
    public Row[] Rows { get; private set; }
    // Cells of the grid
    public Cell[] Cells { get; private set; }
    
    // Size of grid, determined by numbers of cells and rows

    public int Size => Cells.Length;
    public int Height => Rows.Length;
    public int Width => Size / Height;

    private void Awake()
    {
        // Get the rows and cells
        Rows = GetComponentsInChildren<Row>();
        Cells = GetComponentsInChildren<Cell>();
    }
    
    // Automatically runs on the first frame when script is enabled
    private void Start()
    {
        // Go through each row
        for (int y = 0; y < Rows.Length; ++y)
        {
            // Go through each column inside the row
            for (int x = 0; x < Rows[y].Cells.Length; ++x)
            {
                // Save the cell positions
                Rows[y].Cells[x].Position = new Vector2Int(x, y);
            }
        }
    }

    // Get a random cell that is empty
    public Cell GetRandomEmptyCell()
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
}
