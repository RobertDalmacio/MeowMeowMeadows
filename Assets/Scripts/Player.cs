using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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
    //public AnimationClip hoe_forward;
    public Animator animator;

    private Vector3Int positionTile;


    private void Awake()
    {
        inventory = GetComponent<InventoryManager>();
        actionButton.onClick.AddListener(InteractWithGround);
    }
    
    public void DropItem(item item)
    {
        Vector3 spawnLocation = transform.position;


        Vector3 spawnOffset = Random.insideUnitCircle * 1.25f;

        item droppedItem = Instantiate(item, spawnLocation + spawnOffset,Quaternion.identity);

        droppedItem.rb2d.AddForce(spawnOffset * 2f, ForceMode2D.Impulse);
    }

    public void DropItem(item item, int numDrop)
    {
        for(int i=0; i< numDrop; i++)
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
            PlowGround();
        }
        else
        {
            // TODO: Add interaction for other items
            Debug.Log($"Highlighted slot contains: {slot.itemName}. No interaction defined.");
        }
    }

    public void PlowGround()
    {
        // Get the position based on the player's direction
        Vector3Int position = GetPositionBasedOnDirection();

        // Check if the position is interactable
        if (!GameManager.instance.tileManager.IsInteractable(position))
        {
            return;
        }

        animator.SetTrigger("HOE");

        // Plow the ground
        GameManager.instance.tileManager.SetInteracted(position);
        Debug.Log("Plowed the ground at position: " + position);
        
    }

    public void ShowInteractablePosition()
    {
        Vector3Int position = GetPositionBasedOnDirection();

        if (positionTile != position && GameManager.instance.tileManager.IsInteractable(position))
        {
            GameManager.instance.tileManager.SetNonInteractable(positionTile);
            positionTile = position;
            GameManager.instance.tileManager.SetInteractable(positionTile);
        }
    }

    private Vector3Int GetPositionBasedOnDirection()
    {
        if (characterDirection.sprite == leftLook || characterDirection.sprite == leftLook2)
        {
            return new Vector3Int((int)transform.position.x - 1, (int)transform.position.y - 1, 0);
        }
        else if (characterDirection.sprite == rightLook || characterDirection.sprite == rightLook2)
        {
            return new Vector3Int((int)transform.position.x + 1, (int)transform.position.y - 1, 0);
        }
        else if (characterDirection.sprite == frontLook || characterDirection.sprite == frontLook2)
        {
            return new Vector3Int((int)transform.position.x, (int)transform.position.y - 2, 0);
            
        }
        else
        {
            return new Vector3Int((int)transform.position.x, (int)transform.position.y, 0);
        }
    }
}
