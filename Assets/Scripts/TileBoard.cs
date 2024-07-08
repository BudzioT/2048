using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


// Board of all the tiles
public class TileBoard : MonoBehaviour
{
    public Tile tilePrefab;

    public TileState[] tileStates;
    
    private TileGrid _tileGrid;
    private List<Tile> _tiles;

    private bool _wait;

    public Manager manager;

    private void Awake()
    {
        // Initialize the components
        _tileGrid = GetComponentInChildren<TileGrid>();
        // Set the minimum capacity to 16, for performence
        _tiles = new List<Tile>(16);
    }

    // Create a new tile
    public void CreateTile()
    {
        // Instantiate a tile and set its parent to the grid
        Tile tile = Instantiate(tilePrefab, _tileGrid.transform);
        tile.SetState(tileStates[0], 2);

        // Spawn the tile
        tile.Spawn(_tileGrid.GetRandomEmptyCell());
        // Add it to the list
        _tiles.Add(tile);
    }
    
    // Clear the board
    public void ClearBoard()
    {
        // Clear all the tiles from the grid
        foreach (var cell in _tileGrid.Cells)
        {
            cell.Tile = null;
        }
        
        // Go through each tile and destroy it
        foreach (var tile in _tiles)
        {
            Destroy(tile.gameObject);
        }
        
        // Clear the tile list
        _tiles.Clear();
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
                // If tiles can merge, do it
                if (CanMerge(tile, adjacent.Tile))
                {
                    Merge(tile, adjacent.Tile);
                    
                    return true;
                }
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

            return true;
        }

        return false;
    }

    // Return if one tile can merge with the other
    private bool CanMerge(Tile lhs, Tile rhs)
    {
        // Tiles can merge when they have the same number and the tile merged to isn't locked
        return lhs.Number == rhs.Number && !rhs.Locked;
    }

    // Merge two tiles
    private void Merge(Tile lhs, Tile rhs)
    {
        // Remove the tile that merges into another from the tiles list
        _tiles.Remove(lhs);
        
        // Merge them
        lhs.Merge(rhs.TileCell);
        // Make sure index is right, if it exceeds the states, use the last one
        int index = Mathf.Clamp(GetIndexState(rhs.State) + 1, 0, tileStates.Length - 1);

        // Multiply the number
        int number = rhs.Number * 2;
        // Finally, set state of the tile that was merged into
        rhs.SetState(tileStates[index], number);
    }

    private bool CheckGameOver()
    {
        // If there is still place to place tiles, the player certainly didn't lose
        if (_tiles.Count != _tileGrid.Size)
            return false;

        // Check every tile and see if it can merge in minimum one direction
        foreach (var tile in _tiles)
        {
            // Get the adjacent tiles
            TileCell up = _tileGrid.GetAdjacentCell(tile.TileCell, Vector2Int.up);
            TileCell down = _tileGrid.GetAdjacentCell(tile.TileCell, Vector2Int.down);
            TileCell left = _tileGrid.GetAdjacentCell(tile.TileCell, Vector2Int.left);
            TileCell right = _tileGrid.GetAdjacentCell(tile.TileCell, Vector2Int.right);

            // Check if they exist and for ability to merge with every one of them
            if (up != null && CanMerge(tile, up.Tile))
                return false;
            else if (down != null && CanMerge(tile, down.Tile))
                return false;
            else if (left != null && CanMerge(tile, left.Tile))
                return false;
            else if (right != null && CanMerge(tile, right.Tile))
                return false;
        }

        // If no tile could merge, the player lost
        return true;
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
        
        // Unlock every tile
        foreach (var tile in _tiles)
        {
            tile.Locked = false;
        }

        // If there is still place for tiles, create one        
        if (_tiles.Count != _tileGrid.Size)
        {
            CreateTile();
        }

        // If there is a game over, let the game manager handle it
        if (CheckGameOver())
        {
            manager.GameOver();
        }
    }

    // Get the index of given tile state
    private int GetIndexState(TileState state)
    {
        // Go through each state and return the index if it's right
        for (int i = 0; i < tileStates.Length; ++i)
        {
            if (tileStates[i] == state)
                return i;
        }
        
        // Otherwise return -1 to indicate not founding one
        return -1;
    }
}
