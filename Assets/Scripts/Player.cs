using Unity.VisualScripting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    public InventoryManager inventory;
    public Button actionButton;
    public GameObject toolbar;
    public SpriteRenderer characterDirection;

    public Sprite leftLook;
    public Sprite leftLook2;
    public Sprite rightLook;
    public Sprite rightLook2;
    public Sprite frontLook;
    public Sprite frontLook2;

    public Animator animator;

    private HashSet<Vector3Int> plowedPositions = new HashSet<Vector3Int>(); // HashSet to keep track of plowed positions

    private void Awake()
    {
        inventory = GetComponent<InventoryManager>();
        actionButton.onClick.AddListener(InteractWithGround);
    }

    

    public void DropItem(item item)
    {
        Vector3 spawnLocation = transform.position;
        Vector3 spawnOffset = Random.insideUnitCircle * 1.25f;
        item droppedItem = Instantiate(item, spawnLocation + spawnOffset, Quaternion.identity);
        droppedItem.rb2d.AddForce(spawnOffset * 2f, ForceMode2D.Impulse);
    }

    public void DropItem(item item, int numDrop)
    {
        for (int i = 0; i < numDrop; i++)
        {
            DropItem(item);
        }
    }

    private Slot_UI GetHighlightedToolbarSlot()
    {
        Slot_UI[] toolbarSlots = toolbar.GetComponentsInChildren<Slot_UI>();
        // Loop through all toolbar slots
        foreach (Slot_UI slot in toolbarSlots)
        {
            // Find slot that is highlighted
            if (slot.highlight.activeSelf)
            {
                return slot;
            }
        }
        return null; // No highlighted slot found
    }

    public void InteractWithGround()
    {
        // Get the position based on the player's direction
        Vector3Int position = GetPositionBasedOnDirection();

        // Get the Tilemap
        Tilemap tilemap = GameManager.instance.tileManager.GetTilemap();

        //check if wheat plant
        if(tilemap.GetTile(position)==GameManager.instance.tileManager.GetWheatTile())
        {
            harvestWheat(tilemap,position);
        }

        //check if tomato plant
        if(tilemap.GetTile(position)==GameManager.instance.tileManager.GetTomatoTile())
        {
            harvestTomato(tilemap,position);
        }
        // Get the highlighted toolbar slot
        Slot_UI highlightedSlot = GetHighlightedToolbarSlot();

        if (highlightedSlot == null || highlightedSlot.inventory == null)
        {
            Debug.Log("No highlighted slot or inventory is null.");
            return;
        }

        // Get the slot from the inventory
        Inventory.Slot slot = highlightedSlot.inventory.slots[highlightedSlot.slotID];

        if (slot.IsEmpty)
        {
            Debug.Log("Highlighted slot is empty.");
            return;
        }

        // Check if the item is a hoe
        if (slot.itemName == "Hoe")
        {
            animator.SetTrigger("HOE"); // start playing the animation first before setting the tile
            StartCoroutine(PlowGround(position));
        }
        else if (slot.itemName == "wheat seeds")
        {
            

            // Check if the ground is plowed
            if (plowedPositions.Contains(position))
            {
                

                // Reference to Animated Tile
                AnimatedTile animatedTile = GameManager.instance.tileManager.GetPlantingAnimatedTile();

                if (tilemap != null && animatedTile != null)
                {
                    // Replace the tile at the position with the Animated Tile
                    tilemap.SetTile(position, animatedTile);

                    // Get the animation duration from the TileManager
                    float animationDuration = GameManager.instance.tileManager.GetAnimationDuration();

                    highlightedSlot.inventory.Remove(highlightedSlot.slotID);

                    UIManager uiManager = FindFirstObjectByType<UIManager>();
                    if (uiManager != null)
                    {
                        uiManager.RefreshInventoryUI("Toolbar");
                    }

                    // Start a coroutine to show animation of plant growth
                    StartCoroutine(GrowWheat(tilemap,position, animationDuration));

                    //setWheatTile(tilemap,position);
                }
            }
            else
            {
                Debug.Log("Cannot plant here. The ground is not plowed.");
            }
        }
        else if (slot.itemName == "tomato seeds")
        {

            // Check if the ground is plowed
            if (plowedPositions.Contains(position))
            {

                // Reference to Animated Tile
                AnimatedTile animatedTile = GameManager.instance.tileManager.GetPlantingAnimatedTomatoTile();

                if (tilemap != null && animatedTile != null)
                {
                    // Replace the tile at the position with the Animated Tile
                    tilemap.SetTile(position, animatedTile);

                    // Get the animation duration from the TileManager
                    float animationDuration = GameManager.instance.tileManager.GetAnimationTomatoDuration();

                    highlightedSlot.inventory.Remove(highlightedSlot.slotID);

                    UIManager uiManager = FindFirstObjectByType<UIManager>();
                    if (uiManager != null)
                    {
                        uiManager.RefreshInventoryUI("Toolbar");
                    }

                    // Start a coroutine to show animation of plant growth
                    StartCoroutine(GrowTomato(tilemap,position,animationDuration));
                    
                }
            }
            else
            {
                Debug.Log("Cannot plant here. The ground is not plowed.");
            }
        }
        else
        {
            // TODO: Add interaction for other items
            Debug.Log($"Highlighted slot contains: {slot.itemName}. No interaction defined.");
        }
    }

    public IEnumerator PlowGround(Vector3Int position)
    {
        yield return new WaitForSeconds(0.9f);

        // Check if the position is interactable
        if (GameManager.instance.tileManager.IsInteractable(position))
        {
            // Plow the ground
            GameManager.instance.tileManager.SetInteracted(position);

            // Track the plowed position
            plowedPositions.Add(position);

            Debug.Log("Plowed the ground at position: " + position);
        }
    }


    private IEnumerator GrowTomato(Tilemap tilemap, Vector3Int position, float animationDuration)
    {
        // Wait for the animation to complete
        yield return new WaitForSeconds(animationDuration);
        setTomatoTile(tilemap,position);
        
    }

    private IEnumerator GrowWheat(Tilemap tilemap, Vector3Int position, float animationDuration)
    {
        // Wait for the animation to complete
        yield return new WaitForSeconds(animationDuration);
        setWheatTile(tilemap,position);
        
    }

    public void setWheatTile(Tilemap tilemap, Vector3Int position)
    {
        tilemap.SetTile(position, GameManager.instance.tileManager.GetWheatTile());
    }

    public void setTomatoTile(Tilemap tilemap, Vector3Int position)
    {
        tilemap.SetTile(position, GameManager.instance.tileManager.GetTomatoTile());
    }

     public void harvestWheat(Tilemap tilemap, Vector3Int position)
    {
        // Convert the tilemap position to world position
        Vector3 worldPosition = tilemap.CellToWorld(position) + tilemap.tileAnchor;

        // Remove the animated tile from the tilemap
        tilemap.SetTile(position, null);

        // Remove the position from the plowedPositions HashSet
        if (plowedPositions.Contains(position))
        {
            plowedPositions.Remove(position);
        }

        // Reference to plant item from GameManager
        GameObject plantItem = GameManager.instance.wheatItem;
        if (plantItem != null)
        {
            // Drop plant item at the world position
            Instantiate(plantItem, worldPosition, Quaternion.identity);
        }

        // Replace the tile with an interactable tile from the GameManager
        if (GameManager.instance.tileManager != null)
        {
            GameManager.instance.tileManager.GetTilemap().SetTile(position, GameManager.instance.tileManager.interactableTile);
        }
    }

    public void harvestTomato(Tilemap tilemap, Vector3Int position)
    {
        // Convert the tilemap position to world position
        Vector3 worldPosition = tilemap.CellToWorld(position) + tilemap.tileAnchor;
        // Remove the position from the plowedPositions HashSet
        if (plowedPositions.Contains(position))
        {
            plowedPositions.Remove(position);
        }

        // Reference to plant item from GameManager
        GameObject plantItem = GameManager.instance.tomatoItem;
        if (plantItem != null)
        {
            // Drop plant item at the world position
            Instantiate(plantItem, worldPosition, Quaternion.identity);
        }

        // Replace the tile with an interactable tile from the GameManager
        if (GameManager.instance.tileManager != null)
        {
            GameManager.instance.tileManager.GetTilemap().SetTile(position, GameManager.instance.tileManager.interactableTile);
        }
    }

    public void ShowInteractablePosition()
    {
        Vector3Int position = GetPositionBasedOnDirection();
        //GameManager.instance.tileManager.HighlightTile(position);
    }

    private Vector3Int GetPositionBasedOnDirection()
    {
        Debug.Log("Character at position (x, y): " + transform.position.x + ", " + transform.position.y);

        int x = (int)(Mathf.Round(transform.position.x));
        float xOffset = Mathf.Abs(Mathf.Abs(transform.position.x) - Mathf.Abs(x));
        int y = (int)(Mathf.Round(transform.position.y));
        float yOffset = Mathf.Abs(Mathf.Abs(transform.position.y) - Mathf.Abs(y));
        Debug.Log("Rounded position (x, y): " + x + ", " + y);

        if (characterDirection.sprite == leftLook || characterDirection.sprite == leftLook2)
        {
            Debug.Log("Facing left");
            x += 1;
            y += 1;
        }
        else if (characterDirection.sprite == rightLook || characterDirection.sprite == rightLook2)
        {
            Debug.Log("Facing right");
            x += 2;
            y += 1;
        }
        else if (characterDirection.sprite == frontLook || characterDirection.sprite == frontLook2)
        {
            Debug.Log("Facing front");
            x += 2;
        }
        else
        {
            // Looking up
            Debug.Log("Facing up");
            x += 1;
            y += 2;
        }

        return new Vector3Int(x, y, 0);
    }
}
