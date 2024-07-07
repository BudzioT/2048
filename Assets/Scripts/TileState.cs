using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Add the tile state to the assets menu
[CreateAssetMenu(menuName = "Tile State")]
// Tile state
public class TileState : ScriptableObject
{
    // Colors of the tile
    public Color backgroundColor;
    public Color textColor;
}
