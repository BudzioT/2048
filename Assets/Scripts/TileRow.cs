using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Row of tiles
public class TileRow : MonoBehaviour
{
    // Array of cells that occupies the row
    public TileCell[] cells { get; private set; }

    // Enabled on loading the script
    private void Awake()
    {
        // Get the cells that are in the row
        cells = GetComponentsInChildren<TileCell>();
    }
}
