using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class inventory_UI : MonoBehaviour
{

    public string inventoryName;

    public List<Slot_UI> slots = new List<Slot_UI>();

    private Canvas canvas;


    private Inventory inventory;

    // Update is called once per frame
    void Awake()
    {
        canvas = FindFirstObjectByType<Canvas>();
    }

    private void Start()
    {
        inventory = GameManager.instance.player.inventory.GetInventoryByName(inventoryName);
        SetupSlots();
        Refresh();
    }


    public void Refresh()
    {
        
        if (inventory == null)
        {
            return;
        }
        
        if(slots.Count == inventory.slots.Count)
        {
            for(int i=0; i<slots.Count; i++)
            {
                if(inventory.slots[i].itemName != "")
                {
                    slots[i].SetItem(inventory.slots[i]);
                }
                else
                {
                    slots[i].SetEmpty();
                }
            }
        }
        
    }

    public void Remove() {
        Debug.Log("inventory_UI Remove triggered");
        if (UIManager.draggedSlot != null) {
            item itemToDrop = GameManager.instance.item_Manager.GetItemByName(inventory.slots[UIManager.draggedSlot.slotID].itemName);

            if (itemToDrop != null) {
                if(UIManager.dragSingle){
                    GameManager.instance.player.DropItem(itemToDrop);
                    inventory.Remove(UIManager.draggedSlot.slotID);
                }
                else{
                    GameManager.instance.player.DropItem(itemToDrop,inventory.slots[UIManager.draggedSlot.slotID].count);
                    inventory.Remove(UIManager.draggedSlot.slotID, inventory.slots[UIManager.draggedSlot.slotID].count);
                }

                GameManager.instance.uiManager.RefreshAll();
            }
            UIManager.draggedSlot = null;
        }
    }

    public void SlotBeginDrag(Slot_UI slot) {
        if (slot != null) {
            //Debug.Log("Begin dragging " + slot.name);
            UIManager.draggedSlot=slot;
            UIManager.draggedIcon = Instantiate(UIManager.draggedSlot.itemIcon);
            UIManager.draggedIcon.raycastTarget=false;
            UIManager.draggedIcon.rectTransform.sizeDelta = new Vector2(50f,50f);
            UIManager.draggedIcon.transform.SetParent(canvas.transform);

            MoveToMousePosition(UIManager.draggedIcon.gameObject);
        }
    }

    public void SlotDrag() {
        
        if (UIManager.draggedIcon != null) {
            //Debug.Log("Dragging");
            MoveToMousePosition(UIManager.draggedIcon.gameObject);
        }
    }

    public void SlotEndDrag() {
        if (UIManager.draggedIcon != null) {
            //Debug.Log("Done dragging");
            Destroy(UIManager.draggedIcon.gameObject);
            UIManager.draggedIcon = null;
        }
    }

    public void SlotDrop(Slot_UI slot) {
        if (slot != null && UIManager.draggedSlot != null) {
            //Debug.Log("Dropped " + UIManager.draggedSlot.name);
            if (UIManager.dragSingle) {
                UIManager.draggedSlot.inventory.MoveSlot(UIManager.draggedSlot.slotID, slot.slotID, slot.inventory);
            }
            else {
                UIManager.draggedSlot.inventory.MoveSlot(UIManager.draggedSlot.slotID, slot.slotID, slot.inventory, UIManager.draggedSlot.inventory.slots[UIManager.draggedSlot.slotID].count);
            }
                
            GameManager.instance.uiManager.RefreshAll();
        }
    }

    private void MoveToMousePosition(GameObject toMove)
    {
        if(canvas != null)
        {
            Vector2 position;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, null, out position);
            toMove.transform.position = canvas.transform.TransformPoint(position);
        }
    }

    void SetupSlots()
    {
        int counter = 0;
        foreach(Slot_UI slot in slots){
            slot.slotID = counter;
            counter++;
            slot.inventory = inventory;
        }
    }

}
