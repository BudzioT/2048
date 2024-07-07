using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Board of all the tiles
public class TileBoard : MonoBehaviour
{
    public Tile tilePrefab;

    public TileState[] tileStates;
    
    private TileGrid _tileGrid;
    private List<Tile> _tiles;

    private bool _wait;

    private void Awake()
    {
        // Initialize the components
        _tileGrid = GetComponentInChildren<TileGrid>();
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
        Tile tile = Instantiate(tilePrefab, _tileGrid.transform);
        tile.SetState(tileStates[0], 2);

        // Spawn the tile
        tile.Spawn(_tileGrid.GetRandomEmptyCell());
        // Add it to the list
        _tiles.Add(tile);
    }

    // Update the board
    private void Update()
    {
        // Don't allow the input if board is in wait state
        if (_wait)
            return;
        
        // Movement up
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveTiles(Vector2Int.up, 0, 1, 1, 1);
        }
        // Movement down
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveTiles(Vector2Int.down, 0, _tileGrid.Height - 2, 1, -1);
        }
        // Movement left
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveTiles(Vector2Int.left, 1, 0, 1, 1);
        }
        // Movement right
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveTiles(Vector2Int.right, _tileGrid.Width - 2, 0, -1, 1);
        }
    }

    // Move the tiles
    private void MoveTiles(Vector2Int direction, int startX, int startY, int incrX, int incrY)
    {
        bool changed = false;
        
        // Go through each of tile rows
        for (int x = startX; x >= 0 && x < _tileGrid.Width; x += incrX)
        {
            // And columns
            for (int y = startY; y >= 0 && y < _tileGrid.Height; y += incrY)
            {
                // Get the cell at this positions
                TileCell cell = _tileGrid.GetCell(x, y);

                // If it exists, move it
                if (cell.Occupied)
                {
                    // Save if there were any changes (OR because it's in a loop, there may be several changes)
                    changed |= MoveTile(cell.Tile, direction);
                }
            }
        }
        
        // If something changed, let the animation finish
        if (changed)
            StartCoroutine(WaitForChanges());
    }

    // Move single tile
    private bool MoveTile(Tile tile, Vector2Int direction)
    {
        // Cell that this one is moved to
        TileCell cell = null;
        TileCell adjacent = _tileGrid.GetAdjacentCell(tile.TileCell, direction);
        
        // Move the tiles till there isn't any adjacent cells
        while (adjacent != null)
        {
            // If the adjacent tile is occupied, merge it
            if (adjacent.Occupied)
            {
                // TODO: MERGE
                break;
            }

            // Get the next adjacent cell
            cell = adjacent;
            adjacent = _tileGrid.GetAdjacentCell(cell, direction);
        }
        
        // Move the tile into another cell
        if (cell != null)
        {
            tile.Move(cell);
            
            // Set the wait flag to true, so the animation will end properly and wait the required time
            _wait = true;
            
            //StartCoroutine(WaitForChanges());

            return true;
        }

        return false;
    }

    // Wait for changes and set back the wait flag to false
    private IEnumerator WaitForChanges()
    {
        // Activate the waiting time, in case it wasn't already
        _wait = true;

        // Wait for the animation to end (it lasts 0.2 seconds)
        yield return new WaitForSeconds(0.2f);
        // Set back the wait flag to false
        _wait = false;
        
        // TODO: Create a tile, check losing
    }
}
