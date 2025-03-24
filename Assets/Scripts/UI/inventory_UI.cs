using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class inventory_UI : MonoBehaviour
{

    public GameObject inventoryPanel;
    public Button openIventory;

    public Player player;

    public List<Slot_UI> slots = new List<Slot_UI>();

    private Canvas canvas;

    public Slot_UI draggedSlot;

    private Image draggedIcon;


    // Update is called once per frame
    void Awake()
    {
        canvas = FindFirstObjectByType<Canvas>();
        inventoryPanel.SetActive(false);
        openIventory.onClick.AddListener(ToggleInventory);
    }
    

    public void ToggleInventory(){
        if(!inventoryPanel.activeSelf){
            inventoryPanel.SetActive(true);
            Refresh();
        }
        else{
            inventoryPanel.SetActive(false);
        }
    }

    void Refresh()
    {
        if(slots.Count == player.inventory.slots.Count)
        {
            for(int i=0; i<slots.Count; i++)
            {
                if(player.inventory.slots[i].itemName != "")
                {
                    slots[i].SetItem(player.inventory.slots[i]);
                }
                else
                {
                    slots[i].SetEmpty();
                }
            }
        }
    }

    public void Remove(int slotID)
    {
        item itemToDrop = GameManager.instance.item_Manager.GetItemByName(player.inventory.slots[slotID].itemName);
        Debug.Log("here!");
        if (itemToDrop != null)
        {
            Debug.Log("here!");
            player.DropItem(itemToDrop);
            player.inventory.Remove(slotID);
            Refresh();
        }
        
    }

    public void SlotBeginDrag(Slot_UI slot)
    {
        draggedSlot=slot;

        draggedIcon = Instantiate(draggedSlot.itemIcon);
        draggedIcon.raycastTarget=false;
        draggedIcon.rectTransform.sizeDelta = new Vector2(50f,50f);
        draggedIcon.transform.SetParent(canvas.transform);

        MoveToMousePosition(draggedIcon.gameObject);
        Debug.Log("Start Drag: " + draggedSlot.name);
    }

    public void SlotDrag()
    {
        MoveToMousePosition(draggedIcon.gameObject);
        Debug.Log("Dragging : " + draggedSlot.name);
    }

    public void SlotEndDrag()
    {
        Destroy(draggedIcon.gameObject);
        draggedIcon = null;
        Debug.Log("Done dragging: "+ draggedSlot.name);
    }

    public void SlotDrop(Slot_UI slot)
    {
        Debug.Log("Dropped: " + draggedSlot.name + " on " + slot.name);
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

}
