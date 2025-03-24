using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    [SerializeField] private Tilemap interactableMap;
    [SerializeField] private Tile hiddenInteractableTile;
    [SerializeField] private Tile interactedTile;

    [SerializeField] private Tile darkerGrassTile;

    [SerializeField] private Tile normalGrassTile;

    public const string InteractableTag = "interactable";
    public const string InteractableVisibleTag = "Interactable_visible";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach(var position in interactableMap.cellBounds.allPositionsWithin)
        {
            TileBase tile = interactableMap.GetTile(position);

            if (tile != null && tile.name == InteractableVisibleTag)
            {
                interactableMap.SetTile(position, hiddenInteractableTile);
            }
            
        }
    }

    public bool IsInteractable(Vector3Int position)
    {
        TileBase tile = interactableMap.GetTile(position);

        if(tile != null)
        {
            if(tile.name == InteractableTag)
            {
                return true;
            }
        }

        return false;
    }

    public void SetInteracted(Vector3Int position)
    {
        interactableMap.SetTile(position, interactedTile);
        Debug.Log("Tile is interactable! " + position);
    }

    public void SetInteractable(Vector3Int position)
    {
        if(interactableMap.GetTile(position)!=interactedTile)
        {
            interactableMap.SetTile(position, darkerGrassTile);
            Debug.Log("Tile is not tilled yet! " + position);
        }
        //interactableMap.SetTile(position, darkerGrassTile);
    }

    public void SetNonInteractable(Vector3Int position)
    {
        if(interactableMap.GetTile(position)!=interactedTile)
        {
            interactableMap.SetTile(position, normalGrassTile);
            Debug.Log("Tile is not interactable yet! " + position);
        }
    }

}
