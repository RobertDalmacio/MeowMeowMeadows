using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public TileManager tileManager;

    public itemManager item_Manager;

    public UIManager uiManager;

    public Player player;

    public GameObject plantItem; 

    private void Awake()
    {
        if(instance !=null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else{
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);

        item_Manager = GetComponent<itemManager>();
        tileManager = GetComponent<TileManager>();
        uiManager = GetComponent<UIManager>();

        player = FindAnyObjectByType<Player>();
    }
}
