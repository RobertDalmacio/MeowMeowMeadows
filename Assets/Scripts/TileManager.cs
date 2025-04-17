using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    [SerializeField] private Tilemap interactableMap;
    [SerializeField] public Tile interactableTile;
    [SerializeField] private Tile interactedTile;

    [SerializeField] private Tile darkerGrassTile;

    [SerializeField] private Tile normalGrassTile;

    [SerializeField] private float animationSpeed = 1f;

    public const string InteractableTag = "interactable";
    public const string InteractableVisibleTag = "Interactable_visible";
    public AnimatedTile plantingAnimatedTile;
    public AnimatedTile plantingAnimatedTomatoTile;

    public Tile wheatEndTile;
    public Tile tomatoEndTile;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach(var position in interactableMap.cellBounds.allPositionsWithin)
        {
            TileBase tile = interactableMap.GetTile(position);
            
            if (tile != null)
            {
                Debug.Log(tile.name);
                //interactableMap.SetTile(position, hiddenInteractableTile);
            }
            
        }
    }

    public bool IsInteractable(Vector3Int position)
    {
        TileBase tile = interactableMap.GetTile(position);

        if(tile != null && tile.name == InteractableTag) {
            return true;
        }

        return false;
    }

    public void SetInteracted(Vector3Int position)
    {
        interactableMap.SetTile(position, interactedTile);
        Debug.Log("Tile is interactable! " + position);
    }

    public Tile GetWheatTile()
    {
        return wheatEndTile;
    }

    public Tile GetTomatoTile()
    {
        return tomatoEndTile;
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

    public Tilemap GetTilemap()
    {
        return interactableMap;
    }

    public AnimatedTile GetPlantingAnimatedTile()
    {
        return plantingAnimatedTile;
    }

    public AnimatedTile GetPlantingAnimatedTomatoTile()
    {
        return plantingAnimatedTomatoTile;
    }

    public float GetAnimationDuration()
    {
        if (plantingAnimatedTile != null && plantingAnimatedTile?.m_AnimatedSprites.Length > 0)
        {
            return plantingAnimatedTile.m_AnimatedSprites.Length / animationSpeed;
        }
        return 0f; // Default to 0 if no sprites are available
    }

    public float GetAnimationTomatoDuration()
    {
        if (plantingAnimatedTomatoTile != null && plantingAnimatedTomatoTile?.m_AnimatedSprites.Length > 0)
        {
            return plantingAnimatedTomatoTile.m_AnimatedSprites.Length / animationSpeed;
        }
        return 0f; // Default to 0 if no sprites are available
    }

}
