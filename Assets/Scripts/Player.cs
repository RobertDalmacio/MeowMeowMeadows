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
