using UnityEngine;

public class Collectable : MonoBehaviour
{
    //player walks into collectable
    //add collectable to player
    //delete collectable from screen
    public CollectableType type;
    public Sprite icon;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player){
            player.inventory.Add(this);
            Destroy(this.gameObject);
        }
    }
}

public enum CollectableType
{
    NONE, WHEAT_SEED
}
