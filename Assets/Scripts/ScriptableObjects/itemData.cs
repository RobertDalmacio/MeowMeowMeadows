using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "Item Data", order = 50)]
public class itemData : ScriptableObject
{
    public string itemName = "Item Name";
    public Sprite icon;
}
