using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class inventory_UI : MonoBehaviour
{

    public GameObject inventoryPanel;
    public Button openIventory;

    public Player player;

    public List<Slot_UI> slots = new List<Slot_UI>();
    // Update is called once per frame
    void Awake()
    {
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
        
        if (itemToDrop != null)
        {
            player.DropItem(itemToDrop);
            player.inventory.Remove(slotID);
            Refresh();
        }
        
    }
}
