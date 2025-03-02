using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    [SerializeField] private Tilemap interactableMap;
    [SerializeField] private Tile hiddenInteractableTile;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach(var position in interactableMap.cellBounds.allPositionsWithin)
        {
            interactableMap.SetTile(position, hiddenInteractableTile);
        }
    }

}
