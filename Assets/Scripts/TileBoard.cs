using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Board of all the tiles
public class TileBoard : MonoBehaviour
{
    public Tile tilePrefab;

    public TileState[] tileStates;
    
    private Grid _grid;
    private List<Tile> _tiles;

    private void Awake()
    {
        // Initialize the components
        _grid = GetComponentInChildren<Grid>();
        // Set the minimum capacity to 16, for performence
        _tiles = new List<Tile>(16);
    }

    private void Start()
    {
        // Create the tile
        CreateTile();
        CreateTile();
    }

    // Create a new tile
    private void CreateTile()
    {
        // Instantiate a tile and set its parent to the grid
        Tile tile = Instantiate(tilePrefab, _grid.transform);
        tile.SetState(tileStates[0], 2);

        // Spawn the tile
        tile.Spawn(_grid.GetRandomEmptyCell());
        // Add it to the list
        _tiles.Add(tile);
    }
}
