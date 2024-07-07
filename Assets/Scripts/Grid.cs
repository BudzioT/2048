using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
}
