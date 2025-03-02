using System;
using System.Collections.Generic;
using UnityEngine;

public class itemManager : MonoBehaviour
{
    public item[] items;

    private Dictionary<string, item> nameToItemDict = new Dictionary<string, item>();

    private void Awake()
    {
        foreach(item item in items)
        {
            AddItem(item);
        }
    }

    private void AddItem(item item)
    {
        if(!nameToItemDict.ContainsKey(item.data.itemName))
        {
            nameToItemDict.Add(item.data.itemName, item);
        }
    }

    public item GetItemByName(string key)
    {
        if(nameToItemDict.ContainsKey(key))
        {
            return nameToItemDict[key];
        }

        return null;
    }
}
