using Unity.VisualScripting;
using System.Collections;
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
            animator.SetTrigger("HOE"); // start playing the animation first before setting the tile
            Vector3Int position = GetPositionBasedOnDirection(); // Get the position based on the player's direction
            StartCoroutine(PlowGround(position));
        }
        else
        {
            // TODO: Add interaction for other items
            Debug.Log($"Highlighted slot contains: {slot.itemName}. No interaction defined.");
        }
    }

    public IEnumerator PlowGround(Vector3Int position) {
        yield return new WaitForSeconds(0.9f);
        // Check if the position is interactable
        if (GameManager.instance.tileManager.IsInteractable(position)) {
            // Plow the ground
            GameManager.instance.tileManager.SetInteracted(position);
            Debug.Log("Plowed the ground at position: " + position);
        }
        
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

    private Vector3Int GetPositionBasedOnDirection() {
        Debug.Log("character at position (x,y): " + transform.position.x + ", " + transform.position.y);

        int x = (int) (Mathf.Round(transform.position.x));
        float xOffset = Mathf.Abs(Mathf.Abs(transform.position.x) - Mathf.Abs(x));
        int y = (int) (Mathf.Round(transform.position.y));
        float yOffset = Mathf.Abs(Mathf.Abs(transform.position.y) - Mathf.Abs(y));
        Debug.Log("rounded position (x,y): " + x + ", " + y);

        if (characterDirection.sprite == leftLook || characterDirection.sprite == leftLook2) {
            Debug.Log("Facing left");
            if (xOffset >= 0.2f){
                x -= 1;
            }

            y += 1;
        }
        else if (characterDirection.sprite == rightLook || characterDirection.sprite == rightLook2) {
            Debug.Log("Facing right");
            x += 2;
            y+= 1;
        }
        else if (characterDirection.sprite == frontLook || characterDirection.sprite == frontLook2) {
            Debug.Log("Facing front");
            x += 1;

            if (yOffset >= 0.3f) {
                y += 1;
            }
            
        }
        else { // looking up
            Debug.Log("Facing up");
            x += 1;
            y += 2;
        }

        return new Vector3Int(x, y, 0);
    }
}
