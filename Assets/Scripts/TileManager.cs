using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    [SerializeField] private Tilemap interactableMap;
    [SerializeField] public Tile interactableTile;
    [SerializeField] private Tile interactedTile;

    [SerializeField] private Tile darkerGrassTile;   // Highlight tile
    [SerializeField] private Tile normalGrassTile;

    [SerializeField] private float animationSpeed = 1f;

    public const string InteractableTag = "interactable";
    public const string InteractableVisibleTag = "Interactable_visible";
    public AnimatedTile plantingAnimatedTile;

    private Vector3Int? highlightedTile = null;

    void Start()
    {
        foreach (var position in interactableMap.cellBounds.allPositionsWithin)
        {
            TileBase tile = interactableMap.GetTile(position);
            if (tile != null)
            {
                Debug.Log(tile.name);
            }
        }
    }

    public bool IsInteractable(Vector3Int position)
    {
        TileBase tile = interactableMap.GetTile(position);
        return tile != null && tile.name == InteractableTag;
    }

    public void SetInteracted(Vector3Int position)
    {
        interactableMap.SetTile(position, interactedTile);
        Debug.Log("Set as interacted tile at: " + position);
    }

    public void SetInteractable(Vector3Int position)
    {
        if (interactableMap.GetTile(position) != interactedTile)
        {
            interactableMap.SetTile(position, darkerGrassTile);
            highlightedTile = position;
            Debug.Log("Set as highlight (interactable) at: " + position);
        }
    }

    public void SetNonInteractable(Vector3Int position)
    {
        if (interactableMap.GetTile(position) != interactedTile)
        {
            interactableMap.SetTile(position, normalGrassTile);
            Debug.Log("Removed highlight at: " + position);
        }
    }

    public void ClearPreviousHighlight()
    {
        if (highlightedTile.HasValue)
        {
            SetNonInteractable(highlightedTile.Value);
            highlightedTile = null;
        }
    }

    public void HighlightTile(Vector3Int newPosition)
    {
        if (highlightedTile.HasValue && highlightedTile.Value == newPosition)
            return;

        ClearPreviousHighlight();

        if (IsInteractable(newPosition))
        {
            SetInteractable(newPosition);
        }
    }

    public Tilemap GetTilemap()
    {
        return interactableMap;
    }

    public AnimatedTile GetPlantingAnimatedTile()
    {
        return plantingAnimatedTile;
    }

    public float GetAnimationDuration()
    {
        if (plantingAnimatedTile != null && plantingAnimatedTile.m_AnimatedSprites.Length > 0)
        {
            return plantingAnimatedTile.m_AnimatedSprites.Length / animationSpeed;
        }
        return 0f;
    }
}
