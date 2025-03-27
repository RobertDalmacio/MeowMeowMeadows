using UnityEngine;

[RequireComponent(typeof(item))]
public class Collectable : MonoBehaviour
{
   
    //player walks into collectable
    //add collectable to player
    //delete collectable from screen
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player){
            item item = GetComponent<item>();
            if(item !=null)
            {
                player.inventory.Add("Backpack", item);
                Destroy(this.gameObject);
            }
            
        }
    }
}


