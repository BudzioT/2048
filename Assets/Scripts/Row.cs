using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Row of tiles
public class Row : MonoBehaviour
{
    // Array of cells that occupies the row
    public Cell[] Cells { get; private set; }

    // Enabled on loading the script
    private void Awake()
    {
        // Get the cells that are in the row
        Cells = GetComponentsInChildren<Cell>();
    }
}
