using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Singular cell
public class TileCell : MonoBehaviour
{
    // Position of the cell
    public Vector2Int Position { get; set; }
    // Tile that occupies it
    public Tile Tile { get; set; }

    // Tile empty flag
    public bool Empty => Tile == null;
    // Tile occupied flag
    public bool Occupied => Tile != null;
}
