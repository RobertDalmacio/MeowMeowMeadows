using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    //public TextMeshProUGUI storeTitle;
    public TextMeshProUGUI moneyText;

    // Buttons for buying and selling seeds
    public Button buyWheatButton;
    public Button sellWheatButton;
    public Button buyTomatoButton;
    public Button sellTomatoButton;

    public itemData wheatSeedData; // Reference to the itemData for wheat seeds
    public itemData tomatoSeedData; // Reference to the itemData for tomato seeds

    // Seed prices
    public int wheatSeedPrice = 10;
    public int tomatoSeedPrice = 15;

    // Player money
    public int playerMoney = 100;

    private InventoryManager inventoryManager; // Reference to inventory manager

    void Start()
    {
        inventoryManager = GameManager.instance.inventoryManager; // Reference to the inventory manager
        UpdateMoneyUI();

        // Set up buttons' onClick listeners
        buyWheatButton.onClick.AddListener(() => BuySeed("wheat seeds", wheatSeedPrice));
        sellWheatButton.onClick.AddListener(() => SellSeed("wheat seeds", wheatSeedPrice));
        buyTomatoButton.onClick.AddListener(() => BuySeed("tomato seeds", tomatoSeedPrice));
        sellTomatoButton.onClick.AddListener(() => SellSeed("tomato seeds", tomatoSeedPrice));
    }

    // Update the player's money UI
    void UpdateMoneyUI()
    {
        moneyText.text = "Money: $" + playerMoney.ToString();
    }

    // Handle buying seeds
    void BuySeed(string seedType, int seedPrice)
    {
        if (playerMoney >= seedPrice)
        {
            Debug.Log("HERE " );
            playerMoney -= seedPrice;
            itemData selectedSeed = seedType == "Wheat" ? wheatSeedData : tomatoSeedData;
        
        // Create the item in the game world
        item newItem = new GameObject().AddComponent<item>();
        newItem.SetItemData(selectedSeed);

        // Add the item to the player's inventory (Backpack)
        inventoryManager.Add("Backpack", newItem);
            
        }
        else
        {
            Debug.Log("Not enough money to buy " + seedType);
        }
    }
    

    // Handle selling seeds
    void SellSeed(string seedType, int seedPrice)
    {
        itemData selectedSeed = seedType == "Wheat" ? wheatSeedData : tomatoSeedData;

        // Assuming you want to remove an item from the inventory
        Inventory backpackInventory = inventoryManager.GetInventoryByName("Backpack");
        foreach (var slot in backpackInventory.slots)
        {
            if (slot.itemName == selectedSeed.itemName && slot.count > 0)
            {
                slot.RemoveItem(); // Remove one seed
                playerMoney += seedPrice; //decrease money
                UpdateMoneyUI();
                break;
            }
        }
        
    }

}


