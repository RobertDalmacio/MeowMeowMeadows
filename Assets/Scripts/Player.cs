using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Inventory inventory;

    public Button actionButton;

    private void Awake()
    {
        inventory = new Inventory(21);
        actionButton.onClick.AddListener(InteractWithGround);
    }

    public void DropItem(item item)
    {
        Vector3 spawnLocation = transform.position;


        Vector3 spawnOffset = Random.insideUnitCircle * 1.25f;

        item droppedItem = Instantiate(item, spawnLocation + spawnOffset,Quaternion.identity);

        droppedItem.rb2d.AddForce(spawnOffset * 2f, ForceMode2D.Impulse);
    }

    public void InteractWithGround()
    {

        Vector3Int position = new Vector3Int((int)transform.position.x, (int)transform.position.y,0);

        if(GameManager.instance.tileManager.IsInteractable(position))
        {
            Debug.Log("Tile is interactable!");
            GameManager.instance.tileManager.SetInteracted(position);
        }

    }
}
