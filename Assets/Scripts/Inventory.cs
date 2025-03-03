using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Inventory
{
    [System.Serializable]
   public class Slot
   {
        public string itemName;
        public int count;
        public int maxAllowed;

        public Sprite icon;

        public Slot()
        {
            itemName = "";
            count = 0;
            maxAllowed = 99;
        }

        public bool CanAddItem()
        {
            if(count<maxAllowed)
            {
                return true;
            }
            return false;
        }

        public void AddItem(item item)
        {
            this.itemName=item.data.itemName;
            this.icon = item.data.icon;
            count++;
        }

        public void RemoveItem()
        {
            if(count>0){
                count--;

                if(count==0)
                {
                    icon=null;
                    itemName="";
                }
            }
        }
   }

   public List<Slot> slots = new List<Slot>();

   public Inventory(int numSlots)
   {
        for (int i = 0; i < numSlots; i++)
        {
            Slot slot = new Slot();
            slots.Add(slot);
        }
   }

   public void Add(item item)
   {
        //check if others exist in a slot
        foreach(Slot slot in slots)
        {
            if (slot.itemName == item.data.itemName && slot.CanAddItem())
            {
                slot.AddItem(item);
                return;
            }
        }

        //add to empty slot if there is one
        foreach(Slot slot in slots)
        {
            if(slot.itemName == "")
            {
                slot.AddItem(item);
                return;
            }
        }
   }

   public void Remove(int index)
   {
        slots[index].RemoveItem();
   }
}

