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
            Setup();
        }
        else{
            inventoryPanel.SetActive(false);
        }
    }

    void Setup()
    {
        if(slots.Count == player.inventory.slots.Count)
        {
            for(int i=0; i<slots.Count; i++)
            {
                if(player.inventory.slots[i].type != CollectableType.NONE)
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
}
