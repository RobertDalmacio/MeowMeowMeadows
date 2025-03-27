using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.Search;

public class Player : MonoBehaviour
{
    public InventoryManager inventory;

    public Button actionButton;

    public SpriteRenderer characterDirection;

    public Sprite leftLook;
    public Sprite leftLook2;

    public Sprite rightLook;
    public Sprite rightLook2;

    public Sprite frontLook;
    public Sprite frontLook2;

    private Vector3Int positionTile;

    private void Awake()
    {
        inventory = GetComponent<InventoryManager>();
        actionButton.onClick.AddListener(InteractWithGround);
    }

    

    public void DropItem(item item)
    {
        Vector3 spawnLocation = transform.position;


        Vector3 spawnOffset = Random.insideUnitCircle * 1.25f;

        item droppedItem = Instantiate(item, spawnLocation + spawnOffset,Quaternion.identity);

        droppedItem.rb2d.AddForce(spawnOffset * 2f, ForceMode2D.Impulse);
    }

    public void DropItem(item item, int numDrop)
    {
        for(int i=0; i< numDrop; i++)
        {
            DropItem(item);
        }
    }

    public void InteractWithGround()
    {

        Vector3Int position;
        if(characterDirection.sprite == leftLook || characterDirection.sprite == leftLook2){
                position = new Vector3Int((int)transform.position.x-1, (int)transform.position.y - 1,0);
            }
        else if (characterDirection.sprite == rightLook || characterDirection.sprite == rightLook2){
                position = new Vector3Int((int)transform.position.x+1, (int)transform.position.y-1 ,0);
           
            }
        else if (characterDirection.sprite == frontLook || characterDirection.sprite == frontLook2){
                position = new Vector3Int((int)transform.position.x, (int)transform.position.y-2 ,0);
           
        }
        else{
            position = new Vector3Int((int)transform.position.x, (int)transform.position.y ,0);
           
        }
        
        if(positionTile!=position && GameManager.instance.tileManager.IsInteractable(position))
        {
            
            GameManager.instance.tileManager.SetNonInteractable(positionTile);
            positionTile = new Vector3Int(position.x, position.y,0);
            GameManager.instance.tileManager.SetInteractable(positionTile);
        }
        else if(positionTile==position)
        {
            GameManager.instance.tileManager.SetInteracted(position);
        }

    }

    public void ShowInteractablePosition()
    {
        Vector3Int position = new Vector3Int((int)transform.position.x, (int)transform.position.y,0);
        if(positionTile!=position && GameManager.instance.tileManager.IsInteractable(position))
        {
            GameManager.instance.tileManager.SetNonInteractable(positionTile);
            positionTile = new Vector3Int((int)transform.position.x, (int)transform.position.y,0);
            GameManager.instance.tileManager.SetInteractable(positionTile);
        }
        
    }
}
