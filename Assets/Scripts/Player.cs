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
    private Dictionary<Vector3Int, GameObject> plantedPositions = new Dictionary<Vector3Int, GameObject>(); // planted positions with their corresponding planted items
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

    public void InteractWithGround() {
        Vector3Int position = GetPositionBasedOnDirection();

        if (plantedPositions.ContainsKey(position)) {
            Debug.Log("Planted position found at: " + position);
            if (plantedPositions[position] != null) {
                GameObject plantedObject = plantedPositions[position];
                
                 Animator plantAnimator = plantedObject.GetComponent<Animator>();
                if (plantAnimator != null){
                    Debug.Log("isDone? " + plantAnimator.GetBool("Done"));
                    // item newItem = new GameObject().AddComponent<item>();
                    // if (plantedPositions[position].name.Contains("wheat")){
                    //     newItem.SetItemData(GameManager.instance.item_Manager.GetItemByName("wheat_harvested").data);
                    //     inventory.Add("Backpack", newItem);
                    //     plantedPositions.Remove(position); 
                    // }
                    // else if(plantedPositions[position].name.Contains("tomato")){
                    //     newItem.SetItemData(GameManager.instance.item_Manager.GetItemByName("tomato_harvested").data);
                    //     inventory.Add("Backpack", newItem);
                    //     plantedPositions.Remove(position); 
                    // }
                }
            }
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
        else if (slot.itemName.Contains("seeds")) {
            HandleSeedPlanting(slot.itemName, highlightedSlot); // Handle seed planting
        }
        else {
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

    private void HandleSeedPlanting(string seedType, Slot_UI highlightedSlot) {
         // Get the position based on the player's direction
            Vector3Int position = GetPositionBasedOnDirection();

            // Check if the ground is plowed
            if (plowedPositions.Contains(position)) {
                // Get the Tilemap
                Tilemap tilemap = GameManager.instance.tileManager.GetTilemap();

                // Reference to Animated Tile
                //AnimatedTile animatedTile = GameManager.instance.tileManager.GetPlantingAnimatedTile();

                if (tilemap != null)
                {
                    // Replace the tile at the position with the Animated Tile
                    //tilemap.SetTile(position, animatedTile);

                    // Get the animation duration from the TileManager
                    //float animationDuration = GameManager.instance.tileManager.GetAnimationDuration();

                    highlightedSlot.inventory.Remove(highlightedSlot.slotID);

                    UIManager uiManager = FindFirstObjectByType<UIManager>();
                    if (uiManager != null)
                    {
                        uiManager.RefreshInventoryUI("Toolbar");
                    }
               // Remove the position from the plowedPositions HashSet
                if (plowedPositions.Contains(position))
                {
                    plowedPositions.Remove(position);
                }

                     // Convert the tilemap position to world position
                Vector3 worldPosition = tilemap.CellToWorld(position) + tilemap.tileAnchor;

        // Remove the animated tile from the tilemap
        //tilemap.SetTile(position, null);

            GameObject plantItem = null;
            // Reference to plant item from GameManager
            switch(seedType) {
                case "wheat seeds":
                    plantItem = GameManager.instance.plantItems[0]; // Wheat item
                    break;
                case "tomato seeds":
                    plantItem = GameManager.instance.plantItems[1]; // Tomato item
                    break;
                default:
                    Debug.Log("Unknown seed type: " + seedType);
                    break;
            }
            if (plantItem != null) {
                // Drop plant item at the world position
                Instantiate(plantItem, worldPosition, Quaternion.identity);
                plantedPositions.Add(position, plantItem); // Track the planted position
            }

            // Replace the tile with an interactable tile from the GameManager
            // if (GameManager.instance.tileManager != null)
            // {
            //     GameManager.instance.tileManager.GetTilemap().SetTile(position, GameManager.instance.tileManager.interactableTile);
            // }

                    // Start a coroutine to show animation of plant growth
                    //StartCoroutine(GrowPlant(seedType, tilemap, position));
                }
                else {
                Debug.Log("Cannot plant here. The ground is not plowed.");
             }
            }
    }

    private IEnumerator GrowPlant(string seedType, Tilemap tilemap, Vector3Int position, float animationDuration)
    {
        // Wait for the animation to complete
        yield return new WaitForSeconds(animationDuration);

        // Convert the tilemap position to world position
        Vector3 worldPosition = tilemap.CellToWorld(position) + tilemap.tileAnchor;

        // Remove the animated tile from the tilemap
        tilemap.SetTile(position, null);

        // Remove the position from the plowedPositions HashSet
        if (plowedPositions.Contains(position))
        {
            plowedPositions.Remove(position);
        }

        GameObject plantItem = null;
        // Reference to plant item from GameManager
        switch(seedType) {
            case "wheat seeds":
                plantItem = GameManager.instance.plantItems[0]; // Wheat item
                break;
            case "tomato seeds":
                plantItem = GameManager.instance.plantItems[1]; // Tomato item
                break;
            default:
                Debug.Log("Unknown seed type: " + seedType);
                break;
        }
        if (plantItem != null) {
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
