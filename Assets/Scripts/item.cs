using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class item : MonoBehaviour
{
    public itemData data; // Reference to the ScriptableObject itemData

    [HideInInspector] public Rigidbody2D rb2d;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    public void SetItemData(itemData newItemData)
    {
        data = newItemData;  // Assign the item data (this could be done when the item is created)
    }
}
